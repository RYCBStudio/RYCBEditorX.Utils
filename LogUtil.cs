using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Globalization;

namespace RYCBEditorX.Utils;
public class LogUtil
{

    private readonly StreamWriter _logWriter;
    private readonly ManagementClass _managementClass;

    private readonly Dictionary<Enum, string> _translation = new()
    {
        {EnumLogModule.MAIN, "主程序" },
        {EnumLogModule.UPDATE, "更新" },
        {EnumLogModule.NET, "网络" },
        {EnumLogModule.SQL, "SQL" },
        {EnumLogPort.CLIENT, "客户端" },
        {EnumLogPort.SERVER, "服务端" },
        {EnumLogType.INFO, "信息" },
        {EnumLogType.WARN, "警告" },
        {EnumLogType.ERROR, "错误" },
        {EnumLogType.FATAL, "致命错误" },
        {EnumLogType.DEBUG, "调试" },
    };
    public string LogPath
    {
        get; set;
    }

    public LogUtil(string logPath)
    {
        LogPath = logPath;
        _logWriter = new StreamWriter(logPath);
        Log("=============系统信息=============", module: EnumLogModule.CUSTOM, customModuleName: "初始化");
        _managementClass = new ManagementClass("Win32_OperatingSystem");
        var managCollection = _managementClass.GetInstances();
        foreach (var manag in managCollection)
        {
            Log("系统: " + manag["Name"].ToString().Split('|')[0], module: EnumLogModule.CUSTOM, customModuleName: "初始化");
            break;
        }
        _managementClass = new ManagementClass("Win32_Processor");
        managCollection = _managementClass.GetInstances();
        foreach (var manag in managCollection)
        {
            Log("CPU: " + manag["Name"], module: EnumLogModule.CUSTOM, customModuleName: "初始化");
            break;
        }
        Log("语言: " + CultureInfo.CurrentCulture.DisplayName, module: EnumLogModule.CUSTOM, customModuleName: "初始化");
    }

    public void Log(object message, EnumLogType type = EnumLogType.INFO, EnumLogPort port = EnumLogPort.CLIENT, EnumLogModule module = EnumLogModule.MAIN, string customModuleName = "")
    {
        _logWriter.WriteLine("[{0}][{1}|{2}:{3}] {4}",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            _translation[type],
            _translation[port],
            module == EnumLogModule.CUSTOM ? customModuleName : _translation[module],
            message
            );
        _logWriter.Flush();
    }

    public void LogDebug(object message, EnumLogPort port = EnumLogPort.CLIENT, EnumLogModule module = EnumLogModule.MAIN, string customModuleName = "")
    {
        _logWriter.WriteLine("[{0}][{1}|{2}:{3}] {4}",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFFFFF"),
            _translation[EnumLogType.DEBUG],
            _translation[port],
            module == EnumLogModule.CUSTOM ? customModuleName : _translation[module],
            message
            );
        _logWriter.Flush();
    }

    public void Error(Exception ex, string message = "", EnumLogType type = EnumLogType.ERROR, EnumLogPort port = EnumLogPort.CLIENT, EnumLogModule module = EnumLogModule.MAIN, string customModuleName = "")
    {
        _logWriter.WriteLine("[{0}][{1}|{2}:{3}] 发生错误：[{4}] {5} {6}",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            _translation[type],
            _translation[port],
            module == EnumLogModule.CUSTOM ? customModuleName : _translation[module],
            ex.GetType(),
            ex.Message,
            message
            );
        _logWriter.WriteLine("[{0}][{1}|{2}:{3}] \nError Infomation \nType [{4}]\nMessage [{5}]\nStacktrace \n{6}",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            _translation[type],
            _translation[port],
            module == EnumLogModule.CUSTOM ? customModuleName : _translation[module],
            ex.GetType(),
            ex.Message,
            ex.StackTrace
            );
        _logWriter.Flush();
    }
}

public enum EnumLogPort
{
    CLIENT,
    SERVER,
}
public enum EnumLogModule
{
    MAIN,
    UPDATE,
    NET,
    SQL,
    CUSTOM
}
public enum EnumLogType
{
    INFO,
    WARN,
    ERROR,
    FATAL,
    DEBUG
}