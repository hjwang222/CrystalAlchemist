using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentMenuButtons : MonoBehaviour
{
    public void enableButtons(bool value)
    {
        List<Selectable> selectables = new List<Selectable>();
        UnityUtil.GetChildObjects<Selectable>(this.transform, selectables);

        foreach (Selectable selectable in selectables)
        {
            UnityUtil.SetInteractable(selectable, value);

            if (value)
            {
                ButtonExtension buttonExtension = selectable.GetComponent<ButtonExtension>();
                if (buttonExtension != null)
                {
                    buttonExtension.enabled = value;
                    buttonExtension.SetFirst();
                }
            }
        }
    }
}
