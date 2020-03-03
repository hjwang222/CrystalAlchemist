using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;


public class BasicMenu : PreventDeselection
{
    [BoxGroup("Basic")]
    public List<GameObject> menues = new List<GameObject>();

    public virtual void Start()
    {
        this.setFirstMenu();
    }

    public virtual void OnEnable()
    {
        this.setFirstMenu();
    }

    public virtual void OnDisable()
    {
        this.setFirstMenu();
    }

    private void setFirstMenu()
    {
        if(this.menues.Count > 0) CustomUtilities.UI.ShowMenu(this.menues[0], this.menues);
    }

    public virtual void ShowMenu(GameObject menu)
    {
        CustomUtilities.UI.ShowMenu(menu, this.menues);
    }

    public void saveSettings()
    {
        SaveSystem.SaveOptions();
    }
}
