using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SkillLaser : SkillExtension
{
    #region Attributes

    [InfoBox("Kein Hit-Script notwendig, da kein Collider verwendet wird")]
    [SerializeField]
    private Ability impactEffect;

    [ShowIf("impactEffect")]
    [SerializeField]
    private float distanceBetweenImpacts = 1f;

    [SerializeField]
    private Light2D lights;

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
        this.laserSprite.enabled = false;
    }

    private void Update()
    {
        drawLine();
    }

    private void OnDestroy()
    {
        this.hitPoints.Clear();
    }

    #endregion


    #region Functions (private)
    private void drawLine()
    {
        Collider2D hitted = null;
        Vector2 hitPoint = Vector2.zero;

        Vector2 startposition = this.skill.sender.GetShootingPosition();

        if (this.skill.standAlone)
        {
            startposition = this.transform.position;
        }
        else
        {
            this.skill.direction = this.skill.sender.values.direction;
        }

        if (targetRequired && this.skill.target == null) LineRenderUtil.Renderempty(this.laserSprite);
        else LineRenderUtil.RenderLine(this.skill.sender, this.skill.target, this.skill.direction, this.distance, this.laserSprite, startposition, out hitted, out hitPoint);

        if (hitted != null)
        {
            if(CollisionUtil.checkCollision(hitted, this.skill)) this.skill.hitIt(hitted);
            setImpactEffect(hitPoint);
        }
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
                Skill hitPointSkill = this.impactEffect.InstantiateSkill(hitpoint);
                hitPointSkill.transform.position = hitpoint;

                if (hitPointSkill != null) hitPointSkill.sender = this.skill.sender;                

                this.hitPoints.Add(hitPointSkill);
            }
        }
    }
    #endregion
}
