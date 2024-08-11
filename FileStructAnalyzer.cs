using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYCBEditorX.Utils;
public class FileStructAnalyzer
{

    private string JSONPath, JSONContent;
    public string FilePath
    {
        get; set;
    }

    public FileStructAnalyzer(string filePath)
    {
        FilePath = filePath;
        var ext = Path.GetExtension(FilePath);
        if (!ext.Contains(".py") & !ext.Contains(".pyw") & !ext.Contains(".pyx"))
        {
            GlobalConfig.CurrentLogger.Log("传入参数不正确。应为.py|.pyw|.pyx结尾，实际结尾: " +
                ext,
                EnumLogType.ERROR,
                module: EnumLogModule.CUSTOM, customModuleName: "文件结构分析模块");
        }
    }

    public string Analyze()
    {
        var pythonScript = GlobalConfig.StartupPath.Replace("\\", "/") + "Tools/st_getter.py";
        var startInfo = new ProcessStartInfo
        {
            FileName = "python", // Ensure python is in PATH
            Arguments = $"{pythonScript} \"{FilePath.Replace("\\", "/")}\" \"{GlobalConfig.StartupPath.Replace("\\", "/") + "/Cache"}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(startInfo))
        {
            if (process == null)
            {
                GlobalConfig.CurrentLogger.Error(new InvalidOperationException("无法启动Python进程"),
                    type: EnumLogType.ERROR,
                    module: EnumLogModule.CUSTOM,
                    customModuleName: "文件结构分析模块");
            }
            var _ = process.StandardOutput.ReadToEnd();
            var _1 = process.StandardError.ReadToEnd();
            JSONContent = process.StandardOutput.ReadToEnd();
        }

        JSONPath = GlobalConfig.StartupPath + "\\Cache\\" + Path.GetFileName(FilePath) + ".json";
        //File.WriteAllText(JSONPath, JSONContent);

        return JSONPath;
    }
}
