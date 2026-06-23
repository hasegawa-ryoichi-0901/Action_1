using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LogUtils {

    public static class Debug {
        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log (object o) { UnityEngine.Debug.Log(o); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning (object o) { UnityEngine.Debug.LogWarning(o); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError (object o) { UnityEngine.Debug.LogError(o); }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogFormat (string format, params object[] args) {
            UnityEngine.Debug.LogFormat(format, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogErrorFormat (string format, params object[] args) {
            UnityEngine.Debug.LogErrorFormat(format, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarningFormat (string format, params object[] args) {
            UnityEngine.Debug.LogWarningFormat(format, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assert (bool b, string s) { UnityEngine.Debug.Assert(b, s); }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Red (object o) {
            UnityEngine.Debug.LogFormat("<color=red>{0}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Red (string format, params object[] o) {
            UnityEngine.Debug.LogFormat($"<color=red>{format}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Green (object o) {
            UnityEngine.Debug.LogFormat("<color=green>{0}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Green (string format, params object[] o) {
            UnityEngine.Debug.LogFormat($"<color=green>{format}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Cyan (object o) {
            UnityEngine.Debug.LogFormat("<color=cyan>{0}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Cyan (string format, params object[] o) {
            UnityEngine.Debug.LogFormat($"<color=cyan>{format}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Yellow (object o) {
            UnityEngine.Debug.LogFormat("<color=yellow>{0}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Yellow (string format, params object[] o) {
            UnityEngine.Debug.LogFormat($"<color=yellow>{format}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Orange (object o) {
            UnityEngine.Debug.LogFormat("<color=orange>{0}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Orange (string format, params object[] o) {
            UnityEngine.Debug.LogFormat($"<color=orange>{format}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Magenta (object o) {
            UnityEngine.Debug.LogFormat("<color=magenta>{0}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Magenta (string format, params object[] o) {
            UnityEngine.Debug.LogFormat($"<color=magenta>{format}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Blue (object o) {
            UnityEngine.Debug.LogFormat("<color=blue>{0}</color>", o);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
         MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Blue (string format, params object[] o) {
            UnityEngine.Debug.LogFormat($"<color=blue>{format}</color>", o);
        }

    }
}

public static class Logger {
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug (string format, params object[] paramObjects) =>
        LogUtils.Debug.LogFormat(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug (string message) => 
        LogUtils.Debug.LogFormat(message);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning (string format, params object[] paramObjects) =>
        LogUtils.Debug.LogWarningFormat(format, paramObjects);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning (string message) => 
        LogUtils.Debug.LogWarning(message);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error (string format, params object[] paramObjects) =>
        LogUtils.Debug.LogErrorFormat(format, paramObjects);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error (string message) => 
        LogUtils.Debug.LogError(message);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error (Exception e) => 
        LogUtils.Debug.LogError(e.Message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error (object message) => 
        LogUtils.Debug.LogError(message);
    
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Red (string format, params object[] paramObjects) =>
        LogUtils.Debug.Red(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Red (string message) => 
        LogUtils.Debug.Red(message);
    
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Green (string format, params object[] paramObjects) =>
        LogUtils.Debug.Green(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Green (string message) => 
        LogUtils.Debug.Green(message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Yellow (string format, params object[] paramObjects) =>
        LogUtils.Debug.Yellow(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Yellow (string message) => 
        LogUtils.Debug.Yellow(message);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Orange (string format, params object[] paramObjects) =>
        LogUtils.Debug.Orange(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Orange (string message) => 
        LogUtils.Debug.Orange(message);
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Cyan (string format, params object[] paramObjects) =>
        LogUtils.Debug.Cyan(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Cyan (string message) => 
        LogUtils.Debug.Cyan(message);
    
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Magenta (string format, params object[] paramObjects) =>
        LogUtils.Debug.Magenta(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Magenta (string message) => 
        LogUtils.Debug.Magenta(message);
    
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Blue (string format, params object[] paramObjects) =>
        LogUtils.Debug.Blue(format, paramObjects);

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"),
     MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Blue (string message) => 
        LogUtils.Debug.Blue(message);
}