using System;
using System.IO;
using UnityEngine;

namespace DefaultNamespace.Utils
{
    public class BHUtils
    {
        public static bool AddElementEnum<T>(string nameElement, bool autoValue = false, string filePath = "Assets/Scripts/GameEnums.cs") where T : Enum
        {
            // Đọc nội dung của file MyEnums.cs
            string enumContent = File.ReadAllText(filePath);

            // Tìm enum cần chỉnh sửa
            string enumName = typeof(T).Name;
            int startIndex = enumContent.IndexOf($"public enum {enumName}", StringComparison.Ordinal);
            int endIndex = enumContent.IndexOf('}', startIndex);

            // Tách enum content
            string enumDeclaration = enumContent.Substring(startIndex, endIndex - startIndex + 1);
            
            // Kiểm tra xem phần tử đã tồn tại trong enum chưa
            if (Enum.IsDefined(typeof(T), nameElement))
            {
                BHDebug.LogError($"Phần tử '{nameElement}' đã tồn tại trong enum '{enumName}'.");
                return false;
            }

            // Thêm giá trị mới vào enum
            string newValue = autoValue
                ? $"\n\t{nameElement} = {(int)Enum.Parse(typeof(T), Enum.GetValues(typeof(T)).Length.ToString())},"
                : $"\n\t{nameElement},";
            enumDeclaration = enumDeclaration.Insert(enumDeclaration.Length - 2, newValue);

            // Chèn lại giá trị vào file
            enumContent = enumContent.Remove(startIndex, endIndex - startIndex + 1).Insert(startIndex, enumDeclaration);
            File.WriteAllText(filePath, enumContent);

            BHDebug.LogSuccess($"Phần tử '{nameElement}' đã được thêm vào enum '{enumName}'.");
            return true;
        }
    }
}


public static class BHDebug
{
    private const string PrefixNormal = "<b>[b100] </b>";
    private const string PrefixWarning = "<b><color=yellow>[b100] </color></b>";
    private const string PrefixError = "<b><color=red>[b100] </color></b>";
    private const string Prefix = "<b>[b100] </b>";
    private const string PrefixSuccess = "<b><color=blue>[b100] </color></b>";

    public static void Log(object o)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.Log(PrefixNormal + o);
#endif
    }

    public static void LogWarning(object o)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.LogWarning(PrefixWarning + o);
#endif
    }

    public static void LogError(object o)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.LogError(PrefixError + o);
#endif
    }

    public static void Log(object o, DebugColor color)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.Log(string.Format("{0}<color={1}>{2}</color>", Prefix, color.ToString().ToLower(), o));
#endif
    }
    
    public static void LogSuccess(object o)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.Log(PrefixSuccess + o);
#endif
    }
}

public enum DebugColor
{
    White,
    Black,
    Red,
    Green,
    Blue,
    Orange,
    Violet,
    Aqua,
    Gray,
    Magenta,
    Purple,
    Yellow
}