using System;
using Downloader;
using Newtonsoft.Json;

namespace RYCBEditorX.Utils.Update;
internal class UpdateInfo
{
    public string Name
    {
        get; set;
    }
    public  string Version
    {
        get; set;
    }
    public  string Revision
    {
        get; set;
    }
    public  string Description
    {
        get; set;
    }

    public string GetFriendlyVersion() => Version + "." + Revision;

    public static UpdateInfo LoadFromJsonFile(string filePath)
    {
        string json = System.IO.File.ReadAllText(filePath);
        var updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(json);

        return updateInfo;
    }

}
public class UpdateUtils
{
    /// <summary>
    /// 当前更新信息。
    /// </summary>
    internal static UpdateInfo CurrentUpdateInfo
    {
        get; set;
    }

    /// <summary>
    /// 更新包压缩文件路径。
    /// </summary>
    internal static string UpdateArchivePath
    {
        get; set;
    }

    /// <summary>
    /// 下载服务。
    /// </summary>
    public static DownloadService CurrentDownloader
    {
        get; internal set;
    }

    /// <summary>
    /// 是否可以更新。
    /// </summary>
    public static bool UpdateCheckOK
    {
        get; set;
    }

    /// <summary>
    /// 是否可以部署更新。
    /// </summary>
    public static bool CanDeployUpdate
    {
        get; set;
    }

    /// <summary>
    /// 开始更新。
    /// </summary>
    public static async void StartAsync()
    {
        GlobalConfig.CurrentLogger.Log("开始更新", module:EnumLogModule.UPDATE);
        GlobalConfig.CurrentLogger.Log("测试连接", module:EnumLogModule.UPDATE);
        UpdateCheckOK = CloudSourceConnectionTester.TestConnection();
        if (UpdateCheckOK) {
            GlobalConfig.CurrentLogger.Log("下载更新", module: EnumLogModule.UPDATE);
            await UpdateDownloader.DownloadAsync("https://192.168.1.6/home/rex-root/rex/update.json", "update.json");
            CurrentUpdateInfo = UpdateInfo.LoadFromJsonFile(GlobalConfig.StartupPath + "\\Cache\\Update\\update.json");
        }
        if (CanDeployUpdate) {
            await UpdateDownloader.DownloadAsync("https://192.168.1.6/home/rex-root/rex/RYCBEditorX.7z");
        }
    }

#pragma warning disable CA2211
    /// <summary>
    /// 下载开始时的事件。
    /// </summary>
    public static EventHandler<DownloadStartedEventArgs> DownloadStarted;

    /// <summary>
    /// 下载进度改变时的事件。
    /// </summary>
    public static EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

    /// <summary>
    /// 下载完成时的事件。
    /// </summary>
    public static EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DownloadCompleted;


}
