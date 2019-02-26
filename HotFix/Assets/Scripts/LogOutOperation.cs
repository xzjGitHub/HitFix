using System;
using System.IO;
using System.Text;
using UnityEngine;


//日志级别
public enum LogLevel
{
    None = 0,
    Debug = 1,
    Error = 2,
    Warning = 4,
    Exception = 8,
    All = LogLevel.Debug | LogLevel.Error | LogLevel.Warning | LogLevel.Exception
}

//日志写入
public class LogWriter
{
    public string temp = "";
    private string m_logPath = Application.persistentDataPath + "/log/";
    private string m_logFileName = "_log.txt"; /*"log_{0}.txt";*/
    private string m_logFilePath = string.Empty;

    //构造路径
    public LogWriter()
    {
        if (!Directory.Exists(m_logPath))
        {
            Directory.CreateDirectory(m_logPath);
            temp = m_logPath;
        }
        else
        {
            //  DirectoryInfo log = new DirectoryInfo(m_logPath);
            // log.Delete(true);
            Directory.CreateDirectory(m_logPath);
            temp = m_logPath;
        }
        this.m_logFilePath = this.m_logPath + string.Format(DateTime.Today.ToString("yyyyMMdd")) + this.m_logFileName;
    }
    //执行写入
    public void ExcuteWrite(string content)
    {
        using (StreamWriter writer = new StreamWriter(m_logFilePath, true, Encoding.UTF8))
        {
            writer.WriteLine(content);
        }
    }
}

//日志帮助
public class LogHelperLSK
{
    static public LogLevel m_logLevel = LogLevel.All;
    static LogWriter m_logWriter = new LogWriter();
    public static string temp;

    //接收日志
    static LogHelperLSK()
    {
        temp = m_logWriter.temp;
        Application.logMessageReceived += ProcessExceptionReport;
    }

    //异常进入接口
    private static void ProcessExceptionReport(string message, string stackTrace, LogType type)
    {
        LogLevel dEBUG = LogLevel.Debug;
        switch (type)
        {
            case LogType.Error:
                dEBUG = LogLevel.Error;
                break;
            case LogType.Assert:
                dEBUG = LogLevel.Debug;
                break;
            case LogType.Warning:
                dEBUG = LogLevel.Warning;
                break;
            case LogType.Log:
                dEBUG = LogLevel.Debug;
                break;
            case LogType.Exception:
                dEBUG = LogLevel.Exception;
                break;
        }

        if (dEBUG == (m_logLevel & dEBUG))
        {
            Log(string.Concat(new object[] { " [", dEBUG, "]: ", message, '\n', stackTrace }));
        }
    }

    // 加上时间戳
    private static void Log(string message)
    {
        string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + message;
        m_logWriter.ExcuteWrite(msg);
    }
    //打印日志
    static public void Log(object message)
    {
        Log(message, null);
    }
    static public void Log(object message, UnityEngine.Object context)
    {
        if (LogLevel.Debug == (m_logLevel & LogLevel.Debug))
        {
            Debug.Log(message, context);
        }
    }
    //打印错误
    static public void LogError(object message)
    {
        LogError(message, null);
    }
    static public void LogError(object message, UnityEngine.Object context)
    {
        if (LogLevel.Error == (m_logLevel & LogLevel.Error))
        {
            Debug.LogError(message, context);
        }
    }
    //打印警告
    static public void LogWarning(object message)
    {
        LogWarning(message, null);
    }
    static public void LogWarning(object message, UnityEngine.Object context)
    {
        if (LogLevel.Warning == (m_logLevel & LogLevel.Warning))
        {
            Debug.LogWarning(message, context);
        }
    }

}
