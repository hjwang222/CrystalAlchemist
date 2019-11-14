using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject child;

    [SerializeField]
    List<CurrencySlot> slots = new List<CurrencySlot>();

    [SerializeField]
    private PlayerStats playerStats;

    private bool isRunning()
    {
        foreach (CurrencySlot slot in this.slots)
        {
            if (slot.gameObject.activeInHierarchy && slot.isItRunning()) return true;
        }

        return false;
    }


    public void ShowUI()
    {
        StopAllCoroutines();
        child.SetActive(true);
    }

    public void HideUI(float delay)
    {
        if(!isRunning() && this.playerStats.player.currentState != CharacterState.interact)
            StartCoroutine(hideCo(delay));
    }

    private IEnumerator hideCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        child.SetActive(false);
    }
}
