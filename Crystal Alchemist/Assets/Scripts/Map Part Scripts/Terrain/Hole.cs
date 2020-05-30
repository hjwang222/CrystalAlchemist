using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Hole : MonoBehaviour
{
    [HideLabel]
    [SerializeField]
    private Costs cost;

    [SerializeField]
    private Vector2 position;

    [SerializeField]
    private float duration = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();

        if (character != null)
        {
            character.updateResource(cost.resourceType, -cost.amount);
            StartCoroutine(animationCo(character));
        }
    }

    private IEnumerator animationCo(Character character)
    {
        character.transform.DOScale(0, this.duration);
        character.EnableScripts(false);

        yield return new WaitForSeconds(this.duration);

        character.transform.position = this.position;
        character.transform.DOScale(1, 0f);
        character.EnableScripts(true);
    }
}
