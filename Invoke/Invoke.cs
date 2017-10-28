using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Invoke
{
    public interface Ch3IInvoke
    {
        void Schedule(float p_delay, bool p_repeat = false);

        void Debounce(float p_delay, bool p_repeat = false);

        void ExecuteImmediate();

        void ExecuteImmediateScheduled();

        void Cancel();

        void Reset();

        bool IsScheduled { get; }

        bool IsRepeating { get; }

        float Delay { get; }

        float ExecutesIn { get; }
    }
}