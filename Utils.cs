using MelonLoader;
using UnityEngine;
using FeatSettings;
using System.Reflection;
using Il2Cpp;


namespace FeatSettings
{
	public static class Utils
	{
        public static string AddOrUpdateField(string json, string key, string value, bool quoteValue = true)
        {
            if (string.IsNullOrWhiteSpace(json))
                json = "{}";

            json = json.Trim();

            if (json.EndsWith("}"))
                json = json.Substring(0, json.Length - 1);

            string searchKey = $"\"{key}\"";
            int idx = json.IndexOf(searchKey, StringComparison.Ordinal);

            string formattedValue = quoteValue ? $"\"{EscapeJson(value)}\"" : value;

            if (idx >= 0)
            {
                int colonIdx = json.IndexOf(':', idx);
                if (colonIdx >= 0)
                {
                    int endIdx = json.IndexOf(',', colonIdx);
                    if (endIdx == -1)
                        endIdx = json.Length;

                    string before = json.Substring(0, colonIdx + 1);
                    string after = json.Substring(endIdx);
                    json = before + " " + formattedValue + after;
                }
            }
            else
            {
                if (json.Length > 1) 
                    json += ",";
                json += $"\"{key}\":{formattedValue}";
            }

            return json + "}";
        }

        public static bool TryGetField(string json, string key, out string value)
        {
            value = "";
            if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(key))
                return false;

            json = json.Trim();
            string searchKey = $"\"{key}\"";

            int idx = json.IndexOf(searchKey, StringComparison.Ordinal);
            if (idx == -1)
                return false;

            int quoteCount = 0;
            for (int i = 0; i < idx; i++)
            {
                if (json[i] == '"' && (i == 0 || json[i - 1] != '\\'))
                    quoteCount++;
            }
            if (quoteCount % 2 != 0)
                return false;

            int colonIdx = json.IndexOf(':', idx);
            if (colonIdx == -1)
                return false;

            int start = colonIdx + 1;
            int end = FindValueEnd(json, start);
            string rawValue = json.Substring(start, end - start).Trim();

            if (rawValue.StartsWith("\"") && rawValue.EndsWith("\""))
            {
                rawValue = rawValue.Substring(1, rawValue.Length - 2);
                rawValue = rawValue.Replace("\\\"", "\"").Replace("\\\\", "\\");
            }

            value = rawValue;
            return true;
        }


        private static int FindValueEnd(string json, int startIdx)
        {
            bool inQuotes = false;
            for (int i = startIdx; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"' && (i == 0 || json[i - 1] != '\\'))
                    inQuotes = !inQuotes;

                if (!inQuotes && (c == ',' || c == '}'))
                    return i;
            }
            return json.Length;
        }

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}