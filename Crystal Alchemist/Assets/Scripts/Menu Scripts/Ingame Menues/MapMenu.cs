using UnityEngine;
using Sirenix.OdinInspector;

public class MapMenu : MenuControls
{
    [BoxGroup("Mandatory")]
    [Required]
    [SerializeField]
    private GameObject pages;

    [BoxGroup("Mandatory")]
    [Required]
    [SerializeField]
    private GameObject locationCursor;

    public override void OnEnable()
    {
        base.OnEnable();
        //setMapAndCursor();
    }

    /*
    private void setMapAndCursor()
    {
        PlayerUtils temp = this.player.GetComponent<PlayerUtils>();

        if (temp != null)
        {
            for (int j = 0; j < this.pages.transform.childCount; j++)
            {
                MapPage page = this.pages.transform.GetChild(j).GetComponent<MapPage>();

                if (page != null)
                {
                    page.gameObject.SetActive(false);

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
                            bool locationFound = false;

                            for (int i = 0; i < page.points.transform.childCount; i++)
                            {
                                MapPagePoint point = page.points.transform.GetChild(i).GetComponent<MapPagePoint>();
                                if (point != null && point.areaID == temp.areaID && this.locationCursor != null)
                                {
                                    //set cursor of current location
                                    this.locationCursor.SetActive(true);
                                    //if (point.GetComponent<ButtonExtension>() != null) point.GetComponent<ButtonExtension>().setFirst();
                                    this.locationCursor.transform.position = new Vector2(point.transform.position.x, point.transform.position.y+(8*point.transform.localScale.y));
                                    locationFound = true;
                                    break;
                                }
                            }

                            if (!locationFound && this.locationCursor != null) this.locationCursor.SetActive(false);
                        }
                    }
                }
            }
        }
    }*/
}
