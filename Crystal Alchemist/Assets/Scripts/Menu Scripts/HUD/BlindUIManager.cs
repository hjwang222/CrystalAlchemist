using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlindUIManager : MonoBehaviour
{
    public Canvas canvas;
    public Camera cam; 
   
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas.worldCamera = cam;        
    }

}
