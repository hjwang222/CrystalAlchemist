using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroClue : MonoBehaviour
{
    [SerializeField]
    private float duration;

    private void Start() => Destroy(this.gameObject, duration);

    public void Hide() => Destroy(this.gameObject);
}
