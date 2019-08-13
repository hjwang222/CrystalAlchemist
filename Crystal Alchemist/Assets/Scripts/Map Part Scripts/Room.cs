using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class Room : MonoBehaviour
{
    [Required]
    [SerializeField]
    private GameObject virtualCamera;

    [SerializeField]
    private GameObject objectsInArea;

    [SerializeField]
    private string mapName;

    [SerializeField]
    private string mapNameEnglish;

    [SerializeField]
    private StringSignal locationSignal;

    private void Awake()
    {
        setObjects(false);
        this.virtualCamera.SetActive(false);
    }

    private void setObjects(bool value)
    {
        if (this.objectsInArea != null) this.objectsInArea.SetActive(value);
    }

    // OnTriggerEnter2D is a built in Unity function
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string text = Utilities.Format.getLanguageDialogText(this.mapName, this.mapNameEnglish);

            setObjects(true);
            this.virtualCamera.SetActive(true);

            this.locationSignal.Raise(text);
            CinemachineVirtualCamera vcam = this.virtualCamera.GetComponent<CinemachineVirtualCamera>();
            if(vcam.Follow == null) vcam.Follow = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            setObjects(false);
            this.virtualCamera.SetActive(false);
        }
    }

    /*public void setFollowAtNull()
    {
        this.virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = null;
    }    */

    public void setCameraPosition(bool setNull)
    {
        if (setNull)
        {
            this.virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = null;
        }
    }
}