using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class BasicMenu : MonoBehaviour
{
    public List<GameObject> menues = new List<GameObject>();

    public virtual void Start()
    {
        showMenu(this.menues[0]);
    }

    public virtual void OnDisable()
    {
        showMenu(this.menues[0]);
    }

    public void showMenu(GameObject newActiveMenu)
    {
        foreach (GameObject gameObject in this.menues)
        {
            gameObject.SetActive(false);
        }

        if (newActiveMenu != null)
        {
            newActiveMenu.SetActive(true);

            for (int i = 0; i < newActiveMenu.transform.childCount; i++)
            {
                ButtonExtension temp = newActiveMenu.transform.GetChild(i).GetComponent<ButtonExtension>();
                if (temp != null && temp.setFirstSelected)
                {
                    temp.setFirst();
                    break;
                }
            }
        }
    }
}
