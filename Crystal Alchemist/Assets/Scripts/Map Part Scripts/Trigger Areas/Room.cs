using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEditor;

public class Room : MonoBehaviour
{
    [Required]
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private GameObject objectsInArea;

    [BoxGroup("Map")]
    [SerializeField]
    [HideLabel]
    private LocalisationValue localisation; 

    [BoxGroup("Map")]
    [SerializeField]
    private StringSignal locationSignal;

    private void Awake()
    {
        setObjects(false);
        this.virtualCamera.gameObject.SetActive(false);
    }

    private void setObjects(bool value)
    {
        if (this.objectsInArea != null) this.objectsInArea.SetActive(value);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            string text = FormatUtil.getLanguageDialogText(this.localisation);

            setObjects(true);
            this.virtualCamera.gameObject.SetActive(true);

            this.locationSignal.Raise(text);
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