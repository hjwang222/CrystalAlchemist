using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControlMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject padControls;

    [SerializeField]
    private GameObject keyboardControls;

    [SerializeField]
    private GameObject parentMenue;

    [SerializeField]
    private GameObject childMenue;

    private void OnEnable()
    {
        showControlType("");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) close();
    }

    public void showControlType(string type)
    {
        if(type == "keyboard")
        {
            this.padControls.SetActive(false);
            this.keyboardControls.SetActive(true);
        }
        else
        {
            this.padControls.SetActive(true);
            this.keyboardControls.SetActive(false);
        }
    }

    public void close()
    {
        this.childMenue.SetActive(false);
        this.parentMenue.SetActive(true);
    }
}
