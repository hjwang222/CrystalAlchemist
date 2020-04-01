using UnityEngine;

public class ShowControlMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject padControls;

    [SerializeField]
    private GameObject keyboardControls;

    private void OnEnable()
    {
        if (GlobalGameObjects.settings.layoutType == LayoutType.keyboard) showControlType(false);
        else showControlType(true);
    }


    public void showControlType(bool isGamepad)
    {
        this.padControls.SetActive(isGamepad);
        this.keyboardControls.SetActive(!isGamepad);        
    }
}
