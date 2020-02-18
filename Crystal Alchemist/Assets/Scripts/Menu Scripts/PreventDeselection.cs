using UnityEngine;
using UnityEngine.EventSystems;

public class PreventDeselection : MonoBehaviour
{
    private GameObject sel;

    public virtual void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject != sel)
            sel = EventSystem.current.currentSelectedGameObject;
        else if (sel != null && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(sel);
    }
}
