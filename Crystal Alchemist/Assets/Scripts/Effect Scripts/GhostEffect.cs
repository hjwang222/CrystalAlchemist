using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GhostEffect : MonoBehaviour
{
    [SerializeField]
    [Required]
    private GameObject ghost;

    [SerializeField]
    [Required]
    private GameObject mainObject;

    [SerializeField]
    private float delay;

    [SerializeField]
    private float destroyAfter;

    private float ghostDelay;

    [SerializeField]
    private bool useCharacterSprite;


    private void Update()
    {
        if (this.mainObject != null)
        {
            float distortion = 1;

            if (this.mainObject.GetComponent<Skill>() != null) distortion = this.mainObject.GetComponent<Skill>().getTimeDistortion();
            else if (this.mainObject.GetComponent<Character>() != null) distortion = this.mainObject.GetComponent<Character>().values.timeDistortion;

            if (ghostDelay > 0)
            {
                this.ghostDelay -= (Time.deltaTime * distortion);
            }
            else
            {
                GameObject currentGhost = Instantiate(this.ghost, this.mainObject.transform.position, this.mainObject.transform.rotation);

                if (this.useCharacterSprite)
                {
                    if (this.mainObject.GetComponent<Skill>() != null)
                        currentGhost.GetComponent<SpriteRenderer>().sprite = this.mainObject.GetComponent<Skill>().GetComponent<SpriteRenderer>().sprite;
                    else if (this.mainObject.GetComponent<Character>() != null)
                        currentGhost.GetComponent<SpriteRenderer>().sprite = this.mainObject.GetComponent<Character>().GetComponent<SpriteRenderer>().sprite;
                }
                this.ghostDelay = this.delay;

                Destroy(currentGhost, this.destroyAfter);
            }
        }
    }
}
