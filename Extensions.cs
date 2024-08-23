using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RYCBEditorX.Utils;
public static class Extensions
{
    public static bool IsNullOrEmptyEx(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static string ComputeHash(this string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    public static string ComputeMd5(this string input)
    {
        if (input.IsNullOrEmptyEx())
        {
            throw new ArgumentNullException(nameof(input), "Input cannot be null or empty");
        }
        var inputBytes = Encoding.UTF8.GetBytes(input);

        // Convert the byte array to a hexadecimal string
        var sb = new StringBuilder();
        for (var i = 0; i < MD5.HashData(inputBytes).Length; i++)
        {
            sb.Append(MD5.HashData(inputBytes)[i].ToString("x2")); // x2 formats the byte as a two-digit hex number
        }

        return sb.ToString();
    }

    public static IList<T> RemoveDuplicates<T>(this IList<T> list)
    {
        // 使用 HashSet 来实现去重
        var seenItems = new HashSet<T>();
        IList<T> result = [];

        foreach (var item in list)
        {
            // 如果 HashSet 中不存在该项，则添加到 HashSet 和结果列表
            if (seenItems.Add(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    /// <summary>
    /// 判断文本是否全是字母组合
    /// </summary>
    /// <param name="text">需判断的文本或是字符串</param>
    /// <returns>返回true代表有字母存在</returns>
    public static bool IsAllChar(this string text)
    {
        return Regex.Matches(text, "[a-zA-Z]").Count > 0;
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

    public static void AddNotNullAndNoRepeat<T>(this IList<T> objects, T _object)
    {
        if (_object is not null & _object is not DBNull & !objects.Contains(_object))
        {
            objects.Add(_object);
        }
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
