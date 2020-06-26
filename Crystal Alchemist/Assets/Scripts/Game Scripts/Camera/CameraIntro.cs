using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraIntro : MonoBehaviour
{
    private enum Mode
    {
        always,
        session,
        oneTime
    }

    [Required]
    [SerializeField]
    private CinemachineVirtualCamera introCamera;

    [Required]
    [SerializeField]
    private BoolValue CutSceneValue;

    [Required]
    [SerializeField]
    private CinemachineVirtualCamera mainCam;

    [SerializeField]
    [MinValue(0.05)]
    private float speed = 0.1f;

    [Required]
    [SerializeField]
    [MinValue(0)]
    [MaxValue(99)]
    private float delay = 1;

    [SerializeField]
    private Mode mode;

    [HideIf("mode", Mode.always)]
    [Required]
    [SerializeField]
    private string gameProgressID;

    [HideIf("mode", Mode.always)]
    [Required]
    [SerializeField]
    private PlayerGameProgress playerProgress;

    [SerializeField]
    private UnityEvent onTrigger;

    private float position;
    private bool isRunning;
    private bool isInit = true;
    private CinemachineTrackedDolly dolly;
    private bool isPermanent = false;

    private void Start() => CanPlay();

    private void CanPlay()
    {
        if (this.mode == Mode.oneTime) isPermanent = true;
        if (this.mode != Mode.always && this.playerProgress.Contains(this.gameProgressID, this.isPermanent)) this.gameObject.SetActive(false);        
        
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
        this.mainCam.gameObject.SetActive(true);

        this.CutSceneValue.setValue(false);
        GameEvents.current.DoCutScene();

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isInit) return;

        this.mainCam.gameObject.SetActive(false);

        this.CutSceneValue.setValue(true);
        GameEvents.current.DoCutScene();

        this.onTrigger?.Invoke();
        this.dolly = introCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.isRunning = true;

        this.playerProgress.AddProgress(this.gameProgressID, this.isPermanent);
    }
}
