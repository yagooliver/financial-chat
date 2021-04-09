using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financial.Chat.Infra.MessageService
{
    public enum TimeType
    {
        Milliseconds,
        Seconds,
        Minutes,
        Hours,
        Days
    }
    public static class TimeSpanExtensions
    {
        private readonly static IReadOnlyList<TimeConversor> _times = new List<TimeConversor>
        {
            new TimeConversor(TimeType.Milliseconds, "ms", new Func<int, TimeSpan>((time) => TimeSpan.FromMilliseconds(time))),
            new TimeConversor(TimeType.Seconds, "s", new Func<int, TimeSpan>((time) => TimeSpan.FromSeconds(time))),
            new TimeConversor(TimeType.Minutes, "m", new Func<int, TimeSpan>((time) => TimeSpan.FromMinutes(time))),
            new TimeConversor(TimeType.Hours, "h", new Func<int, TimeSpan>((time) => TimeSpan.FromHours(time))),
            new TimeConversor(TimeType.Days, "d", new Func<int, TimeSpan>((time) => TimeSpan.FromDays(time)))
        };

        /// <summary>
        /// Converts all time range like "1, 5, 10" for TimeSpan with chosen TimeType (Seconds, Minutes, Hours etc..)
        /// </summary>
        /// <param name="intervals">Time range in string</param>
        /// <param name="timeType"></param>
        /// <returns></returns>
        public static TimeSpan[] GetIntervalsFromString(string intervals, TimeType timeType)
        {
            if (string.IsNullOrWhiteSpace(intervals)) throw new ArgumentNullException(nameof(intervals));

            return intervals.Split(',')
                            .Select(interval => GetTimeSpanFromTimeType(interval, timeType))
                            .ToArray();
        }

        /// <summary>
        /// Converts a time range like "1m, 5h, 10d" for TimeSpan. Use number + prefix that matches a time type. (ms = Milliseconds, m = Minutes, h = Hours, d = Days)
        /// </summary>
        /// <param name="intervals">Time range in string</param>
        /// <returns></returns>
        public static TimeSpan[] GetIntervalsFromString(string intervals)
        {
            if (string.IsNullOrWhiteSpace(intervals)) throw new ArgumentNullException(nameof(intervals));

            return intervals.Split(',')
                            .Select(interval => GetTimeSpanFromInterval(interval))
                            .ToArray();
        }

        private static TimeSpan GetTimeSpanFromTimeType(string time, TimeType timeType)
        {
            if (!int.TryParse(time, out int _time))
                throw new InvalidCastException($"Cannot convert {time} to int32");

            var timeConversor = _times.FirstOrDefault(t => t.TimeType == timeType);

            if (timeConversor == null)
                throw new NotImplementedException($"TimeType {timeType} not implemented");

            return timeConversor.Conversor(_time);
        }

        private static TimeSpan GetTimeSpanFromInterval(string interval)
        {
            var timeTypePrefix = interval.ExtractOnlyLetters();

            var timeConversor = _times.FirstOrDefault(t => t.Prefix == timeTypePrefix);

            if (timeConversor == null)
                throw new NotImplementedException($"TimeTypePrefix {timeTypePrefix} not implemented");

            if (!int.TryParse(interval.ExtractOnlyNumbers(), out int _time))
                throw new InvalidCastException($"Cannot convert {interval.ExtractOnlyNumbers()} to int32");

            return timeConversor.Conversor(_time);
        }

        /// <summary>
        /// Extract only number from any value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>If the value is ABC123 will return only 123</returns>
        public static string ExtractOnlyNumbers(this string value)
        {
            var stack = new Stack<char>();

            for (var i = value.Length - 1; i >= 0; i--)
            {
                if (!char.IsNumber(value[i]))
                    continue;

                stack.Push(value[i]);
            }

            return new string(stack.ToArray());
        }

        /// <summary>
        /// Extract only letters from any value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>If the value is ABC123 will return only ABC</returns>
        public static string ExtractOnlyLetters(this string value)
        {
            var stack = new Stack<char>();

            for (var i = value.Length - 1; i >= 0; i--)
            {
                if (!char.IsLetter(value[i]))
                    continue;

                stack.Push(value[i]);
            }

            return new string(stack.ToArray());
        }

        private class TimeConversor
        {
            public TimeType TimeType { get; set; }

            public string Prefix { get; set; }

            public Func<int, TimeSpan> Conversor { get; set; }

            public TimeConversor(TimeType timeType, string prefix, Func<int, TimeSpan> conversor)
            {
                TimeType = timeType;
                Prefix = prefix;
                Conversor = conversor;
            }
        }
    }
}
