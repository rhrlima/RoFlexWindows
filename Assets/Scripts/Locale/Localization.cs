using System;
using System.Collections.Generic;

[Serializable]
public class Localization
{
    public string name;
    public string version;
    public string languageName;
    public string languageCode;
    public List<LocalizationGroup> data;

    [NonSerialized] public Dictionary<string, string> parsedData;

    public void ParseData()
    {
        parsedData ??= new();

        foreach (var groupData in data)
        {
            foreach (var entry in groupData.entries)
            {
                var key = $"{groupData.group}.{entry.key}";
                parsedData[key] = entry.value;
            }
        }
    }
}

[Serializable]
public class LocalizationGroup
{
    public string group;
    public List<LocalizationEntry> entries;
}

[Serializable]
public class LocalizationEntry
{
    public string key;
    public string value;
}
