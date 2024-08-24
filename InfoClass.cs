using System.Collections.Generic;
using System.Windows;

namespace RYCBEditorX.Utils;

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
