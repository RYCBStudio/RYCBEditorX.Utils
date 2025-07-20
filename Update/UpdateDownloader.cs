using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Downloader;
using static RYCBEditorX.GlobalConfig;
using static RYCBEditorX.Utils.Update.UpdateUtils;

namespace RYCBEditorX.Utils.Update;
internal class UpdateDownloader
{
    private static readonly DownloadConfiguration options = new()
    {
        BufferBlockSize = 8000, // 通常，主机最大支持8000字节，默认值为8000。
        ChunkCount = 16, // 要下载的文件分片数量，默认值为1
        MaxTryAgainOnFailure = 10, // 失败的最大次数
        ParallelDownload = Downloading.ParallelDownload, // 下载文件是否为并行的。默认值为false
        ParallelCount = Downloading.ParallelCount,
        Timeout = 2000, // 每个 stream reader  的超时（毫秒），默认值是1000

        RequestConfiguration = // 定制请求头文件
                {
            Accept = "*/*",
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    CookieContainer = new CookieContainer(), // Add your cookies
                    Headers = [], // Add your custom headers
                    KeepAlive = false,
                    ProtocolVersion = HttpVersion.Version11, // Default value is HTTP 1.1
                    UseDefaultCredentials = false,
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0",
                    //Credentials = GetCredentialCache(url, , PASSWORD),
                }
    };

    public static async Task DownloadAsync(string url)
    {
        CurrentDownloader = new DownloadService(options);
        CurrentDownloader.DownloadStarted += DownloadStarted;
        CurrentDownloader.DownloadFileCompleted += DownloadCompleted;
        CurrentDownloader.DownloadProgressChanged += DownloadProgressChanged;
        var file = StartupPath + $"\\Cache\\Update\\REX_{CurrentUpdateInfo.GetFriendlyVersion()}+y{DateTime.Now.Year}m{DateTime.Now.Month}d{DateTime.Now.Day}.7z";
        UpdateArchivePath = file;
        if (File.Exists(UpdateArchivePath))
        {
            CanDeployUpdate = true; return;
        }
        await CurrentDownloader.DownloadFileTaskAsync(url, file);
    }

    public static async Task DownloadAsync(string url, string filename)
    {
        CurrentDownloader = new DownloadService(options);
        CurrentDownloader.DownloadStarted += DownloadStarted;
        CurrentDownloader.DownloadFileCompleted += DownloadCompleted;
        CurrentDownloader.DownloadProgressChanged += DownloadProgressChanged;
        var file = StartupPath + $"\\Cache\\Update\\{filename}";
        await CurrentDownloader.DownloadFileTaskAsync(url, file);
    }

    /// <summary>
    /// 获取凭证
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private static CredentialCache GetCredentialCache(string uri, string username, string password)
    {
        //var authorization = string.Format("{0}:{1}", username, password);
        var credCache = new CredentialCache
            {
                { new Uri(uri), "Basic", new NetworkCredential(username, password) }
            };
        return credCache;
    }
}
