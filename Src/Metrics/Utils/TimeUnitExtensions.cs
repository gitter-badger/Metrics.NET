﻿using System;

namespace Metrics.Utils
{
    public static class TimeUnitExtensions
    {
        private static readonly long[,] conversionFactors = BuildConversionFactorsMatrix();

        private static long[,] BuildConversionFactorsMatrix()
        {
            var count = Enum.GetValues(typeof(TimeUnit)).Length;

            var matrix = new long[count, count];
            var timingFactors = new[] 
            {
                1000L,  // Nanoseconds to microseconds
                1000L,  // Microseconds to milliseconds
                1000L,  // Milliseconds to seconds
                60L,    // Seconds to minutes
                60L,    // Minutes to hours
                24L     // Hours to days
            };

            for (var source = 0; source < count; source++)
            {
                var cumulativeFactor = 1L;
                for (var target = source - 1; target >= 0; target--)
                {
                    cumulativeFactor *= timingFactors[target];
                    matrix[source, target] = cumulativeFactor;
                }
            }
            return matrix;
        }

        public static long Convert(this TimeUnit sourceUnit, TimeUnit targetUnit, long value)
        {
            if (sourceUnit == targetUnit)
            {
                return value;
            }

            var sourceIndex = (int)sourceUnit;
            var targetIndex = (int)targetUnit;

            var result = (sourceIndex > targetIndex) ?
                value * conversionFactors[sourceIndex, targetIndex] :
                value / conversionFactors[targetIndex, sourceIndex];

            return result;
        }

        public static long ToNanoseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Nanoseconds, value);
        }

        public static long ToMicroseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Microseconds, value);
        }

        public static long ToMilliseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Milliseconds, value);
        }

        public static long ToSeconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Seconds, value);
        }

        public static long ToMinutes(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Minutes, value);
        }

        public static long ToHours(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Hours, value);
        }

        public static long ToDays(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Days, value);
        }

        public static string Unit(this TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Nanoseconds: return "ns";
                case TimeUnit.Microseconds: return "us";
                case TimeUnit.Milliseconds: return "ms";
                case TimeUnit.Seconds: return "s";
                case TimeUnit.Minutes: return "min";
                case TimeUnit.Hours: return "h";
                case TimeUnit.Days: return "day";
                default:
                    throw new ArgumentOutOfRangeException("unit");
            }
        }
    }
}
