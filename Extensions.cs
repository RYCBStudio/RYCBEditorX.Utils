using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RYCBEditorX.Utils;
public static class Extensions
{
    /// <summary>
    /// 判断字符串是否为<see langword="null"/>或者<see cref="string.Empty"/>
    /// </summary>
    /// <param name="value">需判断的字符串</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// 移除字符串右侧指定字符串(用于SunnyUI兼容)
    /// </summary>
    /// <param name="value">需操作的字符串</param>
    /// <param name="right">删除的字符串</param>
    /// <returns></returns>
    public static string RemoveRight(this string value, string right)
    {
        return value.EndsWith(right) ? value[..^right.Length] : value;
    }

    /// <summary>
    /// 判断一个<see cref="DateTime"/>是否为七天内的时间
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static bool IsWithinSevenDays(this DateTime dateTime)
    {
        DateTime now = DateTime.Now;
        TimeSpan difference = dateTime - now;

        // 判断时间差是否在-7天到7天之间
        return difference.TotalDays >= -7 && difference.TotalDays <= 7;
    }

    /// <summary>
    /// 对于上传至数据库的字段进行转义
    /// </summary>
    /// <param name="value">需转义的字段</param>
    /// <returns>转义后的字段</returns>
    public static string EscapeDatabaseLanguage(this string value)
    {
        return value
            .Replace("\n", "\\n") //换行符
            .Replace("\r", "\\r").Replace("\t", "\\t") //制表符
            .Replace("'", "\uffff") //单引号
            .Replace("--", "\ufffe") //SQL注释
            .Replace("DELETE", "\ufffd", StringComparison.OrdinalIgnoreCase) //SQL语句DELETE
            .Replace("INSERT", "\ufffc", StringComparison.OrdinalIgnoreCase) //SQL语句INSERT
            .Replace("UPDATE", "\ufffb", StringComparison.OrdinalIgnoreCase) //SQL语句UPDATE
            .Replace("SELECT", "\ufffa", StringComparison.OrdinalIgnoreCase) //SQL语句SELECT
            .Replace("DROP", "\ufff9", StringComparison.OrdinalIgnoreCase) //SQL语句DROP
            .Replace("ALTER", "\ufff8", StringComparison.OrdinalIgnoreCase) //SQL语句ALTER
            .Replace("CREATE", "\ufff7", StringComparison.OrdinalIgnoreCase) //SQL语句CREATE
            .Replace("TRUNCATE", "\ufff6", StringComparison.OrdinalIgnoreCase) //SQL语句TRUNCATE
            .Replace("EXEC", "\ufff5", StringComparison.OrdinalIgnoreCase) //SQL语句EXEC
            .Replace("UNION", "\ufff4", StringComparison.OrdinalIgnoreCase) //SQL语句UNION
            .Replace(";", "\ufff3"); //SQL语句结束符
        ;

    }

    /// <summary>
    /// 对于上传至数据库的字段进行反转义
    /// </summary>
    /// <param name="value">需转义的字段</param>
    /// <returns>转义后的字段</returns>
    public static string AntiEscapeDatabaseLanguage(this string value)
    {
        return value
            .AntiReplace("\n", "\\n") //换行符
            .AntiReplace("\r", "\\r").Replace("\t", "\\t") //制表符
            .AntiReplace("'", "\uffff") //单引号
            .AntiReplace("--", "\ufffe") //SQL注释
            .AntiReplace("DELETE", "\ufffd", StringComparison.OrdinalIgnoreCase) //SQL语句DELETE
            .AntiReplace("INSERT", "\ufffc", StringComparison.OrdinalIgnoreCase) //SQL语句INSERT
            .AntiReplace("UPDATE", "\ufffb", StringComparison.OrdinalIgnoreCase) //SQL语句UPDATE
            .AntiReplace("SELECT", "\ufffa", StringComparison.OrdinalIgnoreCase) //SQL语句SELECT
            .AntiReplace("DROP", "\ufff9", StringComparison.OrdinalIgnoreCase) //SQL语句DROP
            .AntiReplace("ALTER", "\ufff8", StringComparison.OrdinalIgnoreCase) //SQL语句ALTER
            .AntiReplace("CREATE", "\ufff7", StringComparison.OrdinalIgnoreCase) //SQL语句CREATE
            .AntiReplace("TRUNCATE", "\ufff6", StringComparison.OrdinalIgnoreCase) //SQL语句TRUNCATE
            .AntiReplace("EXEC", "\ufff5", StringComparison.OrdinalIgnoreCase) //SQL语句EXEC
            .AntiReplace("UNION", "\ufff4", StringComparison.OrdinalIgnoreCase) //SQL语句UNION
            .AntiReplace(";", "\ufff3"); //SQL语句结束符
        ;

    }

    /// <summary>
    /// 字符串替换
    /// </summary>
    /// <param name="str"></param>
    /// <param name="newValue"></param>
    /// <param name="oldValue"></param>
    /// <param name="stringComparison"></param>
    /// <returns></returns>
    public static string AntiReplace(this string str, string newValue, string oldValue, StringComparison stringComparison = StringComparison.CurrentCulture)
    {
        return str.Replace(newValue, oldValue, stringComparison);
    }

    /// <summary>
    /// 使字符串指定位数的字符大写
    /// </summary>
    /// <param name="value"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string ToUpper(this string value, int count)
    {
        return value[..count].ToUpper() + value[count..];
    }

    /// <summary>
    /// 移除字符串右侧指定长度的字符(用于SunnyUI兼容)
    /// </summary>
    /// <param name="value">需操作的字符串</param>
    /// <param name="count">移除字符的个数</param>
    /// <returns></returns>
    public static string RemoveRight(this string value, int count)
    {
        return value[..^count];
    }

    /// <summary>
    /// 判断字符串是否为数字
    /// </summary>
    /// <param name="value">需判断的字符串</param>
    /// <returns></returns>
    public static bool IsNumber(this string value)
    {
        return Regex.IsMatch(value, @"^\d+$");
    }

    /// <summary>
    /// 判断字符串是否不为null或者包含指定字符串
    /// </summary>
    /// <param name="value">需判断的字符串</param>
    /// <param name="pattern">包含的字符串</param>
    /// <returns></returns>
    public static bool ContainsNotNull(this string value, string pattern)
    {
        return value is not null && value.Contains(pattern);
    }

    /// <summary>
    /// 计算字符串的SHA256值
    /// </summary>
    /// <param name="input">需操作的字符串</param>
    /// <returns></returns>
    public static string ComputeSHA256(this string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// 字典尝试获取值
    /// </summary>
    /// <typeparam name="T1">字典的键类型</typeparam>
    /// <typeparam name="T">字典的值类型</typeparam>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static T TryGet<T1, T>(this Dictionary<T1, T> dict, T1 key, T defaultValue = default)
    {
        //try
        //{
        //    GlobalConfig.CurrentLogger.LogDebug("[HIT] key:{0}, value:{1}".Format(key.ToString(), dict[key].ToString()));
        //    return dict[key];
        //}
        //catch (KeyNotFoundException)
        //{
        //    return defaultValue;
        //}
        return defaultValue;
    }

    /// <summary>
    /// 尝试获取字典值。当字典中包含<paramref name="key"/>时，返回字典值；否则返回<paramref name="defaultValue"/>。
    /// 首先考虑相等匹配，其次考虑包含匹配。
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>字典中找到的值或默认值</returns>
    public static List<string> GetIfContains(this Dictionary<string, List<string>> dict, string key, List<string> defaultValue = default)
    {
        // 首先检查字典是否为空
        if (dict == null || dict.Count == 0)
        {
            return defaultValue;
        }

        // 尝试找到相等的键
        if (dict.TryGetValue(key, out var value))
        {
            return value;
        }

        // 如果没有找到相等的键，尝试找到包含的键
        foreach (var kvp in dict)
        {
            if (kvp.Key.Contains(key))
            {
                return kvp.Value; // 如果找到包含的键，返回其值
            }
        }

        // 如果都没有找到，返回默认值
        return defaultValue;
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    /// <param name="input">需计算的字符串</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ComputeMd5(this string input)
    {
        if (input.IsNullOrEmpty())
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

    /// <summary>
    /// 列表去重
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">列表</param>
    /// <returns></returns>
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

    public static string Format(this string value, params string[] args)
    {
        return string.Format(value, args);
    }

    /// <summary>
    /// 不重不漏地向<see cref="IList{T}"/>中添加元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objects">需添加元素的<see cref="IList{T}"/></param>
    /// <param name="_object">需添加的元素</param>
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



    /// <summary>
    /// 根据<paramref name="fileSize"/>的大小自动返回对应的文件大小值。
    /// <br/>
    /// 如：若<paramref name="fileSize"/>32743879328,则返回30.50GB；
    /// 返回值的数值范围为1~1000。
    /// </summary>
    /// <param name="fileSize">文件大小，单位为Bytes</param>
    /// <returns>处理后的文件大小值。</returns>
    public static string ProcessFileSize(long fileSize)
    {
        string[] sizeUnits = ["B", "KB", "MB", "GB", "TB"];
        double size = fileSize;
        var unitIndex = 0;

        while (size >= 1024 && unitIndex < sizeUnits.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{Math.Round(size, 2)}{sizeUnits[unitIndex]}";
    }

    /// <summary>
    /// 显示内部消息的方法。
    /// </summary>
    public static Func<string, string, bool> ShowTip
    {
        get; set;
    }
}
