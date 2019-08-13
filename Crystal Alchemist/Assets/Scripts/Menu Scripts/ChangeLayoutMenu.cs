using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLayoutMenu : MonoBehaviour
{
    [SerializeField]
    private Image buttonGamepad;

    [SerializeField]
    private Image buttonKeyboard;

    [SerializeField]
    private SimpleSignal layoutSignal;

    private void OnEnable()
    {
        getLayout();
    }

    private void getLayout()
    {
        if (GlobalValues.layoutType == LayoutType.keyboard)
        {
            this.buttonKeyboard.gameObject.SetActive(true);
            this.buttonGamepad.gameObject.SetActive(false);
        }
        else
        {
            this.buttonKeyboard.gameObject.SetActive(false);
            this.buttonGamepad.gameObject.SetActive(true);
        }
    }

    public void changeLayout()
    {
        if (GlobalValues.layoutType == LayoutType.keyboard) GlobalValues.layoutType = LayoutType.gamepad;
        else GlobalValues.layoutType = LayoutType.keyboard;
        
        getLayout();

        this.layoutSignal.Raise();
    }
}
