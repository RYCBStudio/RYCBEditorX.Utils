using System.Collections.Generic;
using System.Windows;

namespace RYCBEditorX.Utils;

public static class GlobalWindows
{
    public static List<Window> ActivatingWindows
    {
        get; set;
    }
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
