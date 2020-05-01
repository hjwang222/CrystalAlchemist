using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Required]
    [SerializeField]
    [BoxGroup("Player Signals")]
    private SimpleSignal openInventorySignal;

    [Required]
    [SerializeField]
    [BoxGroup("Player Signals")]
    private SimpleSignal openPauseSignal;

    private Player player;

    private void Start()
    {
        this.player = this.GetComponent<Player>();
    }

    public void Inventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (this.player.CanOpenMenu()) this.openInventorySignal.Raise();
            else GameEvents.current.DoInventory();
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (this.player.CanOpenMenu()) this.openPauseSignal.Raise();
            else GameEvents.current.DoPause();
        }
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
