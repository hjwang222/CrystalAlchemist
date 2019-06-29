using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class Room : MonoBehaviour
{
    [Required]
    public GameObject virtualCamera;

    private void Awake()
    {
        this.virtualCamera.SetActive(false);
    }

    // OnTriggerEnter2D is a built in Unity function
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            this.virtualCamera.SetActive(true);

            CinemachineVirtualCamera vcam = this.virtualCamera.GetComponent<CinemachineVirtualCamera>();
            if(vcam.Follow == null) vcam.Follow = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            this.virtualCamera.SetActive(false);
        }
    }

    public void setFollowAtNull()
    {
        this.virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = null;
    }    
}