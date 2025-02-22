using System.Collections;
using System.Collections.Generic;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using UnityEngine;

[Target("UnityLog")]
public class UnityLog : TargetWithLayout
{
    protected override void Write(LogEventInfo logEvent)
    {
        Name = "UnityLog";
        var msg = Layout.Render(logEvent);
        Debug.Log(msg);
    }
}
