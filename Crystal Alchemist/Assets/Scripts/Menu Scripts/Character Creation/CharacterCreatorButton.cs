using UnityEngine.UI;
using UnityEngine;

public class CharacterCreatorButton : MonoBehaviour
{
    public CharacterCreatorMenu mainMenu;

    [SerializeField]
    private SimpleSignal buttonNavigationSignal;

    public virtual void Click()
    {
        if(this.buttonNavigationSignal != null) this.buttonNavigationSignal.Raise();
        this.mainMenu.updatePreview();
    }


}
