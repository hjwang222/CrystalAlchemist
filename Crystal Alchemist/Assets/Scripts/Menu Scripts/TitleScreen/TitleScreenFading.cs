using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Image))]
public class TitleScreenFading : MonoBehaviour
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private GameObject loading;

    private void Start() => StartCoroutine(fadeCo());   

    private IEnumerator fadeCo()
    {
        this.GetComponent<Image>().DOFade(1, duration);
        yield return new WaitForSeconds(duration);
        loading.SetActive(true);
    }
}
