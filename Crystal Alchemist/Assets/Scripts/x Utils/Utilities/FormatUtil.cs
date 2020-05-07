using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

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

    public static void getStartTime(float factor, out int hour, out int minute)
    {
        DateTime origin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
        TimeSpan diff = DateTime.Now - origin;
        double difference = Math.Floor(diff.TotalSeconds);
        float minutes = (float)(difference / (double)factor); //elapsed ingame minutes
        float fhour = ((minutes / 60f) % 24f);
        float fminute = (minutes % 60f);

        if (fhour >= 24) fhour = 0;
        if (fminute >= 60) fminute = 0;

        hour = Mathf.RoundToInt(fhour);
        minute = Mathf.RoundToInt(fminute);
    }
}
