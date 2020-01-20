using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public enum VolumeType
{
    music,
    effects
}

public class TitleScreenMenues : MonoBehaviour
{
    [BoxGroup("Switch Buttons")]
    public Image firstButton;
    [BoxGroup("Switch Buttons")]
    public Image secondButton;
    [BoxGroup("Switch Buttons")]
    public SimpleSignal switchSignal;

    [BoxGroup("Volume Buttons")]
    public TextMeshProUGUI textField;
    [BoxGroup("Volume Buttons")]
    public Slider slider;
    [BoxGroup("Volume Buttons")]
    public VolumeType volumeType;


    public void switchButtons(Image active, Image notActive)
    {
        active.color = setAlpha(active, 1f);
        notActive.color = setAlpha(notActive, 0.2f);
    }

    private Color setAlpha(Image image, float transparency)
    {
        return new Color(image.color.r, image.color.g, image.color.b, transparency);
    }
}
