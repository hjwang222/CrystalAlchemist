using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreenControls : MonoBehaviour
{
    public static TitleScreenControls current;
    private PlayerInputs inputs;

    public Action OnCancel;

    private void Awake()
    {
        current = this;
        this.inputs = new PlayerInputs();
    }

    private void Start()
    {
        this.inputs.Controls.Cancel.performed += ctx => DoCancel();
    }

    public void DoCancel() => this.OnCancel?.Invoke();

    private void OnEnable() => this.inputs.Enable();    

    private void OnDisable() => this.inputs.Disable();    
}
