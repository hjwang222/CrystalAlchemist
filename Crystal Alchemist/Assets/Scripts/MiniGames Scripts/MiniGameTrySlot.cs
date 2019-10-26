using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTrySlot : MonoBehaviour
{
    [SerializeField]
    private GameObject successMark;

    [SerializeField]
    private GameObject failMark;

    [SerializeField]
    private GameObject goldMark;    

    public void setAsNeccessary()
    {
        this.goldMark.SetActive(true);
    }

    public void setMark(bool success)
    {
        if (success) this.successMark.SetActive(true);
        else
        {
            this.goldMark.SetActive(false);
            this.failMark.SetActive(true);
        }
    }
}
