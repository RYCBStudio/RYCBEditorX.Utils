using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Collections.Generic;

namespace RYCBEditorX.Utils;

public static class PythonSyntaxChecker
{
    private static readonly ScriptEngine Engine = Python.CreateEngine();

    public static List<PyCodeErr> CheckSyntax(string code)
    {
        var errors = new List<PyCodeErr>();

        try
        {
            var source = Engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            source.Compile();
        }
        catch (SyntaxErrorException ex)
        {
            errors.Add(new PyCodeErr
            {
                Type = PyCodeErr.ErrorType.SyntaxError,
                Message = ex.Message,
                LineNumber = ex.Line,
                CodeSnippet = GetLine(code, ex.Line)
            });
        }

        return errors;
    }

    private static string GetLine(string code, int lineNumber)
    {
        var lines = code.Split('\n');
        return lineNumber > 0 && lineNumber <= lines.Length
            ? lines[lineNumber - 1].Trim()
            : string.Empty;
    }
}
