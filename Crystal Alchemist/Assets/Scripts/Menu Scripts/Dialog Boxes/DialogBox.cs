using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MenuBehaviour
{
    #region Attribute
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private StringValue dialogText;

    private List<string> texts = new List<string>();
    private int index = 0;
    private int maxLength = 28;
    private float delay = 0.5f;
    #endregion

    public override void Start()
    {
        base.Start();
        showDialogBox(this.dialogText.getValue());
    }

    public void next(int index)
    {
        this.index += index;
        if (this.index < 0) this.index = 0;

        if (this.index < this.texts.Count) showText();
        else hideDialogBox();
    }

    #region Funktionen (Show, Hide, Text)

    public void showDialogBox(string text)
    {
        //Zeige die DialogBox (Signal)
        this.texts.Clear();
        this.texts = formatText(text);
        showText();
    }

    private void hideDialogBox()
    {
        this.index = 0;
        this.ExitMenu();
    }

    private List<string> formatText(string text)
    {
        List<string> result = new List<string>();

        string[] temp = text.Replace("\n", "\n ").Split(' ');
        string line = "";
        int i = 0;

        while (i < temp.Length)
        {
            string word = temp[i];

            if ((line + word).Length > this.maxLength
                || i == temp.Length - 1
                || word.Contains("\n"))
            {
                word = word.Replace("\n", "");
                line += word + " ";
                result.Add(line);
                line = "";
            }
            else if ((line + word).Length <= this.maxLength)
            {
                line += word + " ";
            }
            i++;
        }

        return result;
    }

    private void showText()
    {
        if (this.index + 1 < this.texts.Count) this.textMesh.text = this.texts[this.index] + "\n" + this.texts[this.index + 1];
        else this.textMesh.text = this.texts[this.index];
    }

    #endregion

}
