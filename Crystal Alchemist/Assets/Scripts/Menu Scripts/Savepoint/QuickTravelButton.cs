using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField]
    [Required]
    private Image image;

    private TeleportStats location;

    public void SetLocation(TeleportStats stat)
    {
        this.location = stat;
        this.textField.text = this.location.GetTeleportName();
        this.image.sprite = this.location.icon;
    }

    public void Teleport()
    {
        Cursor.visible = false;
        this.nextTeleport.SetValue(this.location);
        SceneManager.LoadScene(this.nextTeleport.scene);
    }
}
