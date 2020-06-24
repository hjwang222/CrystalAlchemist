using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class Room : MonoBehaviour
{
    [BoxGroup("Area")]
    [Required]
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [BoxGroup("Area")]
    [SerializeField]
    private GameObject objectsInArea;

    [BoxGroup("Map")]
    [SerializeField]
    [Required]
    private StringValue stringValue;

    [BoxGroup("Map")]
    [SerializeField]
    private string localisationID;

    private void Awake()
    {
        setObjects(false);
        this.virtualCamera.gameObject.SetActive(false);
    }

    private void Start()
    {
        this.stringValue.SetValue(this.localisationID);
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

            this.stringValue.SetValue(this.localisationID);
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