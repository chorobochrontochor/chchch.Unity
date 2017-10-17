using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EasingFunctions
{
    public delegate double EasingFunction(double p_p);

    public static double Linear(double p_p)
    {
        return p_p;
    }

    public static double EaseInQuad(double p_p)
    {
        return p_p * p_p;
    }

    public static double EaseOutQuad(double p_p)
    {
        return p_p * (2 - p_p);
    }

	public static double EaseInOutQuad(double p_p)
    {
        return (p_p < 0.5) ? (2 * p_p * p_p) : ((4 - 2 * p_p) * p_p - 1);
    }
}
