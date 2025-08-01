using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Services.Common;

namespace RYCBEditorX.Utils;
public static class StaticAnalyzer
{
    public static List<PyCodeErr> CheckUndefinedVariables(string code)
    {
        var errors = new List<PyCodeErr>();
        var lines = code.Split('\n');

        // 简单示例：检查明显的未定义变量
        var regex = new Regex(@"^(?!\s*(def|class|import|from|for|if|elif|while|with)\b).*?\b([a-zA-Z_]\w*)\b(?!\s*=)");

        for (int i = 0; i < lines.Length; i++)
        {
            var matches = regex.Matches(lines[i]);
            foreach (Match match in matches)
            {
                var varName = match.Groups[2].Value;
                if (!IsBuiltin(varName) && !IsDefinedInScope(code, varName, i))
                {
                    errors.Add(new PyCodeErr
                    {
                        Type = PyCodeErr.ErrorType.NameError,
                        Message = $"可能未定义的变量: {varName}",
                        LineNumber = i + 1,
                        CodeSnippet = lines[i].Trim()
                    });
                }
            }
        }

        return errors;
    }

    public static bool IsBuiltin(string name)
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

    public static bool IsDefinedInScope(string code, string varName, int currentLine)
    {
        // 简化实现：检查变量是否在当前行之前定义过
        var lines = code.Split('\n');
        for (int i = 0; i < currentLine; i++)
        {
            if (Regex.IsMatch(lines[i], $@"(def|class)\s+{varName}\b") ||
                Regex.IsMatch(lines[i], $@"{varName}\s*="))
            {
                return true;
            }
        }
        return false;
    }
}
