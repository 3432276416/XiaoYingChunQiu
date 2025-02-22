using System;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Unity3D;

public static class UnityLog
{
    public static Logger logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Unity3D()
                    .WriteTo.File($"Logs\\log.txt")
                    .CreateLogger();
    public static void Init()
    {
        logger.Information("日志初始化成功");
    }
}
