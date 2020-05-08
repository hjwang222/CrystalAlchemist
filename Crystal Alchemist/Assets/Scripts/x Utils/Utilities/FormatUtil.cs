using UnityEngine.UI;
using TMPro;
using UnityEngine;

public static class FormatUtil
{
    public static void SetButtonColor(Button button, Color newColor)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = newColor;
        button.colors = cb;
    }

    public static string getLanguageDialogText(string originalText, string alternativeText)
    {
        if (MasterManager.settings.useAlternativeLanguage && alternativeText.Replace(" ", "").Length > 1) return alternativeText;
        else return originalText;
    }

    public static string formatFloatToString(float value, float schwelle)
    {
        if (Mathf.Abs(value) >= 10) return value.ToString("N0");
        else if (value % 1 == 0) return value.ToString("N0");

        return value.ToString("N1");
    }

    public static string setDurationToString(float value)
    {
        int rounded = Mathf.RoundToInt(value);

        if (rounded > 59) return (rounded / 60).ToString("0") + "m";
        else return rounded.ToString("0") + "s";
    }

    public static string formatString(float value, float maxValue)
    {
        string formatString = "";

        for (int i = 0; i < maxValue.ToString().Length; i++)
        {
            formatString += "0";
        }

        if (value == 0) return formatString;
        else return value.ToString(formatString);
    }

    public static void set3DText(TextMeshPro tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
    {
        if (tmp != null)
        {
            if (text != null) tmp.text = text + "";
            if (bold) tmp.fontStyle = FontStyles.Bold;
            if (outlineColor != null) tmp.outlineColor = outlineColor;
            if (fontColor != null) tmp.color = fontColor;
            if (outlineWidth > 0) tmp.outlineWidth = outlineWidth;
        }
    }

    public static void set3DText(TextMeshProUGUI tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
    {
        if (tmp != null)
        {
            if (text != null) tmp.text = text + "";
            if (bold) tmp.fontStyle = FontStyles.Bold;
            if (outlineColor != null) tmp.outlineColor = outlineColor;
            if (fontColor != null) tmp.color = fontColor;
            if (outlineWidth > 0) tmp.outlineWidth = outlineWidth;
        }
    }
}
