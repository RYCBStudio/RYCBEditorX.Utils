﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using RYCBEditorX.Utils;

namespace RYCBEditorX;
public class GlobalConfig
{

    /// <summary>
    /// 为了调用主包中的VERSION而创建。
    /// </summary>
    public static string Version
    {
        get; set;
    }
    
    /// <summary>
    /// 为了调用主包中的REVISION_NUMBER而创建。
    /// </summary>
    public static string Revision
    {
        get; set;
    }

    public static string LocalizationString
    {
        get; set;
    }

    /// <summary>
    /// 为了调用主包中的STARTUP_PATH而创建。
    /// </summary>
    public static string StartupPath
    {
        get; set;
    }

    /// <summary>
    /// 为了调用主包中的LOGGER而创建。
    /// </summary>
    public static LogUtil CurrentLogger
    {
        get; set;
    }

    /// <summary>
    /// 为了调用主包中的LocalizationService而创建。
    /// </summary>
    public static LocalizationService LocalizationService
    {
        get; set;
    }

    /// <summary>
    /// 为了调用主包中的Resource而创建。
    /// </summary>
    public static ResourceDictionary Resources
    {
        get; set;
    }

    /// <summary>
    /// 当前的皮肤。
    /// </summary>
    public static string Skin
    {
        get; set;
    }

    /// <summary>
    /// 编辑器支持语法高亮的最大文件大小。
    /// </summary>
    public static int MaximumFileSize
    {
        get; set;
    }

    /// <summary>
    /// Xshd文件路径。
    /// </summary>
    public static string XshdFilePath
    {
        get; set;
    }

    /// <summary>
    /// 代码模板。
    /// </summary>
    public static Dictionary<string, string> CodeTemplates
    {
        get; set;
    } = [];

    public static string CommonTempFilePath => StartupPath + "Cache\\" + DateTime.Now.Ticks;

    public static class Editor
    {
        /// <summary>
        /// 指示<see cref="ICSharpCode.AvalonEdit.TextEditor"/>是否显示行号。
        /// </summary>
        public static bool ShowLineNumber
        {
            get; set;
        }

        /// <summary>
        /// 指示<see cref="ICSharpCode.AvalonEdit.TextEditor"/>的字体。
        /// </summary>
        public static string FontFamilyName
        {
            get; set;
        }

        /// <summary>
        /// 指示<see cref="ICSharpCode.AvalonEdit.TextEditor"/>的字体大小。
        /// </summary>
        public static int FontSize
        {
            get; set;
        }

        /// <summary>
        /// 指示<see cref="ICSharpCode.AvalonEdit.TextEditor"/>的主题。
        /// </summary>
        public static string Theme
        {
            get; set;
        }

        public static Color Fore
        {
            get; set;
        }

        public static Color Back
        {
            get; set;
        }
    }

    public static string GetSkin(string skinName)
    {
        return skinName switch
        {
            "default" => "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml",
            "light" => "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml",
            "dark" => "pack://application:,,,/HandyControl;component/Themes/SkinDark.xaml",
            _ => "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml",
        };
    }

    public static List<RunProfile> CurrentProfiles
    {
        get; set;
    }
    public static RunProfile CurrentRunProfile
    {
        get; set;
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
}
