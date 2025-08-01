// PythonErrorAnalyzer.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using Microsoft.VisualStudio.Services.Common;

namespace RYCBEditorX.Utils;
public static class PythonErrorAnalyzer
{

    private static readonly ScriptEngine PythonEngine = Python.CreateEngine();

    public static List<PyCodeErr> AnalyzeCode(string code)
    {
        var errors = new List<PyCodeErr>();

        // 1. 语法检查
        errors.AddRange(CheckSyntaxErrors(code));

        // 2. 只有语法正确时才进行静态分析
        if (errors.Count == 0)
        {
            errors.AddRange(CheckStaticIssues(code));
        }

        return errors;
    }

    private static List<PyCodeErr> CheckStaticIssues(string code)
    {
        var errors = new List<PyCodeErr>();

        // 检查未定义变量
        var varRegex = new Regex(@"(?<!\w\.)(?<var>[a-zA-Z_]\w*)(?!\s*=)[^\.\w]");
        foreach (Match match in varRegex.Matches(code))
        {
            var varName = match.Groups["var"].Value;
            if (!IsBuiltin(varName) && !IsDefinedInCode(code, varName, match.Index))
            {
                int lineNum = GetLineNumber(code, match.Index);
                errors.Add(new PyCodeErr
                {
                    Type = PyCodeErr.ErrorType.NameError,
                    Message = $"可能未定义的变量: {varName}",
                    LineNumber = lineNum,
                    CodeSnippet = GetLine(code, lineNum),
                    FilePath = "当前编辑的文件"
                });
            }
        }

        return errors;
    }

    private static bool IsBuiltin(string name)
    {
        var builtins = new HashSet<string>
        {
            "print", "range", "len", "str", "int", "list", "dict",
            "True", "False", "None", "and", "or", "not", "in", "is",
            "def", "class", "import", "from", "return", "if", "else",
            "elif", "for", "while", "try", "except", "finally", "with"
        };
        builtins.AddRange(Language.Python.BuiltIns);
        builtins.AddRange(Language.Python.Keywords);
        builtins.AddRange(Language.Python.MagicMethods);
        return builtins.Contains(name);
    }

    private static bool IsDefinedInCode(string code, string varName, int beforeIndex)
    {
        // 检查变量是否在指定位置前定义过
        var definitionPattern = $@"(def|class)\s+{varName}\b|{varName}\s*=";
        return Regex.IsMatch(code.Substring(0, beforeIndex), definitionPattern);
    }

    private static string GetLine(string code, int lineNumber)
    {
        if (string.IsNullOrEmpty(code) || lineNumber < 1)
            return string.Empty;

        using (var reader = new StringReader(code))
        {
            string line;
            int currentLine = 1;
            while ((line = reader.ReadLine()) != null)
            {
                if (currentLine == lineNumber)
                    return line.Trim();
                currentLine++;
            }
        }
        return string.Empty;
    }

    private static int GetLineNumber(string code, int charIndex)
    {
        if (string.IsNullOrEmpty(code) || charIndex < 0)
            return 1;

        int lineNumber = 1;
        for (int i = 0; i < Math.Min(charIndex, code.Length); i++)
        {
            if (code[i] == '\n')
                lineNumber++;
        }
        return lineNumber;
    }

    public static async Task<List<PyCodeErr>> AnalyzePythonFileAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            var errors = new List<PyCodeErr>();

            if (!File.Exists(filePath))
            {
                errors.Add(new PyCodeErr
                {
                    Type = PyCodeErr.ErrorType.OtherError,
                    Message = "File not found",
                    LineNumber = 0,
                    FilePath = filePath,
                    CodeSnippet = string.Empty
                });
                return errors;
            }

            // 1. 首先检查语法错误
            var syntaxErrors = CheckSyntaxErrors(filePath);
            if (syntaxErrors.Count > 0)
            {
                return syntaxErrors;
            }

            // 2. 检查运行时错误
            var runtimeErrors = CheckRuntimeErrors(filePath);
            errors.AddRange(runtimeErrors);

            return errors;
        });
    }

    public static List<PyCodeErr> AnalyzePythonFile(string filePath)
    {
        var errors = new List<PyCodeErr>();

        if (!File.Exists(filePath))
        {
            errors.Add(new PyCodeErr
            {
                Type = PyCodeErr.ErrorType.OtherError,
                Message = "File not found",
                LineNumber = 0,
                FilePath = filePath,
                CodeSnippet = string.Empty
            });
            return errors;
        }

        // 1. 首先检查语法错误
        var syntaxErrors = CheckSyntaxErrors(filePath);
        if (syntaxErrors.Count > 0)
        {
            return syntaxErrors; // 如果有语法错误，直接返回，因为无法运行
        }

        // 2. 检查运行时错误
        var runtimeErrors = CheckRuntimeErrors(filePath);
        errors.AddRange(runtimeErrors);

        return errors;
    }

    private static List<PyCodeErr> CheckSyntaxErrors(string filePath)
    {
        var errors = new List<PyCodeErr>();
        var pythonExe = FindPythonExecutable();

        if (pythonExe == null)
        {
            errors.Add(new PyCodeErr
            {
                Type = PyCodeErr.ErrorType.OtherError,
                Message = "Python executable not found",
                LineNumber = 0,
                FilePath = filePath,
                CodeSnippet = string.Empty
            });
            return errors;
        }

        var processStartInfo = new ProcessStartInfo
        {
            FileName = pythonExe,
            Arguments = $"-m py_compile \"{filePath}\"",
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardErrorEncoding = Encoding.UTF8
        };

        using var process = Process.Start(processStartInfo);
        var errorOutput = process.StandardError.ReadToEnd();
        process.WaitForExit(5000); // 等待最多5秒

        if (!string.IsNullOrEmpty(errorOutput))
        {
            var syntaxError = ParsePythonError(errorOutput, filePath);
            if (syntaxError != null)
            {
                errors.Add(syntaxError);
            }
        }

        return errors;
    }

    private static List<PyCodeErr> CheckRuntimeErrors(string filePath)
    {
        var errors = new List<PyCodeErr>();
        var pythonExe = FindPythonExecutable();

        if (pythonExe == null)
        {
            errors.Add(new PyCodeErr
            {
                Type = PyCodeErr.ErrorType.OtherError,
                Message = "Python executable not found",
                LineNumber = 0,
                FilePath = filePath,
                CodeSnippet = string.Empty
            });
            return errors;
        }

        var processStartInfo = new ProcessStartInfo
        {
            FileName = pythonExe,
            Arguments = $"\"{filePath}\"",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardErrorEncoding = Encoding.UTF8,
            StandardOutputEncoding = Encoding.UTF8
        };

        using var process = Process.Start(processStartInfo);
        var errorOutput = process.StandardError.ReadToEnd();
        var stdOutput = process.StandardOutput.ReadToEnd();
        process.WaitForExit(5000); // 等待最多5秒

        if (!string.IsNullOrEmpty(errorOutput))
        {
            var runtimeErrors = ParseRuntimeErrors(errorOutput, filePath);
            errors.AddRange(runtimeErrors);
        }

        return errors;
    }

    private static PyCodeErr ParsePythonError(string errorOutput, string filePath)
    {
        // 示例错误格式:
        //   File "test.py", line 2
        //     print("Hello world"
        //                        ^
        // SyntaxError: unexpected EOF while parsing

        var lines = errorOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2) return null;

        // 解析第一行获取文件名和行号
        var firstLineMatch = Regex.Match(lines[0], @"File ""(.+)"", line (\d+)");
        if (!firstLineMatch.Success) return null;

        var lineNumber = int.Parse(firstLineMatch.Groups[2].Value);
        var codeSnippet = lines.Length > 1 ? lines[1].Trim() : string.Empty;

        // 解析错误类型和消息
        var errorType = PyCodeErr.ErrorType.SyntaxError;
        var errorMessage = lines[^1].Trim();

        if (lines.Length > 2)
        {
            var lastLine = lines[^1];
            if (lastLine.StartsWith("SyntaxError:"))
            {
                errorMessage = lastLine["SyntaxError:".Length..].Trim();
            }
            else if (lastLine.StartsWith("IndentationError:"))
            {
                errorMessage = lastLine["IndentationError:".Length..].Trim();
                errorType = PyCodeErr.ErrorType.SyntaxError;
            }
        }

        return new PyCodeErr
        {
            Type = errorType,
            Message = errorMessage,
            LineNumber = lineNumber,
            FilePath = filePath,
            CodeSnippet = codeSnippet
        };
    }

    private static List<PyCodeErr> ParseRuntimeErrors(string errorOutput, string filePath)
    {
        var errors = new List<PyCodeErr>();
        var lines = errorOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0) return errors;

        // 示例运行时错误格式:
        // Traceback (most recent call last):
        //   File "test.py", line 3, in <module>
        //     print(undefined_var)
        // NameError: name 'undefined_var' is not defined

        var tracebackStart = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("Traceback (most recent call last):"))
            {
                tracebackStart = i;
                break;
            }
        }

        if (tracebackStart == -1)
        {
            // 不是标准traceback格式的错误
            errors.Add(new PyCodeErr
            {
                Type = PyCodeErr.ErrorType.OtherError,
                Message = errorOutput.Trim(),
                LineNumber = 0,
                FilePath = filePath,
                CodeSnippet = string.Empty
            });
            return errors;
        }

        // 解析traceback
        if (tracebackStart + 2 >= lines.Length) return errors;

        // 获取错误发生的位置
        var fileLineMatch = Regex.Match(lines[tracebackStart + 1], @"File ""(.+)"", line (\d+), in (.+)");
        if (!fileLineMatch.Success) return errors;

        var lineNumber = int.Parse(fileLineMatch.Groups[2].Value);
        var codeSnippet = tracebackStart + 2 < lines.Length ? lines[tracebackStart + 2].Trim() : string.Empty;

        // 获取错误类型和消息
        var errorLine = lines[^1];
        PyCodeErr.ErrorType errorType;
        string errorMessage;

        if (errorLine.StartsWith("NameError:"))
        {
            errorType = PyCodeErr.ErrorType.NameError;
            errorMessage = errorLine["NameError:".Length..].Trim();
        }
        else if (errorLine.StartsWith("TypeError:"))
        {
            errorType = PyCodeErr.ErrorType.TypeError;
            errorMessage = errorLine["TypeError:".Length..].Trim();
        }
        else if (errorLine.StartsWith("ValueError:"))
        {
            errorType = PyCodeErr.ErrorType.ValueError;
            errorMessage = errorLine["ValueError:".Length..].Trim();
        }
        else if (errorLine.StartsWith("ImportError:"))
        {
            errorType = PyCodeErr.ErrorType.ImportError;
            errorMessage = errorLine["ImportError:".Length..].Trim();
        }
        else if (errorLine.StartsWith("RuntimeError:"))
        {
            errorType = PyCodeErr.ErrorType.RuntimeError;
            errorMessage = errorLine["RuntimeError:".Length..].Trim();
        }
        else
        {
            errorType = PyCodeErr.ErrorType.OtherError;
            errorMessage = errorLine;
        }

        errors.Add(new PyCodeErr
        {
            Type = errorType,
            Message = errorMessage,
            LineNumber = lineNumber,
            FilePath = filePath,
            CodeSnippet = codeSnippet
        });

        return errors;
    }

    private static string FindPythonExecutable()
    {
        if (File.Exists(GlobalConfig.PythonPath)) return GlobalConfig.PythonPath;
        // 检查常见Python安装路径
        var possiblePaths = new List<string>
        {
            "python", // 如果在PATH中
            "python3",
            "py",
            @"C:\Python310\python.exe",
            @"C:\Python311\python.exe",
            @"C:\Python312\python.exe",
            @"C:\Python313\python.exe",
            @"C:\Program Files\Python310\python.exe",
            @"C:\Program Files\Python312\python.exe",
            @"C:\Program Files\Python311\python.exe",
            @"C:\Program Files\Python313\python.exe",
        };

        foreach (var path in possiblePaths)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processStartInfo);
                process.WaitForExit(1000); // 等待1秒

                if (process.ExitCode == 0)
                {
                    return path;
                }
            }
            catch
            {
                // 忽略异常，继续尝试下一个路径
            }
        }

        return null;
    }
}