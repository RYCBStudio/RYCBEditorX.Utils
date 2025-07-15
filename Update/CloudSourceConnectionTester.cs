using System;
using System.Net.Http;
using System.Windows.Threading;

namespace RYCBEditorX.Utils.Update;
public class CloudSourceConnectionTester
{
    private static bool _status = false;
    private static DispatcherTimer _TimeOut;
    private static HttpClient _client;
    public static bool TestConnection()
    {
        _TimeOut = new() { Interval = TimeSpan.FromSeconds(1) };
        _TimeOut.Tick += _TimeOut_Elapsed;
        _TimeOut.Start();
        _client = new HttpClient();
        try
        {
            var _ = _client.GetAsync("http://101.34.85.14");
            if (_.Result.IsSuccessStatusCode)
            {
                GlobalConfig.CurrentLogger.Log("连接成功", module: EnumLogModule.UPDATE);
                _status = true;
            }
        }
        catch (System.Net.WebException ex)
        {
            GlobalConfig.CurrentLogger.Log("连接失败", module: EnumLogModule.UPDATE);
            GlobalConfig.CurrentLogger.Error(ex, module: EnumLogModule.UPDATE);
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
