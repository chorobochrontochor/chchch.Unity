using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Time
{
    public interface Ch3ITimeStamp
    {
        float ScaledTimePassed { get; }

        float ScaledFixedTimePassed { get; }

        float RealTimePassed { get; }
    }
}