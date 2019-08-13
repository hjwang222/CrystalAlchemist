using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowControlMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject padControls;

    [SerializeField]
    private GameObject keyboardControls;

    [SerializeField]
    private GameObject parentMenue;

    [SerializeField]
    private Image buttonGamepad;
    [SerializeField]
    private Image buttonKeyboard;

    [SerializeField]
    private TextMeshProUGUI activeGamepad;
    [SerializeField]
    private TextMeshProUGUI activeKeyboard;

    [SerializeField]
    private GameObject childMenue;

    private LayoutType currentType;
    private LayoutType tempType;

    [SerializeField]
    private Color standardColor;

    [SerializeField]
    private Color activeColor;

    [SerializeField]
    private SimpleSignal layoutSignal;

    private void OnEnable()
    {
        this.currentType = GlobalValues.layoutType;
        if (this.currentType == LayoutType.keyboard) showControlType("keyboard");
        else showControlType("gamepad");

        showActive();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) close();
    }

    private void showActive()
    {
        if (this.currentType == LayoutType.keyboard)
        {
            this.buttonGamepad.color = standardColor;
            this.buttonKeyboard.color = activeColor;

            this.activeGamepad.gameObject.SetActive(false);
            this.activeKeyboard.gameObject.SetActive(true);
        }
        else
        {
            this.buttonGamepad.color = activeColor;
            this.buttonKeyboard.color = standardColor;

            this.activeGamepad.gameObject.SetActive(true);
            this.activeKeyboard.gameObject.SetActive(false);
        }
    }

    public void showControlType(string type)
    {
        if(type.ToLower() == "keyboard")
        {
            this.padControls.SetActive(false);
            this.keyboardControls.SetActive(true);

            this.tempType = LayoutType.keyboard;
        }
        else
        {
            this.padControls.SetActive(true);
            this.keyboardControls.SetActive(false);

            this.tempType = LayoutType.gamepad;
        }
    }

    public void close()
    {
        this.childMenue.SetActive(false);
        this.parentMenue.SetActive(true);
    }

    public void save()
    {
        this.currentType = this.tempType;
        GlobalValues.layoutType = this.currentType;

        SaveSystem.SaveOptions();

        showActive();
        this.layoutSignal.Raise();
    }
}
