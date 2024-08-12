using System;

namespace RYCBEditorX.Utils;
public static class Extensions
{
    public static bool IsNullOrEmptyEx(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <exception cref="InvalidCastException"/>
    /// <returns></returns>
    public static T TryConvertTo<T>(this object value)
    {
        return (T)value;
    }

    public static string FormatEx(this string value, params string[] args)
    {
        return string.Format(value, args);
    }

    /// <summary>
    /// 将<see cref="System.Drawing.Color"/>转换为<see cref="System.Windows.Media.Color"。/>
    /// </summary>
    /// <param name="color"></param>
    /// <returns>转换结果</returns>
    /// <exception cref="ArgumentException"></exception>
    public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color color)
    {
        return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
