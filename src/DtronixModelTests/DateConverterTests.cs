using System;
using System.Data.SQLite;
using System.IO;
using DtronixModel;
using System.Reflection;
using NUnit.Framework;

namespace DtronixModelTests
{
    public class DateConverterTests
    {

        [Test]
        public void UnixSeconds()
        {
            DateTimeOffset now_offset = new DateTimeOffset(DateTime.UtcNow);
            DateTimeOffset offset_test = Converters.FromUnixTimeSeconds(Converters.ToUnixTimeSeconds(now_offset));

            Assert.AreEqual(now_offset.Year, offset_test.Year);
            Assert.AreEqual(now_offset.Month, offset_test.Month);
            Assert.AreEqual(now_offset.Date, offset_test.Date);
            Assert.AreEqual(now_offset.Hour, offset_test.Hour);
            Assert.AreEqual(now_offset.Minute, offset_test.Minute);
            Assert.AreEqual(now_offset.Second, offset_test.Second);
        }

        [Test]
        public void UnixMilliseconds()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTimeOffset now_offset = new DateTimeOffset(DateTime.UtcNow);
            DateTimeOffset offset_test = Converters.FromUnixTimeMilliseconds(Converters.ToUnixTimeMilliseconds(now_offset));

            Assert.AreEqual(now_offset.Year, offset_test.Year);
            Assert.AreEqual(now_offset.Month, offset_test.Month);
            Assert.AreEqual(now_offset.Date, offset_test.Date);
            Assert.AreEqual(now_offset.Hour, offset_test.Hour);
            Assert.AreEqual(now_offset.Minute, offset_test.Minute);
            Assert.AreEqual(now_offset.Second, offset_test.Second);
            Assert.AreEqual(now_offset.Millisecond, offset_test.Millisecond);
        }
    }
}
