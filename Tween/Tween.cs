using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Tween
{
    public delegate void Callback(double p_position);

    public interface ITween
    {
        ITween OnStart(Callback p_onStart);

        ITween OnStep(Callback p_onStep);

        ITween OnFinish(Callback p_onFinish);

        ITween Start();

        ITween Restart();

        ITween Pause();

        ITween Resume();

        ITween Finish();
    }
}