using Soneta.Types;
using System;

namespace ImportCzasuPracy
{
    public static class Extensions
    {
        public static Time ToTime(this TimeSpan time)
        {
            return new Time(time.Hours, time.Minutes);
        }
    }
}
