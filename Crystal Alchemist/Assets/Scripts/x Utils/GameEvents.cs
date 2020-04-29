using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField]
    private MasterManager _globalGameObjects;

    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public Action OnSubmit;
    public Action OnInventory;
    public Action OnPause;
    public Action OnCancel;
    public Action<int> OnPage;

    public void DoSubmit()
    {
        if (this.OnSubmit != null) OnSubmit();
    }

    public void DoInventory()
    {
        if (this.OnInventory != null) OnInventory();
    }

    public void DoPause()
    {
        if (this.OnPause != null) OnPause();
    }

    public void DoCancel()
    {
        if (this.OnCancel != null) OnCancel();
    }

    public void DoPage(int page)
    {
        if (this.OnPage != null) OnPage(page);
    }
}
