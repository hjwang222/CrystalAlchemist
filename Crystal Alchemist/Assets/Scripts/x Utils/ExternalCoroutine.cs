using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalCoroutine : MonoBehaviour
{
    public static ExternalCoroutine instance;

    void Start()
    {
        ExternalCoroutine.instance = this;
    }    
}
