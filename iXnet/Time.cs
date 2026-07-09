using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    public static class Time
    {
        private static DateTime UnixZeroDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static UInt64 GetUnixTimeStamp(DateTime time)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            DateTime utcTime = time.ToUniversalTime();
            TimeSpan span = (utcTime - UnixZeroDate);
            return (UInt64)span.TotalSeconds;
        }

        public static DateTime GetFromUnixTimeStamp(UInt64 timeStamp)
        {
            return (UnixZeroDate + new TimeSpan((long)(timeStamp * TimeSpan.TicksPerSecond))).ToLocalTime();
        }
    }
}
