using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> menues = new List<GameObject>(); 

    [SerializeField]
    private AudioClip music;

    [Required]
    [SerializeField]
    private StringValue saveGameSlot;

    [SerializeField]
    private GameObject mainFrame;
    [SerializeField]
    private GameObject darkFrame;

    private string lastSavepoint = null;

    void Start()
    {
        this.mainFrame.SetActive(true);
        if (this.darkFrame != null) this.darkFrame.SetActive(false);

        Cursor.visible = true;
        showMenu(this.menues[0]);
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Cancel")) showMenu();
    }

    private void LateUpdate()
    {
        if (!Cursor.visible) Cursor.visible = true;
    }
       
    public void exitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void startTheGame(string scene, string saveSlot)
    {
        this.saveGameSlot.setValue(saveSlot);
        SceneManager.LoadSceneAsync(scene);
        Cursor.visible = false;
    }

    public void save()
    {
        SaveSystem.SaveOptions();        
    }

    private void OnDisable()
    {
        showMenu(this.menues[0]);
        this.mainFrame.SetActive(true);
    }

    public void showMenu(GameObject newActiveMenu)
    {
        foreach(GameObject gameObject in this.menues)
        {
            gameObject.SetActive(false);
        }

        if (newActiveMenu != null)
        {
            newActiveMenu.SetActive(true);

            for (int i = 0; i < newActiveMenu.transform.childCount; i++)
            {
                ButtonExtension temp = newActiveMenu.transform.GetChild(i).GetComponent<ButtonExtension>();
                if (temp != null && temp.setFirstSelected)
                {
                    temp.setFirst();
                    break;
                }
            }
        }
    }

    public void showBackground(bool dark)
    {
        this.mainFrame.SetActive(false);
        if(this.darkFrame != null) this.darkFrame.SetActive(false);

        if (dark && this.darkFrame != null) this.darkFrame.SetActive(true);
        else if(!dark) this.mainFrame.SetActive(true);
    }

   
}
