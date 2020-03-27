using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SkillLaser : SkillExtension
{
    #region Attributes

    [InfoBox("Kein Hit-Script notwendig, da kein Collider verwendet wird")]
    [SerializeField]
    private Skill impactEffect;

    [ShowIf("impactEffect")]
    [SerializeField]
    private float distanceBetweenImpacts = 1f;

    [SerializeField]
    [MinValue(0)]
    private float distance = 0;

    [SerializeField]
    private SpriteRenderer laserSprite;

    [SerializeField]
    private bool targetRequired = false;

    private List<Skill> hitPoints = new List<Skill>();

    #endregion


    #region Unity Functions

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
        this.hitPoints.Clear();
    }

    #endregion


    #region Functions (private)

    private void drawLine(bool updateRotation)
    {
        //Bestimme Winkel und Position
        float angle;
        Vector2 startpoint;
        Vector3 rotation;

        RotationUtil.setDirectionAndRotation(this.skill, out angle, out startpoint, out this.skill.direction, out rotation);

        if (this.skill.target != null && updateRotation)
        {
            this.skill.direction = (Vector2)this.skill.target.transform.position - startpoint;
            float temp_angle = Mathf.Atan2(this.skill.direction.y, this.skill.direction.x) * Mathf.Rad2Deg;
            this.skill.direction = RotationUtil.DegreeToVector2(temp_angle);
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

        if (this.targetRequired)
        {
            if (this.skill.target != null) setLaserToTarget(startpoint, rotation, this.skill.target); //laser to target
            else drawLaser(); //no Laser
        }
        else
        {
            RaycastHit2D hitInfo = Physics2D.CircleCast(newPosition, this.laserSprite.size.y / 5, this.skill.direction, distance, layerMask);

            Collider2D hitted = hitInfo.collider;
            Vector2 hitpoint = hitInfo.point;

            if (hitInfo && !hitInfo.collider.isTrigger) drawLaser(startpoint, hitpoint, rotation, hitted); //Laser         
            else drawLaser(startpoint, rotation); //Laser max Length
        }
    }

    private Collider2D getCollider(Character character)
    {
        //Übernehme Position, wenn ein Ziel vorhanden ist
        foreach (Collider2D collider in character.GetComponentsInChildren<Collider2D>(false))
        {
            if (!collider.isTrigger) return collider;
        }

        return null;
    }

    private void setLaserToTarget(Vector2 startpoint, Vector3 rotation, Character character)
    {
        Collider2D collider = getCollider(character);

        if (collider != null)
        {
            Vector2 hitpoint = character.transform.position;
            drawLaser(startpoint, hitpoint, rotation, collider);
        }
    }

    #endregion


    #region Laser und Impact

    private void drawLaser(Vector2 startpoint, Vector2 hitpoint, Vector3 rotation, Collider2D collider)
    {
        if (CollisionUtil.checkCollision(collider, this.skill)) this.skill.hitIt(collider);
        Vector2 position = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y - startpoint.y) / 2) + startpoint;

        this.laserSprite.transform.position = position;
        this.laserSprite.size = new Vector2(Vector3.Distance(hitpoint, startpoint), this.laserSprite.size.y);
        this.laserSprite.transform.rotation = Quaternion.Euler(rotation);

        setImpactEffect(hitpoint);
    }

    private void drawLaser(Vector2 startpoint, Vector3 rotation)
    {
        Vector2 position = new Vector2(this.skill.direction.x * (this.distance / 2), this.skill.direction.y * (this.distance / 2)) + startpoint;

        this.laserSprite.transform.position = position;
        this.laserSprite.size = new Vector2(this.distance, this.laserSprite.size.y);
        this.laserSprite.transform.rotation = Quaternion.Euler(rotation);
    }

    private void drawLaser()
    {
        this.laserSprite.size = new Vector2(0, 0);
    }

    private void setImpactEffect(Vector2 hitpoint)
    {
        if (this.impactEffect != null)
        {
            bool impactPossible = true;
            this.hitPoints.RemoveAll(item => item == null);

            foreach (Skill skill in this.hitPoints)
            {
                if (Vector2.Distance(skill.transform.position, hitpoint) < this.distanceBetweenImpacts) impactPossible = false;
            }

            if (impactPossible)
            {
                Skill hitPointSkill = Instantiate(this.impactEffect, hitpoint, Quaternion.identity);
                hitPointSkill.transform.position = hitpoint;

                if (hitPointSkill != null)
                {
                    //Position nicht überschreiben
                    hitPointSkill.overridePosition = false;
                    hitPointSkill.sender = this.skill.sender;
                }

                this.hitPoints.Add(hitPointSkill);
            }
        }
    }


    #endregion
}
