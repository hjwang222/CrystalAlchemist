using UnityEngine;
using Sirenix.OdinInspector;

public class UniversalMenuControls : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Main")]
    private BasicMenu mainGameObject;

    public void Back(GameObject mainMenu)
    {
        if (this.mainGameObject != null)
        {
            if (this.mainGameObject.GetComponent<SaveGameMenu>() != null)
                this.mainGameObject.GetComponent<SaveGameMenu>().exitMenu();
            else if (this.mainGameObject.GetComponent<BasicMenu>() != null)
                CustomUtilities.UI.ShowMenu(mainMenu, this.mainGameObject.GetComponent<BasicMenu>().menues);
        }
    }

    public void BackAndSave(GameObject mainMenu)
    {
        if (this.mainGameObject != null)
        {
            CustomUtilities.UI.ShowMenu(mainMenu, this.mainGameObject.GetComponent<BasicMenu>().menues);

            if (this.mainGameObject.GetComponent<BasicMenu>() != null)
                this.mainGameObject.GetComponent<BasicMenu>().save();            
        }
    }
}
