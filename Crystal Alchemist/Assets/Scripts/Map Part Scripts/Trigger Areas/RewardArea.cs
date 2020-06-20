using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class RewardArea : MonoBehaviour
{
    [SerializeField]
    private bool collectAll;

    [SerializeField]
    [MinValue(1)]
    [HideIf("collectAll")]
    private int maxAmount = 1;

    private int counter;

    private void Start() => GameEvents.current.OnCollect += DestroyItems;

    private void OnDestroy() => GameEvents.current.OnCollect -= DestroyItems;

    private void DestroyItems(ItemStats stats)
    {
        if (this.collectAll) return;
        counter++;
        if (counter >= this.maxAmount) StartCoroutine(disableCo());
    }

    private IEnumerator disableCo()
    {
        yield return new WaitForEndOfFrame();
        this.gameObject.SetActive(false);
    }
}
