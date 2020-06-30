using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class MiniDialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textfield;

    [SerializeField]
    private SpriteRenderer sprite;

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
        SetSize();
    }

    [Button]
    private void SetSize()
    {
        float x = 2.5f;
        float y = 1.5f;
        int length = this.textfield.text.Length;

        int xLength = length;
        if (length > 10) xLength = 10;
        x = (float)(xLength / 2);
        if (x < 2.5f) x = 2.5f;

        int yLength = 1;
        if (length > 10) yLength = 1 + (length / 10);
        y = 1 + (0.5f * yLength);
        if (y < 1.5f) y = 1.5f;

        this.sprite.size = new Vector2(x, y);

        RectTransform rt = (RectTransform)this.textfield.transform;
        rt.sizeDelta = new Vector2(x - 0.5f, y - 0.5f);
    }
}
