using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject child;

    [SerializeField]
    List<CurrencySlot> slots = new List<CurrencySlot>();


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
        if(!isRunning() && GameObject.FindWithTag("Player").GetComponent<Player>().currentState != CharacterState.interact)
            StartCoroutine(hideCo(delay));
    }

    private IEnumerator hideCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        child.SetActive(false);
    }
}
