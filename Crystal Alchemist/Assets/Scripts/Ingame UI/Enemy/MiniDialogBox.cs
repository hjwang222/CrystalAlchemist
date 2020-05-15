using UnityEngine;
using TMPro;

public class MiniDialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textfield;

    private float duration;

    private void Start()
    {
        Destroy(this.gameObject, this.duration);
    }

    public void setDialogBox(string text, float duration)
    {
        this.duration = duration;
        this.textfield.text = text;
    }
}
