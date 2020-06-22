using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickTravelMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerTeleportList list;

    [SerializeField]
    private QuickTravelButton template;

    [SerializeField]
    private GameObject content;

    private List<QuickTravelButton> buttons = new List<QuickTravelButton>();

    private void OnEnable()
    {
        //bool setFirst = true;
        this.template.gameObject.SetActive(false);

        for(int i = 0; i < this.buttons.Count; i++)
        {
            Destroy(this.buttons[i].gameObject);
        }

        this.buttons.Clear();

        for(int i = 0; i < list.GetStats().Count; i++)
        {
            TeleportStats stats = list.GetStats(i);
            if (stats.scene == SceneManager.GetActiveScene().name) continue;

            QuickTravelButton newButton = Instantiate(template, this.content.transform);
            newButton.gameObject.SetActive(true);
            newButton.SetLocation(stats);

            /*if (setFirst)
            {
                setFirst = false;
                newButton.GetComponent<ButtonExtension>().setFirstSelected = true;
            }*/

            this.buttons.Add(newButton);            
        }

        //this.buttons[0].GetComponent<ButtonExtension>().setFirstSelected = true;
        //this.buttons[0].GetComponent<ButtonExtension>().SetFirst();
    }
}
