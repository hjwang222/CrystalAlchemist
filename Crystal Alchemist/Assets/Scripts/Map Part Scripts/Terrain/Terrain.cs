using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField]
    private GameObject unityIndicator;

    private void Awake()
    {
        this.unityIndicator.SetActive(false);
    }
}
