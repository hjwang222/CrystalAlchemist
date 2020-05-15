using System.Collections.Generic;
using UnityEngine;

public class QuickTravelMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerTeleportList list;

    [SerializeField]
    private QuickTravelButton template;

    private List<QuickTravelButton> buttons = new List<QuickTravelButton>();

    private void OnEnable()
    {
        this.template.gameObject.SetActive(false);

        for(int i = 0; i < this.buttons.Count; i++)
        {
            Destroy(this.buttons[i].gameObject);
        }

        this.buttons.Clear();

        for(int i = 0; i < list.GetStats().Count; i++)
        {
            QuickTravelButton newButton = Instantiate(template, this.transform);
            newButton.gameObject.SetActive(true);
            newButton.SetLocation(list.GetStats(i));
            this.buttons.Add(newButton);
        }

        this.buttons[0].GetComponent<ButtonExtension>().setFirstSelected = true;
        this.buttons[0].GetComponent<ButtonExtension>().SetFirst();
    }
}
