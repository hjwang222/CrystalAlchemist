using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject ghost;

    [SerializeField]
    private StandardSkill skill;

    [SerializeField]
    private float delay;

    [SerializeField]
    private float destroyAfter;

    private float ghostDelay;

    [SerializeField]
    private bool useCharacterSprite;

    private void Update()
    {
        if (ghostDelay > 0)
        {
            this.ghostDelay -= (Time.deltaTime * this.skill.timeDistortion);
        }
        else
        {
            GameObject currentGhost = Instantiate(this.ghost, skill.transform.position, skill.transform.rotation);

            if (this.useCharacterSprite)
            {
                Sprite currentSprite = this.skill.sender.GetComponent<SpriteRenderer>().sprite;
                //currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
            }
            this.ghostDelay = this.delay;

            Destroy(currentGhost, this.destroyAfter);
        }
    }
}
