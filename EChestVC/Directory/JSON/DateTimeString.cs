using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EChestVC.Directory.JSON
{
    static class DateTimeString
    {
        public static string GetString(DateTime dateTime)
        {
            return dateTime.ToString("O");
        }

        public static DateTime FromString(string dateTime)
        {
            var provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(dateTime, "O", provider);
        }
    }
}
