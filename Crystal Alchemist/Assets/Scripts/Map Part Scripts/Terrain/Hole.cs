using System.Collections;
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

        if (character != null) StartCoroutine(animationCo(character));        
    }

    private IEnumerator animationCo(Character character)
    {
        character.transform.DOScale(0, this.duration);
        character.EnableScripts(false);

        yield return new WaitForSeconds(this.duration);

        if (character.GetComponent<Player>() != null)
        {
            character.transform.position = this.position;
            character.transform.DOScale(1, 0f);
            character.EnableScripts(true);
            character.updateResource(cost.resourceType, -cost.amount);
        }
        else
        {
            character.KillIt(false);
        }
    }
}
