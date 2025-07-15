using System.Collections.Generic;

namespace RYCBEditorX.Utils.Crossings;
public class GlobalMsgCrossing
{
    public static bool HasGlobalMsg
    {
        get; set;
    }

    public static List<Message> GlobalMsg
    {
        get; set;
    } = [];
}

public class Message
{
    public string? Text { get; set; }
    public int? Level { get; set; }
}
