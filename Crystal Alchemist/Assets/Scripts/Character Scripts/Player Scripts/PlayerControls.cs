using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        this.player = this.GetComponent<Player>();
    }

    public void Inventory(InputAction.CallbackContext context)
    {
        if (context.performed && player.CanOpenMenu()) MenuEvents.current.OpenInventory();        
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && player.CanOpenMenu()) MenuEvents.current.OpenPause();     
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed) GameEvents.current.DoSubmit();
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (context.performed) GameEvents.current.DoCancel();
    }

    public void NextPage(InputAction.CallbackContext context)
    {
        if (context.performed) GameEvents.current.DoPage(1);
    }

    public void PreviousPage(InputAction.CallbackContext context)
    {
        if (context.performed) GameEvents.current.DoPage(-1);
    }
}
