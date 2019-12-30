using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : MenuControls
{
    [SerializeField]
    private List<MapPage> pages = new List<MapPage>();

    [SerializeField]
    private GameObject locationCursor;

    public override void OnEnable()
    {
        base.OnEnable();
        setMapAndCursor();
    }

    private void setMapAndCursor()
    {
        foreach(MapPage page in this.pages)
        {
            page.gameObject.SetActive(false);
        }

        PlayerUtils temp = this.player.GetComponent<PlayerUtils>();

        if (temp != null)
        {
            foreach(MapPage page in this.pages)
            {
                //set Map visible if Player contains map
                if (CustomUtilities.Items.getMaps(this.player).Contains(page.mapID))
                {
                    page.showMap = true;
                }

                //set Map active of current location of player
                if (page.mapID == temp.mapID)
                {
                    page.gameObject.SetActive(true);

                    if (page.showMap)
                    {
                        foreach (MapPagePoint point in page.points)
                        {
                            if (point.areaID == temp.areaID)
                            {
                                //set cursor of current location
                                if (point.GetComponent<ButtonExtension>() != null) point.GetComponent<ButtonExtension>().setFirst();
                                this.locationCursor.transform.position = point.transform.position;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
