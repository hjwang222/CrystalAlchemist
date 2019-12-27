using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEditor;

public class Room : MonoBehaviour
{
    [Required]
    [SerializeField]
    private GameObject virtualCamera;

    [SerializeField]
    private GameObject objectsInArea;

    [SerializeField]
    private string mapID;

    [SerializeField]
    private string areaID;

    [SerializeField]
    private string mapName;

    [SerializeField]
    private string mapNameEnglish;

    [SerializeField]
    private StringSignal locationSignal;

    [SerializeField]
    private StringSignal mapLocationSignal;



#if UNITY_EDITOR
    [Button]
    public void setComponent()
    {
        this.mapID = "Overworld";
        this.areaID = "???";
        this.locationSignal = (StringSignal)AssetDatabase.LoadAssetAtPath("Assets/Scriptable Objects/Signals/locationSignal.asset", typeof(StringSignal));
        this.mapLocationSignal = (StringSignal)AssetDatabase.LoadAssetAtPath("Assets/Scriptable Objects/Signals/mapLocationSignal.asset", typeof(StringSignal));
    }
#endif







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
            string text = CustomUtilities.Format.getLanguageDialogText(this.mapName, this.mapNameEnglish);

            setObjects(true);
            this.virtualCamera.SetActive(true);

            this.locationSignal.Raise(text);
            this.mapLocationSignal.Raise(this.mapID + "|" + this.areaID);

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