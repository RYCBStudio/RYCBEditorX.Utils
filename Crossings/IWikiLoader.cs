using System;
using System.Collections.Generic;

namespace RYCBEditorX.Crossing;
public class IWikiLoader:ICrossing
{
    public static Func<string, List<string>> GetWiki { get; set; }
    public static Func<List<string>> GetAllTargets { get; set; }
    public void Register()
    {
        //throw new NotImplementedException();
    }
}
