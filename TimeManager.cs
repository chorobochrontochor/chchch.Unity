using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch
{
    public interface ITimeStamp
    {
        float ScaledTimePassed { get; }

        float ScaledFixedTimePassed { get; }

        float RealTimePassed { get; }
    }

    public static class TimeManager
    {
        private class TimeStamp : ITimeStamp
        {
            private float _scaledTime;
            private float _scaledFixedTime;
            private float _realTime;

            public TimeStamp()
            {
                _scaledTime = Time.time;
                _scaledFixedTime = Time.fixedTime;
                _realTime = Time.unscaledTime;
            }

            public float ScaledTimePassed
            {
                get
                {
                    return Time.time - _scaledTime;
                }
            }

            public float ScaledFixedTimePassed
            {
                get
                {
                    return Time.fixedTime - _scaledFixedTime;
                }
            }

            public float RealTimePassed
            {
                get
                {
                    return Time.unscaledTime - _realTime;
                }
            }
        }

        public const float NORMAL_TIME = 1f;
        public const float FREEZE_TIME = 0f;

        public static float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
            set
            {
                Time.timeScale = value;
            }
        }

        public static ITimeStamp CreateTimeStamp()
        {
            return new TimeStamp();
        }

        public static float ScaledTimePassed
        {
            get
            {
                return Time.time;
            }
        }

        public static float FixedScaledTimePassed
        {
            get
            {
                return Time.fixedTime;
            }
        }

        public static float RealTimePassed
        {
            get
            {
                return Time.unscaledTime;
            }
        }
    }
}