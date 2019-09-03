using System;

namespace HomesicknessVisualiser.Services
{
    public static class TimeTranslator
    {
        public static long ToJavascriptReadable(this DateTime dt)
        {
            long DateTimeMinTimeTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
            return (dt.ToUniversalTime().Ticks - DateTimeMinTimeTicks) / 10000;
        }

        public static string ToShortDate(this DateTime dt)
        {
            var date = dt.Date.ToShortDateString();
            int dateLength = date.Length;
            var shortDate = date.Substring(0, dateLength - 5).Replace("/", ".");

            return shortDate;
        }
    }
}
