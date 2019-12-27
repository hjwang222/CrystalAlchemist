using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : MenuControls
{
    [SerializeField]
    private GameObject targetObject;

    public override void OnEnable()
    {
        base.OnEnable();

        //load all maps, set false
        //get map, set true
        //get area, set cursor
    }

    private void setMapAndCursor()
    {
        PlayerUtils temp = this.player.GetComponent<PlayerUtils>();

        if (temp != null)
        {
            foreach (MapPage page in this.player.maps)
            {
                if (page.mapID == temp.mapID)
                {
                    //setActive

                    foreach (MapPagePoint point in page.points)
                    {
                        if (point.areaID == temp.areaID)
                        {
                            if (point.GetComponent<ButtonExtension>() != null) point.GetComponent<ButtonExtension>().setFirst();
                            //set Cursor
                            break;
                        }
                    }

                    break;
                }
            }
        }
    }
}
