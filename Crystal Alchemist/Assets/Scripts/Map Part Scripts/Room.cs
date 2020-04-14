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

    [BoxGroup("Map")]
    [SerializeField]
    private string mapID;

    [BoxGroup("Map")]
    [SerializeField]
    private string areaID;

    [BoxGroup("Map")]
    [SerializeField]
    private string mapName;

    [BoxGroup("Map")]
    [SerializeField]
    private string mapNameEnglish;

    [BoxGroup("Map")]
    [SerializeField]
    private StringSignal locationSignal;


#if UNITY_EDITOR
    [Button]
    public void setComponent()
    {
        this.mapID = "Overworld";
        this.areaID = "???";
        this.locationSignal = (StringSignal)AssetDatabase.LoadAssetAtPath("Assets/Resources/Scriptable Objects/Signals/Game Signals/locationSignal.asset", typeof(StringSignal));
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
        if (!other.isTrigger)
        {
            string text = FormatUtil.getLanguageDialogText(this.mapName, this.mapNameEnglish);

            setObjects(true);
            this.virtualCamera.SetActive(true);

            this.locationSignal.Raise(text);
            CinemachineVirtualCamera vcam = this.virtualCamera.GetComponent<CinemachineVirtualCamera>();
            if (vcam.Follow == null) vcam.Follow = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            setObjects(false);
            this.virtualCamera.SetActive(false);
        }
    }

    public void setCameraPosition(bool setNull)
    {
        if (setNull)
        {
            this.virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = null;
        }
    }
}