using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SHARE
namespace yv.shared {
public class Logger {

#region Info
    public static void Info (string format, params object[] paramObjects) { }
    public static void Info (string message) { }
#endregion

#region Debug
    public static void Debug (string format, params object[] paramObjects) { }
    public static void Debug (string message) { }
#endregion

#region Warning
    public static void Warning (string format, params object[] paramObjects) { }
    public static void Warning (string message) { }
    public static void Warning (Exception exception, string message) { }
#endregion

#region Error
    public static void Error (Exception exception, string message) { }
    public static void Error (string format, params object[] paramObjects) { }
    public static void Error (string message) { }
#endregion

}
}

#endif