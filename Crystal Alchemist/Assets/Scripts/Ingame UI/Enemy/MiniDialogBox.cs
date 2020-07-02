using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class MiniDialogBox : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private TextMeshPro textfield;

    [SerializeField]
    private SpriteRenderer sprite;

    private float duration;
    private Vector2 position;

    private void Start()
    {
        this.transform.position = this.position;
        CheckFlip();
        if(duration > 0) Destroy(this.gameObject, this.duration);
    }

    [Button]
    private void CheckFlip()
    {
        float pos = Camera.main.WorldToScreenPoint(this.transform.position).x;

        if (pos > Screen.currentResolution.width / 2) Flip(true);
        else Flip(false);
    }

    private void Flip(bool value)
    {
        this.sprite.flipX = value;

        float modifier = 1;
        if (this.sprite.flipX) modifier = -1;
        this.parent.transform.localPosition *= new Vector2(modifier, 0);

        RectTransform rt = (RectTransform)this.textfield.transform;
        if (this.sprite.flipX) this.textfield.transform.localPosition = new Vector2(-1*(rt.sizeDelta.x+0.25f), 1);
        else this.textfield.transform.localPosition = new Vector2(0.25f, 1);
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
