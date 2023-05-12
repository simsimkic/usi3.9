using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime StringToDateTime(string date)
        {
            return DateTime.ParseExact(date, "dd.MM.yyyy. HH:mm", null);
        }

        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy. HH:mm");
        }
    }
}
