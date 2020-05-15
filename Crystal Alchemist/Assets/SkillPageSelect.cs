using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillPageSelect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    [SerializeField]
    private List<GameObject> pages = new List<GameObject>();

    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private GameObject previousButton;

    private int index;

    public void Initialize()
    {
        UpdateButtons();
        for (int i = 1; i < this.pages.Count; i++) this.pages[i].SetActive(false);
        GameEvents.current.OnPage += SetNextPage;
    }

    private void OnDestroy() => GameEvents.current.OnPage -= SetNextPage;

    private void UpdateButtons()
    {
        if (this.index == 0) this.previousButton.SetActive(false);
        else this.previousButton.SetActive(true);

        if (this.index == this.pages.Count - 1) this.nextButton.SetActive(false);
        else this.nextButton.SetActive(true);

        this.textField.text = (this.index + 1) + "/" + this.pages.Count;
    }

    public void SetNextPage(int value)
    {
        for(int i = 0; i < this.pages.Count; i++)
        {
            if (pages[i].activeInHierarchy) this.index = i;
        }

        pages[this.index].SetActive(false);
        pages[this.index + value].SetActive(true);

        UpdateButtons();
    }
}

