using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AddSpawn : MonoBehaviour
{
    [SerializeField]
    private AI character;

    [SerializeField]
    private float delay;

    [SerializeField]
    private UnityEvent OnAfterDelay;

    private Character target;

    public void Initialize(Character target) => this.target = target;

    private void Start() => StartCoroutine(delayCo());    

    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(this.delay);
        AI character = Instantiate(this.character, this.transform.position, Quaternion.identity);
        character.InitializeAddSpawn(this.target);
        this.OnAfterDelay?.Invoke();
        Destroy(this.gameObject, 0.3f);
    }
}
