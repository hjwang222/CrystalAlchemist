using UnityEngine;
using TMPro;

public class MiniDialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textfield;

    private float height;
    private float duration;

    private void Start()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + height);
        Destroy(this.gameObject, this.duration);
    }

    public void setDialogBox(string text, float duration, float height)
    {
        this.duration = duration;
        this.textfield.text = text;
        this.height = height;
    }
}
