using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GlobalGameObjects globalGameObjects;

    [SerializeField]
    private PlayerInventory playerInventory;

    private void Awake()
    {
        this.globalGameObjects.Initialize(); //set objects to static
        this.playerInventory.Initialize(); //remove null objects
    }
}
