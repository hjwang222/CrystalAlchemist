using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class QuickTravelButton : MonoBehaviour
{
    [SerializeField]
    [Required]
    private TeleportStats nextTeleport;

    [SerializeField]
    [Required]
    private TextMeshProUGUI textField;

    private TeleportStats location;

    public void SetLocation(TeleportStats stat)
    {
        this.location = stat;
        this.textField.text = this.location.teleportName;
    }

    public void Teleport()
    {
        Cursor.visible = false;
        this.nextTeleport.SetValue(this.location);
        SceneManager.LoadScene(this.nextTeleport.scene);
    }
}
