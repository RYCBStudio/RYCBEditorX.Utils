using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RYCBEditorX.Utils;
public class LocalizationService
{
    private Dictionary<string, string> _localizationDictionary;
    public Dictionary<string, string> LocalizationDictionary
    {
        get; private set;
    }

    public LocalizationService(string filePath)
    {
        LoadLocalization(filePath);
    }

    public void LoadLocalization(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _localizationDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            LocalizationDictionary = _localizationDictionary;
        }
    }

    public string GetLocalizedString(string key)
    {
        return _localizationDictionary.TryGetValue(key, out var value) ? value : key;
    }
}