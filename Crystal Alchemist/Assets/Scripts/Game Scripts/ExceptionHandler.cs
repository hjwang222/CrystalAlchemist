using System;
using System.IO;
using UnityEngine;

public class ExceptionHandler : MonoBehaviour
{
    private StreamWriter m_Writer;

    [SerializeField]
    private int maxFileSize = 5;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;

        try
        {
            string folder = Path.Combine(Application.dataPath, "Fehlerprotokoll");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            long bytes = this.maxFileSize * 1000000;
            string path = Path.Combine(folder, "log.txt");
            if (File.Exists(path) && new FileInfo(path).Length > bytes) File.Delete(path);

            m_Writer = new StreamWriter(path, true);
            m_Writer.AutoFlush = true;
        }
        catch
        {

        }
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        try
        {
            if (type == LogType.Exception && this.m_Writer != null)
            {
                string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                m_Writer.WriteLine("{0} {1}: {2}\r\n{3}", type, date, logString, stackTrace.Replace("\n", Environment.NewLine));
            }
        }
        catch
        {

        }
    }
}