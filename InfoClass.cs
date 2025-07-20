using System.Collections.Generic;
using System.Windows;

namespace RYCBEditorX.Utils;

public enum NotificationType
{
    Info,
    Warn,
    Error
}

public class AppConfig
{
    public string Skin
    {
        get; set;
    }
    public int MaximumFileSize
    {
        get; set;
    }
    public string Language
    {
        get; set;
    }
    public string PythonPath
    {
        get; set;
    }
    public AutoSaveConfig AutoSave
    {
        get; set;
    }
    public AutoBackupConfig AutoBackup
    {
        get; set;
    }
    public string Font
    {
        get; set;
    }
    public string XshdFilePath
    {
        get; set;
    }
    public EditorConfig Editor
    {
        get; set;
    }
    public DownloadingConfig Downloading
    {
        get; set;
    }
}

public class AutoSaveConfig
{
    public bool Enabled
    {
        get; set;
    }
    public int Interval
    {
        get; set;
    }
}

public class AutoBackupConfig
{
    public bool Enabled
    {
        get; set;
    }
    public int Interval
    {
        get; set;
    }
    public string Path
    {
        get; set;
    }
}

public class EditorConfig
{
    public string Theme
    {
        get; set;
    }
    public bool ShowLineNumber
    {
        get; set;
    }
    public string FontName
    {
        get; set;
    }
    public int FontSize
    {
        get; set;
    }
}

public class DownloadingConfig
{
    public bool ParallelDownload
    {
        get; set;
    }
    public int ParallelCount
    {
        get; set;
    }
}

public static class GlobalWindows
{
    public static List<Window> ActivatingWindows
    {
        get; set;
    }

    public static Window CurrentMainWindow
    {
        get; set;
    }
}

public class RunProfile
{
    public string Name
    {
        get; set;
    }

    public string ScriptPath
    {
        get; set;
    }

    public string ScriptArgs
    {
        get; set;
    }

    public string Interpreter
    {
        get; set;
    }

    public string InterpreterArgs
    {
        get; set;
    }

    public bool UseBPSR
    {
        get; set;
    }
}

public static class Language
{
    public static class Python
    {
        public static List<string> Keywords => ["False", "None", "True", "and", "as", "assert", "async", "await", "break",
            "class", "continue", "def", "del", "elif", "else", "except", "finally", "for", "from", "global", "if", "import",
            "in", "is", "lambda", "nonlocal", "not", "or", "pass", "raise", "return", "try", "while", "with", "yield"];
        public static List<string> MagicMethods => ["__add__", "__sub__", "__mul__", "__floordiv__", "__div__", "__mod__",
            "__pow__", "__lshift__", "__rshift__", "__and__", "__xor__", "__or__", "__iadd__", "__isub__", "__imul__",
            "__idiv__", "__ifloordiv__", "__imod__", "__ipow__", "__ilshift__", "__irshift__", "__iand__", "__ixor__",
            "__ior__", "__neg__", "__pos__", "__abs__", "__invert__", "__complex__", "__int__", "__long__", "__float__",
            "__oct__", "__hex__", "__round__", "__floor__", "__ceil__", "__trunc__", "__lt__", "__le__", "__eq__", "__ne__",
            "__ge__", "__gt__", "__str__", "__repr__", "__len__", "__hash__", "__nonzero__", "__dir__", "__sizeof__", "__len__",
            "__getitem__", "__setitem__", "__delitem__", "__iter__", "__reversed__", "__contains__", "__missing__"];
        public static List<string> BuiltIns => ["abs", "aiter", "all", "anext", "any", "ascii", "bin", "bool", "breakpoint",
            "bytearray", "bytes", "callable", "chr", "classmethod", "compile", "complex", "delattr", "dict", "dir", "divmod",
            "enumerate", "eval", "exec", "filter", "float", "format", "frozenset", "getattr", "globals", "hasattr", "hash",
            "help", "hex", "id", "input", "int", "isinstance", "issubclass", "iter", "len", "list", "locals", "map", "max",
            "memoryview", "min", "next", "object", "oct", "open", "ord", "pow", "print", "property", "range", "repr",
            "reversed", "round", "set", "setattr", "slice", "sorted", "staticmethod", "str", "sum", "super", "tuple",
            "type", "vars", "zip", "__import__"];
    }

    public static class Dictionaries
    {
        public static Dictionary<string, string> LangDict = new()
        {
            {"zh-CN","简体中文" },
            {"en-US","English" },
        };
    }
}

public static class Icons
{
    public const string KEYWORD = "\xe629";
    public const string FUNCTION = "\xe73c";
    public const string PACKAGE = "\xe646";
    public const string BUILTIN = "\xe6bd";
    public const string VARIABLE = "\xe660";
    public const string MAGIC = "\xf03e";
    public const string CT = "\xe76a";

    public const string INFO = "\xe60e";
    public const string WARN = "\xe6d1";
    public const string ERROR = "\xeb37";
}

public enum CompletionDataType
{
    Keyword,
    Function,
    Builtin,
    Variable,
    Magic,
    CodeTemplate
}

public class ProfileInfo
{
    public string Name
    {
        get; set;
    }
}

public class GeneralPackageInfo
{
    public string Name
    {
        get; set;
    }
    public string Version
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
}

public class CodeTemplate
{
    public string Name
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public string Template
    {
        get; set;
    }
    public static void GetTemplate(KeyValuePair<string, string> template, ref List<CodeTemplate> templates)
    {
        templates.Add(new()
        {
            Name = template.Key,
            Template = template.Value
        });
    }
    public static CodeTemplate GetTemplate(KeyValuePair<string, string> template, string desc)
    {
        return new()
        {
            Name = template.Key,
            Description = desc,
            Template = template.Value
        };
    }
}

public class Comment
{
    public string User
    {
        get; set;
    }
    public string Uid
    {
        get; set;
    }
    public string CommentText
    {
        get; set;
    }
    public string Time
    {
        get; set;
    }
    public int Likes
    {
        get; set;
    }
    public string Target
    {
        get; set;
    }
}

public class PythonPackageInfo : GeneralPackageInfo
{
    public string Requires
    {
        get; set;
    }
    public string RequiredBy
    {
        get; set;
    }
    public string Location
    {
        get; set;
    }
    public string Summary
    {
        get; set;
    }
    public string License
    {
        get; set;
    }
    public string Author
    {
        get; set;
    }
}
