using chchch.Time;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Invoke
{
    public static class Ch3InvokeManager
    {
        private static float MIN_DELAY = 0.0000001f;

        private class Ch3Invoke : Ch3IInvoke
        {
            private _fixedUpdateBehaviour _fixedUpdateBehaviour;
            private System.Action _action;
            private Ch3ITimeStamp _scheduleTimeStamp;
            private bool _isScheduled;
            private bool _isRepeating;
            private float _delay;
            private Ch3TimeType _timeType;

            public Ch3Invoke(_fixedUpdateBehaviour p_fixedUpdateBehaviour, System.Action p_action)
            {
                _fixedUpdateBehaviour = p_fixedUpdateBehaviour;
                _action = p_action;
                _scheduleTimeStamp = Ch3TimeManager.CreateTimeStamp();
                _isScheduled = false;
                _isRepeating = false;
                _delay = MIN_DELAY;
                _timeType = Ch3TimeType.SCALED_FIXED;
            }

            private void cancel()
            {
                if (IsScheduled)
                {
                    _fixedUpdateBehaviour.Remove(this);
                }

                _isScheduled = false;
                _delay = MIN_DELAY;
            }

            private void schedule()
            {
                if (!IsScheduled)
                {
                    _fixedUpdateBehaviour.Add(this);

                    _isScheduled = true;
                }
            }

            public void Cancel()
            {
                cancel();
            }

            public void Reset()
            {
                if (IsScheduled)
                {
                    _scheduleTimeStamp = Ch3TimeManager.CreateTimeStamp();
                    schedule();
                }
            }

            public void Schedule(float p_delay, Ch3TimeType p_timeType = Ch3TimeType.SCALED_FIXED, bool p_repeat = false)
            {
                if (!IsScheduled)
                {
                    _scheduleTimeStamp = Ch3TimeManager.CreateTimeStamp();
                }

                _isRepeating = p_repeat;
                _delay = (p_delay < MIN_DELAY) ? MIN_DELAY : p_delay;
                _timeType = p_timeType;

                schedule();
            }

            public void Debounce(float p_delay, Ch3TimeType p_timeType = Ch3TimeType.SCALED_FIXED, bool p_repeat = false)
            {
                _scheduleTimeStamp = Ch3TimeManager.CreateTimeStamp();
                _isRepeating = p_repeat;
                _delay = (p_delay < MIN_DELAY) ? MIN_DELAY : p_delay;
                _timeType = p_timeType;

                schedule();
            }

            public bool IsScheduled
            {
                get
                {
                    return _isScheduled;
                }
            }

            public bool IsRepeating
            {
                get
                {
                    return _isRepeating;
                }
            }

            public float Delay
            {
                get
                {
                    return _delay;
                }
            }

            public float ExecutesIn
            {
                get
                {
                    return ShouldExecute ? 0 : (_delay - _scheduleTimeStamp.GetTimePassed(TimeType));
                }
            }

            public void ExecuteImmediate()
            {
                _action();
            }

            public void ExecuteImmediateScheduled()
            {
                if (IsScheduled)
                {
                    _action();

                    if (!IsRepeating)
                    {
                        cancel();
                    }
                    else
                    {
                        _scheduleTimeStamp = Ch3TimeManager.CreateTimeStamp();
                    }
                }
            }

            public void ExecuteInternally()
            {
                _action();

                if (!IsRepeating)
                {
                    cancel();
                }
                else
                {
                    _scheduleTimeStamp = Ch3TimeManager.CreateTimeStamp();
                }
            }

            public bool ShouldExecute
            {
                get
                {
                    return _scheduleTimeStamp.GetTimePassed(TimeType) >= _delay;
                }
            }

            public Ch3TimeType TimeType
            {
                get
                {
                    return _timeType;
                }
            }
        }
        
        public static Ch3IInvoke CreateInvoker(System.Action p_action)
        {
            return new Ch3Invoke(fixedUpdateBehaviour, p_action);
        }

        private class _fixedUpdateBehaviour : MonoBehaviour
        {
            private List<Ch3Invoke> _executables = new List<Ch3Invoke>();

            public void Update()
            {
                for (int i = _executables.Count - 1; i >= 0; i--)
                {
                    if (_executables[i].ShouldExecute)
                    {
                        _executables[i].ExecuteInternally();
                    }
                }
            }

            public void Add(Ch3Invoke p_executable)
            {
                _executables.Add(p_executable);
            }

            public void Remove(Ch3Invoke p_executable)
            {
                _executables.Remove(p_executable);
            }
        };

        private static _fixedUpdateBehaviour _monoBehaviour;

        private static _fixedUpdateBehaviour fixedUpdateBehaviour
        {
            get
            {
                if (_monoBehaviour == null)
                {
                    GameObject gameObject = new GameObject(typeof(Ch3Invoke).Name);
                    gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave;
                    Object.DontDestroyOnLoad(gameObject);
                    _monoBehaviour = gameObject.AddComponent<_fixedUpdateBehaviour>();
                }

                return _monoBehaviour;
            }
        }
    }
}
