using System;

#nullable enable

namespace RYCBEditorX.Utils;
public class SQLConnectionException : Exception
{
    public SQLConnectionException() : base() { }

    public SQLConnectionException(string? message) : base(message) { }

    public SQLConnectionException(string? message,  Exception? inner) : base(message, inner) { }
}
