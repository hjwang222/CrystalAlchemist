using UnityEngine;
using Sirenix.OdinInspector;

public class UniversalMenuControls : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Main")]
    private GameObject mainGameObject;

    public void Back(GameObject mainMenu)
    {
        if (this.mainGameObject != null)
        {
            if (this.mainGameObject.GetComponent<SaveGameMenu>() != null) this.mainGameObject.GetComponent<SaveGameMenu>().exitMenu();
            else if (this.mainGameObject.GetComponent<BasicMenu>() != null) this.mainGameObject.GetComponent<BasicMenu>().showMenu(mainMenu);
        }
    }

    public void BackAndSave(GameObject mainMenu)
    {
        if (this.mainGameObject != null)
        {
            if (this.mainGameObject.GetComponent<TitleScreen>() != null)
            {
                this.mainGameObject.GetComponent<TitleScreen>().showMenu(mainMenu);
                this.mainGameObject.GetComponent<TitleScreen>().save();
            }
        }
    }
}
