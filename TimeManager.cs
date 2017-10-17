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

        private static ITween _timeScaleTween;

        public static float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
            set
            {
                stopTimeScaleTween();

                Time.timeScale = value;
            }
        }

        private static void stopTimeScaleTween()
        {
            if (_timeScaleTween != null)
            {
                _timeScaleTween.Pause();
                _timeScaleTween = null;
            }
        }

        public static void TweenTimeScale(EasingFunctions.EasingFunction p_easingFunction, double p_from, double p_to, double p_duration)
        {
            stopTimeScaleTween();

            Time.timeScale = (float) p_from;

            _timeScaleTween = TweenManager.CreateTween(p_easingFunction, p_duration).OnStep((double p_value) => {

                Time.timeScale = (float) (p_from + (p_to - p_from) * p_value);
            }).OnFinish((double p_value) => {

                Time.timeScale = (float) p_to;
            }).Start();
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