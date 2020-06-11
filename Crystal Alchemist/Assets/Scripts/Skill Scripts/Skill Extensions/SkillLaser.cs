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

    private Vector2 position;

    #endregion


    #region Unity Functions

    public override void Initialize() => this.laserSprite.enabled = false;    

    public override void Updating() => drawLine();
    
    private void OnDestroy() => this.hitPoints.Clear();    

    #endregion

    #region Functions (private)
    private void drawLine()
    {
        Collider2D hitted = null;
        Vector2 hitPoint = Vector2.zero;

        if (this.skill.standAlone)
        {
            this.position = this.transform.position;
            this.skill.SetDirection(RotationUtil.DegreeToVector2(this.transform.rotation.eulerAngles.z));
        }
        else
        {
            this.skill.SetDirection(this.skill.sender.values.direction);
            this.skill.SetVectors();

            this.position = this.transform.position;
        }

        if (targetRequired && this.skill.target == null) LineRenderUtil.Renderempty(this.laserSprite);
        else LineRenderUtil.RenderLine(this.skill.sender, this.skill.target, this.skill.GetDirection(), this.distance, this.laserSprite, this.position, out hitted, out hitPoint);

        if (hitted != null && this.skill.GetTriggerActive())
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
                Skill hitPointSkill = this.impactEffect.InstantiateSkill(hitpoint, this.skill.sender); 
                this.hitPoints.Add(hitPointSkill);
            }
        }
    }
    #endregion
}
