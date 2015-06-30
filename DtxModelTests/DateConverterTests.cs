using System;
using System.Data.SQLite;
using System.IO;
using DtxModel;
using System.Reflection;
using Xunit;

namespace DtxModelTests {
	public class DateConverterTests {

		[Fact]
		public void UnixSeconds() {
			DateTimeOffset now_offset = new DateTimeOffset(DateTime.UtcNow);
			DateTimeOffset offset_test = Converters.FromUnixTimeSeconds(Converters.ToUnixTimeSeconds(now_offset));

			Assert.Equal(now_offset.Year, offset_test.Year);
			Assert.Equal(now_offset.Month, offset_test.Month);
			Assert.Equal(now_offset.Date, offset_test.Date);
			Assert.Equal(now_offset.Hour, offset_test.Hour);
			Assert.Equal(now_offset.Minute, offset_test.Minute);
			Assert.Equal(now_offset.Second, offset_test.Second);
		}

		[Fact]
		public void UnixMilliseconds() {
			DateTime utcNow = DateTime.UtcNow;
			DateTimeOffset now_offset = new DateTimeOffset(DateTime.UtcNow);
			DateTimeOffset offset_test = Converters.FromUnixTimeMilliseconds(Converters.ToUnixTimeMilliseconds(now_offset));

			Assert.Equal(now_offset.Year, offset_test.Year);
			Assert.Equal(now_offset.Month, offset_test.Month);
			Assert.Equal(now_offset.Date, offset_test.Date);
			Assert.Equal(now_offset.Hour, offset_test.Hour);
			Assert.Equal(now_offset.Minute, offset_test.Minute);
			Assert.Equal(now_offset.Second, offset_test.Second);
			Assert.Equal(now_offset.Millisecond, offset_test.Millisecond);
		}
	}
}
