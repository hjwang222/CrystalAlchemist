using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;

public class DialogBox : MenuBehaviour
{
    #region Attribute

    [BoxGroup("DialogBox")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI textfield;

    [BoxGroup("DialogBox")]
    [SerializeField]
    [Required]
    private StringValue dialogText;

    [BoxGroup("DialogBox")]
    [SerializeField]
    [Required]
    private EventValue eventValue; 

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
        if (index >= texts.Count)
        {
            this.eventValue.GetValue()?.Invoke();
            ExitMenu();
        }
        else textfield.text = texts[index];
    }  
}
