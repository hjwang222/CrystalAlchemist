using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutChange : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gamepadUI;

    [SerializeField]
    private List<GameObject> keyboardUI;

    private void setActive(bool value, List<GameObject> gameObjects)
    {
        foreach (GameObject obje in gameObjects)
        {
            obje.SetActive(value);
        }
    }

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
            setActive(false, this.gamepadUI);
            setActive(true, this.keyboardUI);
        }
        else
        {
            setActive(true, this.gamepadUI);
            setActive(false, this.keyboardUI);
        }
    }
}
