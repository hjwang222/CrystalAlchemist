using UnityEngine;
using TMPro;

public class MiniDialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textfield;

    private float duration;

    private Vector2 position;

    private void Start()
    {
        this.transform.position = this.position;
        Destroy(this.gameObject, this.duration);
    }

    public void setDialogBox(string text, float duration, Vector2 position)
    {
        this.duration = duration;
        this.textfield.text = text;
        this.position = position;
    }
}
