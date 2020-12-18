using System;
using System.Collections.Generic;

namespace CompositeData
{
    public readonly struct CompositeKey<T> : IEquatable<CompositeKey<T>>
        where T : Enumeration
    {
        public CompositeKey(Item item, T key)
        {
            Item = item;
            Key = key;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// Gets the key for the item.
        /// </summary>
        public T Key { get; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(CompositeKey<T> other) => Equals(Item.Id, other.Item.Id) && Key.Equals(other.Key);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is CompositeKey<T> other && Equals(other);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Item.Id.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Key);
            }
        }
    }
}