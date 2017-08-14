using System;

namespace DtronixModel
{
    /// <summary>
    /// Class to handle attribute retrieval and caching of results.
    /// </summary>
    /// <typeparam name="TObject">Main class that the attribute is attached to.</typeparam>
    /// <typeparam name="TAttribute">Attribute type to retrieve.</typeparam>
    internal static class AttributeCache<TObject, TAttribute>
    {
        private static readonly TAttribute[] Attributes;

        static AttributeCache()
        {
            var attObjects = typeof(TObject).GetCustomAttributes(typeof(TAttribute), true);
            if (attObjects.Length == 0)
                throw new ArgumentException("Object " + typeof(TObject) + "Does not contain attribute " +
                                            typeof(TAttribute) + ".");

            Attributes = attObjects as TAttribute[];
        }

        /// <summary>
        /// Gets the first attribute.
        /// </summary>
        /// <returns>TAttribute</returns>
        public static TAttribute GetAttribute()
        {
            return Attributes[0];
        }

        /// <summary>
        /// Retrieves all the attributes.
        /// </summary>
        /// <returns>Array of TAttributes</returns>
        public static TAttribute[] GetAttributes()
        {
            return Attributes;
        }
    }
}