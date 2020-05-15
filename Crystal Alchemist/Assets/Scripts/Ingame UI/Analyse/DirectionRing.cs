using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionRing : MonoBehaviour
{
    [SerializeField]
    private Character character;

    // Update is called once per frame
    void Update()
    {
        float angle = (Mathf.Atan2(this.character.values.direction.y, this.character.values.direction.x) * Mathf.Rad2Deg)+90;
        Vector3 rotation = new Vector3(0, 0, angle);

        this.transform.rotation = Quaternion.Euler(rotation);
    }
}
