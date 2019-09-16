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
            var dateLength = date.Length;
            var monthAndDay = date.Substring(0, dateLength - 5).Replace("/", ".");
            var year = date.Substring(dateLength - 4);

            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            int month = Convert.ToInt32(monthAndDay.Substring(0, 1)) - 1;

            string dateWithMonthInLetters = year + "\n" + months[month] + monthAndDay.Substring(1);

            return dateWithMonthInLetters;
        }
    }
}
