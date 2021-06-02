using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class DebugLog
{
    [Conditional("EnableDebug")]
    public static void Logger(string msg)
    {
        UnityEngine.Debug.Log("<color=yellow>" + msg + "</color>");
    }
}

