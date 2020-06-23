using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraIntro : MonoBehaviour
{
    [Required]
    [SerializeField]
    private CinemachineVirtualCamera introCamera;

    [Required]
    [SerializeField]
    private Room room;

    [SerializeField]
    [MinValue(0.05)]
    private float speed = 0.1f;

    [Required]
    [SerializeField]
    [MinValue(0)]
    [MaxValue(99)]
    private float delay = 1;

    [SerializeField]
    private bool uniqueEvent = true;

    [ShowIf("uniqueEvent")]
    [Required]
    [SerializeField]
    private string gameProgressID;

    [ShowIf("uniqueEvent")]
    [Required]
    [SerializeField]
    private PlayerGameProgress playerProgress;

    [SerializeField]
    private UnityEvent onTrigger;

    private float position;
    private bool isRunning;
    private bool isInit = true;
    private CinemachineTrackedDolly dolly;

    private void Start() => CanPlay();

    private void CanPlay()
    {
        if (this.uniqueEvent && this.playerProgress.Contains(this.gameProgressID)) this.gameObject.SetActive(false);        
        else this.playerProgress.Add(this.gameProgressID);
        
        this.isInit = false;
    }

    private void LateUpdate()
    {
        if (!isRunning || this.dolly == null) return;

        this.position += speed * Time.deltaTime;
        this.dolly.m_PathPosition = position;
        if (this.position >= 1) StartCoroutine(delayCo());
    }

    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(this.delay);
        this.room.gameObject.SetActive(true);
        GameEvents.current.DoCutScene(false);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isInit) return;

        this.room.gameObject.SetActive(false);
        GameEvents.current.DoCutScene(true);
        this.onTrigger?.Invoke();
        this.dolly = introCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.isRunning = true;
    }
}
