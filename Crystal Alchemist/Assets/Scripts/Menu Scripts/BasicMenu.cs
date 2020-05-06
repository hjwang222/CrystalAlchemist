using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class BasicMenu : PreventDeselection
{
    [BoxGroup("Basic")]
    public List<GameObject> menues = new List<GameObject>();

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

    private void setFirstMenu()
    {
        if(this.menues.Count > 0) GameUtil.ShowMenu(this.menues[0], this.menues);
    }

    public virtual void ShowMenu(GameObject menu)
    {
        if(this.inputPossible) GameUtil.ShowMenu(menu, this.menues);
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
}
