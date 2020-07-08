using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TitleScreenAutoUpdate : MonoBehaviour
{
    //1. Check Version
    //2. Show Info
    //3. Download
    //4. Launch Updater
    private UnityWebRequest www;

    private bool isDownloading = false;
    private string current;

    private void Start()
    {
        this.current = Directory.GetCurrentDirectory();
        this.www = new UnityWebRequest("http://www.gungnir-arts.com/Download/Patch.zip");
    }

    [Button]
    private void Download()
    {
        StartCoroutine(GetRequest());
    }

    private void Update()
    {
        if(this.isDownloading) UnityEngine.Debug.Log(this.www.downloadProgress);
    }

    IEnumerator GetRequest()
    {
        this.isDownloading = true;
        string path = Path.Combine(current, "Patch.zip");
        www.downloadHandler = new DownloadHandlerFile(path);

        yield return www.SendWebRequest();

        this.isDownloading = false;

        if (www.isNetworkError || www.isHttpError) UnityEngine.Debug.Log(www.error);
        else
        {
            UnityEngine.Debug.Log("Success " + path);            
            Application.Quit();

            path = Path.Combine(current, "CrystalAlchemistUpdater.exe");
            Process.Start(path);
        }
    }
}
