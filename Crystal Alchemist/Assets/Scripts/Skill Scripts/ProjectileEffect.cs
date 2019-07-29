using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ProjectileEffect : MonoBehaviour
{
    [SerializeField]
    [Required]
    private GameObject ghost;

    [SerializeField]
    [Required]
    private StandardSkill skill;

    [SerializeField]
    private float delay;

    [SerializeField]
    private float destroyAfter;

    private float ghostDelay;

    [SerializeField]
    private bool useCharacterSprite;

    private void Start()
    {
        if(this.skill == null) this.skill = this.GetComponent<StandardSkill>();
    }

    private void Update()
    {
        if (this.skill != null && this.skill.delayTimeLeft <= 0)
        {
            if (ghostDelay > 0)
            {
                this.ghostDelay -= (Time.deltaTime * this.skill.timeDistortion);
            }
            else
            {
                GameObject currentGhost = Instantiate(this.ghost, this.skill.transform.position, skill.transform.rotation);

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
}
