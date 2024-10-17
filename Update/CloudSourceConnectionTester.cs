using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RYCBEditorX.Utils.Update;
internal class CloudSourceConnectionTester
{
    private static bool _status;
    private static DispatcherTimer _TimeOut;
    private static HttpClient _client;
    public static async Task<bool> TestConnection()
    {
        _TimeOut = new() { Interval = TimeSpan.FromSeconds(2) };
        _TimeOut.Tick += _TimeOut_Elapsed;
        _TimeOut.Start();
        try
        {
            GlobalConfig.CurrentLogger.Log("正在测试连接", module: EnumLogModule.UPDATE);
            _client = new HttpClient() { Timeout = TimeSpan.FromSeconds(1) };
            GlobalConfig.CurrentLogger.Log("准备GET", module: EnumLogModule.UPDATE);
            var response = await _client.GetAsync("https://192.168.1.6/");
            GlobalConfig.CurrentLogger.Log("GET完成", module: EnumLogModule.UPDATE);
            if (response.IsSuccessStatusCode)
            {
                GlobalConfig.CurrentLogger.Log("GET结果正常", module: EnumLogModule.UPDATE);
                _status = true;
            }
            else
            {
                GlobalConfig.CurrentLogger.Log("GET有误", module: EnumLogModule.UPDATE);
                _status = false;
            }
        }
        catch (Exception)
        {
            _status = false;
        }
        finally
        {
            _TimeOut?.Stop();
            _client?.Dispose();
        }
        return _status;
    }

    private static void _TimeOut_Elapsed(object sender, EventArgs e)
    {

        GlobalConfig.CurrentLogger.Log("连接超时", module: EnumLogModule.UPDATE);
        _TimeOut?.Stop();
        _client?.Dispose();
        _status = false;
    }
}
