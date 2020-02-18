using UnityEngine.UI;
using UnityEngine;

public class CharacterCreatorButton : MonoBehaviour
{
    public CharacterCreatorMenu mainMenu;

    public virtual void Click()
    {
        this.mainMenu.updatePreview();
    }


}
