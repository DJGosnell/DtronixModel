using System;

namespace DtronixModel
{
    /// <summary>
    /// Utility class to translate database values to Net Types.
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Converts Unix timestamp (seconds) to DateTimeOffset.
        /// </summary>
        /// <param name="seconds">Unix seconds to convert to a DateTime object</param>
        /// <returns>Converted time in DateTimeOffset.</returns>
        public static DateTimeOffset FromUnixTimeSeconds(long seconds)
        {
            if (seconds < -62135596800L || seconds > 253402300799L)
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "");

            return new DateTimeOffset(seconds * 10000000L + 621355968000000000L, TimeSpan.Zero);
        }

        /// <summary>
        /// Converts Unix timestamp (milliseconds) to DateTimeOffset.
        /// </summary>
        /// <param name="milliseconds">Unix milliseconds to convert to a DateTime object</param>
        /// <returns>Converted time in DateTimeOffset.</returns>
        public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds)
        {
            if (milliseconds < -62135596800000L || milliseconds > 253402300799999L)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "");

            return new DateTimeOffset(milliseconds * 10000L + 621355968000000000L, TimeSpan.Zero);
        }

        /// <summary>
        /// Converts Unix timestamp (seconds) to DateTime.
        /// </summary>
        /// <param name="value">Unix seconds to convert to a DateTime object.</param>
        /// <returns>Converted time in DateTime.</returns>
        public static DateTime ToDateTime(double value)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value).ToLocalTime();
        }

        /// <summary>
        /// Converts a given DateTimeOffset into a Unix timestamp in seconds.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Milliseconds passed since epoch.</returns>
        public static long ToUnixTimeSeconds(DateTimeOffset value)
        {
            return value.Ticks / 10000000L - 62135596800L;
        }

        /// <summary>
        /// Converts a given DateTimeOffset into a Unix timestamp in milliseconds.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Milliseconds passed since epoch.</returns>
        public static long ToUnixTimeMilliseconds(DateTimeOffset value)
        {
            return value.Ticks / 10000L - 62135596800000L;
        }
    }
}