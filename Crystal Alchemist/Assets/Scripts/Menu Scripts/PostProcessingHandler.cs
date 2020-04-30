using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using System;

public class PostProcessingHandler : MonoBehaviour
{
    [BoxGroup]
    [SerializeField]
    private Volume volume;

    [BoxGroup]
    [SerializeField]
    private float fadingSteps = 0.025f;

    private ColorAdjustments colorGrading;

    public void StartFading(Action action)
    {
        if (this.volume.profile.TryGet(out this.colorGrading)) StartCoroutine(FadeOut(this.fadingSteps, action));
    }

    private IEnumerator FadeOut(float delay, Action action)
    {
        while (this.colorGrading.saturation.value > -100)
        {
            this.colorGrading.saturation.value -= 1f;
            if (this.colorGrading.saturation.value <= -100)
            {
                action.Invoke();
                break;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void ResetFading()
    {
        this.colorGrading.saturation.value = 0;
        this.colorGrading.colorFilter.value = Color.white;
    }
}
