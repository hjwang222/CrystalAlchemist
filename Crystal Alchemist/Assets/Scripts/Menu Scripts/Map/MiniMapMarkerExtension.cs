using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapMarkerExtension : MonoBehaviour
{
    [SerializeField]
    private GameObject marker;

    private void Start()
    {
        if (this.marker != null) Instantiate(this.marker, this.transform);
    }
}
