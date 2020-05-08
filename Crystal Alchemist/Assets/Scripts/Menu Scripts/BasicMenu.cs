using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class BasicMenu : PreventDeselection
{
    [BoxGroup("Basic")]
    public List<GameObject> menues = new List<GameObject>();

    [BoxGroup("Mandatory")]
    [Required]
    public CustomCursor cursor;

    [HideInInspector]
    public bool inputPossible;

    public virtual void Start()
    {
        this.setFirstMenu();  
    }

    public virtual void OnEnable()
    {
        StartCoroutine(this.delayCo());
        this.setFirstMenu();
    }

    public virtual void OnDisable()
    {
        this.setFirstMenu();
    }

    public virtual void OnDestroy()
    {
           
    }

    private void setFirstMenu()
    {
        if(this.menues.Count > 0) ShowMenu(this.menues[0], this.menues);
    }

    public virtual void ShowMenu(GameObject menu)
    {
        if(this.inputPossible) ShowMenu(menu, this.menues);
    }

    public void saveSettings()
    {
        SaveSystem.SaveOptions();
    }

    private IEnumerator delayCo()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.1f);
        this.inputPossible = true;
    }

    private void ShowMenu(GameObject newActiveMenu, List<GameObject> menues)
    {
        foreach (GameObject gameObject in menues) gameObject.SetActive(false);        

        if (newActiveMenu != null && menues.Count > 0)
        {
            newActiveMenu.SetActive(true);

            for (int i = 0; i < newActiveMenu.transform.childCount; i++)
            {
                ButtonExtension temp = newActiveMenu.transform.GetChild(i).GetComponent<ButtonExtension>();
                if (temp != null) temp.SetFirst();
            }
        }
    }
}
