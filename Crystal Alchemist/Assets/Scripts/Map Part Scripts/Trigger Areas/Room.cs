using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class Room : MonoBehaviour
{
    [Required]
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private GameObject objectsInArea;

    [BoxGroup("Map")]
    [SerializeField]
    private StringSignal locationSignal;

    [BoxGroup("Map")]
    [SerializeField]
    private string localisationID;

    private string text;

    private void Awake()
    {
        setObjects(false);
        this.virtualCamera.gameObject.SetActive(false);
    }

    private void Start()
    {        
        this.text = FormatUtil.GetLocalisedText(this.localisationID, LocalisationFileType.maps);
        SettingsEvents.current.OnLanguangeChanged += UpdateText;
    }

    private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= UpdateText;    

    private void UpdateText()
    {
        this.text = FormatUtil.GetLocalisedText(this.localisationID, LocalisationFileType.maps);
        this.locationSignal.Raise(this.text);
    }

    private void setObjects(bool value)
    {
        if (this.objectsInArea != null) this.objectsInArea.SetActive(value);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            setObjects(true);
            this.virtualCamera.gameObject.SetActive(true);

            this.locationSignal.Raise(this.text);
            if (this.virtualCamera.Follow == null) this.virtualCamera.Follow = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            setObjects(false);
            this.virtualCamera.gameObject.SetActive(false);
        }
    }
}