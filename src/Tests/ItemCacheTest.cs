using System;
using CompositeData;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class ItemCacheTests
    {
        [Fact]
        public void Items_WhenSameCompositeItemAdded_ThenEachCompositePersists()
        {
            // Given
            ItemCache sut = new ItemCacheFixture();
            sut.Filter(ItemType.Thing1);
            sut.AddOrUpdate(1, ItemType.Thing1);

            sut.Items
                .Should()
                .NotBeEmpty();

            sut.Filter(ItemType.Thing2);
            sut.AddOrUpdate(1, ItemType.Thing2);

            sut.Items
                .Should()
                .NotBeEmpty();

            // When
            // POINT: [rlittlesii] if I filter back to Thing2, it's item persists
            sut.Filter(ItemType.Thing1);

            // Then
            sut.Items
                .Should()
                .NotBeEmpty()
                .And
                .ContainSingle(x => x.Item.Id == 1);
        }

        [Fact]
        public void Items_WhenDifferentCompositeItemAdded_ThenEachCompositePersists()
        {
            // Given
            ItemCache sut = new ItemCacheFixture();
            sut.Filter(ItemType.Thing1);
            sut.AddOrUpdate(1, ItemType.Thing1);

            // When
            sut.Filter(ItemType.Thing2);
            sut.AddOrUpdate(2, ItemType.Thing2);
            sut.Filter(ItemType.Thing1);

            // Then
            sut.Items
                .Should()
                .ContainSingle(x => x.Item.Id == 1);
        }

    }

    public class ItemCacheFixture
    {
        public static implicit operator ItemCache(ItemCacheFixture fixture) => fixture.Build();

        private ItemCache Build() => new ItemCache(new ItemRepository());
    }
}