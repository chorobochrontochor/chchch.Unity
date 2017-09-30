using System.Collections;
using System.Collections.Generic;

namespace chchch
{
    public static class Logger
    {
        [System.Diagnostics.ConditionalAttribute("DEBUG")]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static void DebugLogError(params object[] p_args)
        {
            string message = "";
            for (int i = 0; i < p_args.Length; i++)
            {
                message += p_args[i] + " ";
            }

            #if UNITY_EDITOR
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(1);

                UnityEngine.Debug.LogError(stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " -> " + message);
            #else
                UnityEngine.Debug.LogError("Unknown" + "." + "Unknown" + " -> " + _message);
            #endif
        }

        [System.Diagnostics.ConditionalAttribute("DEBUG")]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static void DebugLogInfo(params object[] p_args)
        {
            string message = "";
            for (int i = 0; i < p_args.Length; i++)
            {
                message += p_args[i] + " ";
            }

            #if UNITY_EDITOR
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(1);

                UnityEngine.Debug.Log(stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " -> " + message);
            #else
                UnityEngine.Debug.Log("Unknown" + "." + "Unknown" + " -> " + _message);
            #endif
        }
    }
}