using chchch.Easing;
using chchch.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Time
{
    public static class Ch3TimeManager
    {
        private class TimeStamp : Ch3ITimeStamp
        {
            private float _scaledTime;
            private float _scaledFixedTime;
            private float _realTime;

            public TimeStamp()
            {
                _scaledTime = UnityEngine.Time.time;
                _scaledFixedTime = UnityEngine.Time.fixedTime;
                _realTime = UnityEngine.Time.unscaledTime;
            }

            public float ScaledTimePassed
            {
                get
                {
                    return UnityEngine.Time.time - _scaledTime;
                }
            }

            public float ScaledFixedTimePassed
            {
                get
                {
                    return UnityEngine.Time.fixedTime - _scaledFixedTime;
                }
            }

            public float RealTimePassed
            {
                get
                {
                    return UnityEngine.Time.unscaledTime - _realTime;
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
                return UnityEngine.Time.timeScale;
            }
            set
            {
                stopTimeScaleTween();

                UnityEngine.Time.timeScale = value;
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

        public static void TweenTimeScale(Ch3EasingFunctionDelegate p_easingFunction, double p_from, double p_to, double p_duration)
        {
            stopTimeScaleTween();

            UnityEngine.Time.timeScale = (float) p_from;

            _timeScaleTween = Ch3TweenManager.CreateTween(p_easingFunction, p_duration).OnStep((double p_value) => {

                UnityEngine.Time.timeScale = (float) (p_from + (p_to - p_from) * p_value);
            }).OnFinish((double p_value) => {

                UnityEngine.Time.timeScale = (float) p_to;
            }).Start();
        }

        public static Ch3ITimeStamp CreateTimeStamp()
        {
            return new TimeStamp();
        }

        public static float ScaledTimePassed
        {
            get
            {
                return UnityEngine.Time.time;
            }
        }

        public static float FixedScaledTimePassed
        {
            get
            {
                return UnityEngine.Time.fixedTime;
            }
        }

        public static float RealTimePassed
        {
            get
            {
                return UnityEngine.Time.unscaledTime;
            }
        }

        public static object TweenManager { get; private set; }
    }
}