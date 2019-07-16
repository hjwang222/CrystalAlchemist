using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutChange : MonoBehaviour
{
    [SerializeField]
    private GameObject gamepadUI;

    [SerializeField]
    private GameObject keyboardUI;

    private void Start()
    {
        updateLayout();
    }

    private void OnEnable()
    {
        updateLayout();
    }

    public void updateLayout()
    {
        if(GlobalValues.layoutType == LayoutType.keyboard)
        {
            this.gamepadUI.SetActive(false);
            this.keyboardUI.SetActive(true);            
        }
        else
        {
            this.gamepadUI.SetActive(true);
            this.keyboardUI.SetActive(false);            
        }
    }
}
