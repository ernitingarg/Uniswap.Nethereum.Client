using System;

namespace Uniswap.Client
{
    public static class TimeExtension
    {
        private static readonly DateTime OriginDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static uint ToTimespamp(this DateTime date)
        {
            return (uint)(date - OriginDateTime).TotalSeconds;
        }

        public static DateTime FromTimestamp(this int timestamp)
        {
            return OriginDateTime.AddSeconds(timestamp).ToUniversalTime();
        }
    }
}
