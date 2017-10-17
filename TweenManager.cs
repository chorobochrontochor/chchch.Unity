using chchch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch
{
    public delegate void TweenCallback(double p_position);

    public interface ITween
    {
        ITween OnStart(TweenCallback p_onStart);

        ITween OnStep(TweenCallback p_onStep);

        ITween OnFinish(TweenCallback p_onFinish);

        ITween Start();

        ITween Restart();

        ITween Pause();

        ITween Resume();

        ITween Finish();
    }
    
    public static class TweenManager
    {
        private class Tween : ITween
        {
            private _updateBehaviour _updateBehaviour;
            private EasingFunctions.EasingFunction _easingFunction;
            private ITimeStamp _timeStamp;
            private double _duration;
            private bool _isActive;
            private bool _lastStep;

            private TweenCallback _onStart;
            private TweenCallback _onStep;
            private TweenCallback _onFinish;

            private bool isFinished
            {
                get
                {
                    return _timeStamp.ScaledTimePassed >= _duration;
                }
            }

            private double position
            {
                get
                {
                    return _easingFunction(_timeStamp.ScaledTimePassed / _duration);
                }
            }

            public Tween(_updateBehaviour p_updateBehaviour, EasingFunctions.EasingFunction p_easingFunction, double p_duration)
            {
                _updateBehaviour = p_updateBehaviour;
                _timeStamp = TimeManager.CreateTimeStamp();
                _easingFunction = p_easingFunction;

                _duration = p_duration == 0 ? 0.00000001 : p_duration;
                _lastStep = false;
                _isActive = false;
            }

            public void Step()
            {
                if (_lastStep)
                {
                    Finish();
                }
                else
                {
                    if (isFinished)
                    {
                        _lastStep = true;
                    }

                    if (_onStep != null)
                    {
                        _onStep(position);
                    }
                }
            }

            public ITween OnStart(TweenCallback p_onStart)
            {
                _onStart = p_onStart;

                return this;
            }

            public ITween OnStep(TweenCallback p_onStep)
            {
                _onStep = p_onStep;

                return this;
            }

            public ITween OnFinish(TweenCallback p_onFinish)
            {
                _onFinish = p_onFinish;

                return this;
            }

            public ITween Start()
            {
                if (!_isActive)
                {
                    Restart();
                }

                return this;
            }

            public ITween Restart()
            {
                _timeStamp = TimeManager.CreateTimeStamp();

                activate();
                
                if (_onStart != null)
                {
                    _onStart(0.0);
                }

                return this;
            }

            public ITween Pause()
            {
                deactivate();

                return this;
            }

            public ITween Resume()
            {
                activate();

                return this;
            }

            public ITween Finish()
            {
                Pause();

                if (_onStep != null)
                {
                    _onStep(1.0);
                }

                if (_onFinish != null)
                {
                    _onFinish(1.0);
                }

                return this;
            }

            private void activate()
            {
                if (!_isActive)
                {
                    _updateBehaviour.Add(this);
                    _isActive = true;
                    _lastStep = false;
                }
            }

            private void deactivate()
            {
                if (_isActive)
                {
                    _updateBehaviour.Remove(this);
                    _isActive = false;
                    _lastStep = false;
                }
            }
        }
        
        private class _updateBehaviour : MonoBehaviour
        {
            private List<Tween> _tweens = new List<Tween>();

            public void Update()
            {
                for (int i = 0; i < _tweens.Count; i++)
                {
                    _tweens[i].Step();
                }
            }

            public void Add(Tween p_tweenHandle)
            {
                _tweens.Add(p_tweenHandle);
            }

            public void Remove(Tween p_tweenHandle)
            {
                _tweens.Remove(p_tweenHandle);
            }
        };

        private static _updateBehaviour _monoBehaviour;

        private static _updateBehaviour updateBehaviour
        {
            get
            {
                if (_monoBehaviour == null)
                {
                    GameObject gameObject = new GameObject(typeof(TweenManager).Name);
                    gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave;
                    Object.DontDestroyOnLoad(gameObject);
                    _monoBehaviour = gameObject.AddComponent<_updateBehaviour>();
                }
                
                return _monoBehaviour;
            }
        }
        
        public static ITween CreateTween(EasingFunctions.EasingFunction p_easingFunction, double p_duration)
        {
            return new Tween(updateBehaviour, p_easingFunction, p_duration);
        }
    }
}