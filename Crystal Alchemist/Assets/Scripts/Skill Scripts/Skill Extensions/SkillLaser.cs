using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillLaser : SkillExtension
{
    #region Attributes

    [InfoBox("Kein Hit-Script notwendig, da kein Collider verwendet wird")]
    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    [Range(0, CustomUtilities.maxFloatSmall)]
    private float distance = 0;

    [SerializeField]
    private SpriteRenderer laserSprite;

    private GameObject fire;
    private bool placeFire = true;



    #endregion

    private void Start()
    {
        drawLine(true);
    }

    private void Update()
    {
        drawLine(this.skill.rotateIt());
    }
    private void OnDestroy()
    {
        this.placeFire = false;
        this.fire = null;
    }


    #region Functions (private)

    private void drawLine(bool updateRotation)
    {
        //Bestimme Winkel und Position

        float angle;
        Vector2 startpoint;
        Vector3 rotation;

        CustomUtilities.Rotation.setDirectionAndRotation(this.skill, out angle, out startpoint, out this.skill.direction, out rotation);

        if (this.skill.target != null && updateRotation)
        {
            this.skill.direction = (Vector2)this.skill.target.transform.position - startpoint;
            float temp_angle = Mathf.Atan2(this.skill.direction.y, this.skill.direction.x) * Mathf.Rad2Deg;
            this.skill.direction = CustomUtilities.Rotation.DegreeToVector2(temp_angle);
        }

        renderLine(startpoint, rotation);
    }

    private void renderLine(Vector2 startpoint, Vector3 rotation)
    {
        //No hit on itself
        int layerMask = 1 << this.skill.sender.gameObject.layer;
        layerMask = ~layerMask;

        float offset = 1f;
        Vector2 newPosition = new Vector2(startpoint.x - (this.skill.direction.x * offset), startpoint.y - (this.skill.direction.y * offset));

        RaycastHit2D hitInfo = Physics2D.CircleCast(newPosition, this.laserSprite.size.y / 5, this.skill.direction, distance, layerMask);

        SkillTargetingSystemModule targetingSystemModule = this.GetComponent<SkillTargetingSystemModule>();

        if ((targetingSystemModule != null 
            //&& targetingSystemModule.lockOnGameObject != null 
            && this.skill.target != null) 
            || ((targetingSystemModule == null 
            || (targetingSystemModule != null 
            //&& targetingSystemModule.lockOnGameObject == null)
            ) 
            && hitInfo && !hitInfo.collider.isTrigger)))
        {
            //Ziel bzw. Collider wurde getroffen, zeichne Linie bis zum Ziel
            Collider2D hitted = hitInfo.collider;
            Vector2 hitpoint = hitInfo.point;

            if (this.skill.target != null)
            {
                //Übernehme Position, wenn ein Ziel vorhanden ist
                foreach(Collider2D collider in this.skill.target.GetComponentsInChildren<Collider2D>(false))
                {
                    if (!collider.isTrigger) hitted = collider;
                }

                hitpoint = this.skill.target.transform.position;
            }

            if (CustomUtilities.Collisions.checkCollision(hitted, this.skill)) this.skill.hitIt(hitted);

            Vector2 temp = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y - startpoint.y) / 2) + startpoint;

            this.laserSprite.transform.position = temp;
            this.laserSprite.size = new Vector2(Vector3.Distance(hitpoint, startpoint), this.laserSprite.size.y);
            this.laserSprite.transform.rotation = Quaternion.Euler(rotation);

            if (this.impactEffect != null)
            {
                //Generiere einen Treffer

                if (this.fire == null && this.placeFire)
                {
                    this.fire = Instantiate(this.impactEffect, hitpoint, Quaternion.identity);
                    this.fire.transform.position = hitpoint;
                    Skill fireSkill = this.fire.GetComponent<Skill>();

                    if (fireSkill != null)
                    {
                        //Position nicht überschreiben
                        fireSkill.overridePosition = false;
                        fireSkill.sender = this.skill.sender;
                    }

                    fire.hideFlags = HideFlags.HideInHierarchy;
                }
                else if (this.fire != null && this.placeFire)
                {
                    this.fire.transform.position = hitpoint;
                }
            }
        }        
        else
        {
            if (targetingSystemModule == null 
                || (targetingSystemModule != null 
                //&& targetingSystemModule.lockOnGameObject == null
                ))
            {
                //Kein Ziel getroffen, zeichne Linie mit max Länge            
                Vector2 temp = new Vector2(this.skill.direction.x * (this.distance / 2), this.skill.direction.y * (this.distance / 2)) + startpoint;

                this.laserSprite.transform.position = temp;
                this.laserSprite.size = new Vector2(this.distance, this.laserSprite.size.y);
                this.laserSprite.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                //Wenn ein Lockon vorhanden aber kein Ziel existiert, kein Laser!
                this.laserSprite.size = new Vector2(0, 0);
            }
        }
    }

    #endregion
}
