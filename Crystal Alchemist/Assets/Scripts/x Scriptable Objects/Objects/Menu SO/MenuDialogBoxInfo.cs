using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game/Menu/Menu Dialogbox Info")]
public class MenuDialogBoxInfo : ScriptableObject
{
    public UnityEvent OnConfirm;
    public Costs costs;
    public string text;
    public DialogBoxType type;
    public GameObject parent;

    public void SetValue(UnityEvent OnConfirm, Costs costs, string text, DialogBoxType type, GameObject parent)
    {
        this.OnConfirm = OnConfirm;
        this.costs = costs;
        this.text = text;
        this.type = type;
        this.parent = parent;
    }
}
