using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPage : MonoBehaviour
{
    public string mapID;

    [SerializeField]
    private GameObject map;

    public List<MapPagePoint> points = new List<MapPagePoint>();

    public bool showMap;

    private void OnEnable()
    {
        if (this.showMap)
        {
            this.map.SetActive(true);
        }
        else this.map.SetActive(false);


    }

}
