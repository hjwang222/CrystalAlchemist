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

    [BoxGroup("Map")]
    [SerializeField]
    private StringSignal mapLocationSignal;


#if UNITY_EDITOR
    [Button]
    public void setComponent()
    {
        this.mapID = "Overworld";
        this.areaID = "???";
        this.locationSignal = (StringSignal)AssetDatabase.LoadAssetAtPath("Assets/Resources/Scriptable Objects/Signals/locationSignal.asset", typeof(StringSignal));
        this.mapLocationSignal = (StringSignal)AssetDatabase.LoadAssetAtPath("Assets/Resources/Scriptable Objects/Signals/mapLocationSignal.asset", typeof(StringSignal));
    }

    /*
    [Button]
    public void setAudio()
    {        
        this.audioClipSignalStart = (AudioClipSignal)AssetDatabase.LoadAssetAtPath("Assets/Scriptable Objects/Signals/BGMClipStartSignal.asset", typeof(AudioClipSignal));
        this.audioClipSignalLoop = (AudioClipSignal)AssetDatabase.LoadAssetAtPath("Assets/Scriptable Objects/Signals/BGMClipLoopSignal.asset", typeof(AudioClipSignal));
        this.startMusicSignal = (SimpleSignal)AssetDatabase.LoadAssetAtPath("Assets/Scriptable Objects/Signals/startBackgroundMusic.asset", typeof(SimpleSignal));
    }*/
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
            /*if (this.audioClipSignalStart != null) this.audioClipSignalStart.Raise(this.musicStart);
            if (this.audioClipSignalLoop != null) this.audioClipSignalLoop.Raise(this.musicLoop);
            if (this.startMusicSignal != null) this.startMusicSignal.Raise();*/

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