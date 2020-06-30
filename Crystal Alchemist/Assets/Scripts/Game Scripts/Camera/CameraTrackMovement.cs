﻿using System.Collections;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraTrackMovement : MonoBehaviour
{
    [SerializeField]
    [MinValue(0.05)]
    private float speed = 0.1f;

    [Required]
    [SerializeField]
    [MinValue(0)]
    [MaxValue(99)]
    private float delay = 1;

    [SerializeField]
    private UnityEvent onAfterCameraCompleted;

    private CinemachineVirtualCamera vcam;
    private CinemachineTrackedDolly dolly;

    private void OnEnable()
    {
        this.vcam = this.GetComponent<CinemachineVirtualCamera>();
        this.dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.dolly.m_PathPosition = 0;
    }

    private void LateUpdate()
    {
        this.dolly.m_PathPosition += speed * Time.deltaTime;
        if (this.dolly.m_PathPosition >= 1) StartCoroutine(delayCo());
    }

    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(this.delay);
        onAfterCameraCompleted?.Invoke();
    }
}
