using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtxModel {
	public class Converters {
		/// <summary>
		/// Converts a given DateTime into a Unix timestamp
		/// </summary>
		/// <param name="value">Any DateTime</param>
		/// <returns>The given DateTime in Unix timestamp format</returns>
		public static long DateTimeToUnix(DateTime value) {
			return (long)Math.Truncate((value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
		}

		/// <summary>
		/// Converts a Unix long into a DateTime object.
		/// </summary>
		/// <param name="value">Any Unix long.</param>
		/// <returns>The given long in a DateTime object.</returns>
		public static DateTime UnixToDateTime(long value) {
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			return epoch.AddSeconds((double)value).ToLocalTime();
		}

	}
}
