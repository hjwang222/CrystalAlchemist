using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DialogBox : MenuBehaviour
{
    #region Attribute
    [SerializeField]
    private TextMeshProUGUI textfield;

    [SerializeField]
    private StringValue dialogText;

    private List<string> texts = new List<string>();
    private int index = 0;
    #endregion

    public override void Start()
    {
        base.Start();
        texts = dialogText.GetValue().Split('\n').ToList();
        ShowNextDialog(0);
    }

    public void ShowNextDialog(int value)
    {
        index += value;
        if (index >= texts.Count) ExitMenu();
        else textfield.text = texts[index];
    }  
}
