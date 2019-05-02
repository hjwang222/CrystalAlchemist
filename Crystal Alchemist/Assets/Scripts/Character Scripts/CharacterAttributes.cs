using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    





    #region Attributes

    [Header("Signale")]
    [Tooltip("GUI Update Signal für Life")]
    public SimpleSignal healthSignal;
    [Tooltip("GUI Update Signal für Mana")]
    public SimpleSignal manaSignal;

    
    [HideInInspector]
    public CastBar castbar;
    [HideInInspector]
    public CastBar activeCastbar;

    [HideInInspector]
    public Transform homePosition;
    [HideInInspector]
    public CharacterState currentState;
    [HideInInspector]
    public float life;
    [HideInInspector]
    public float spellspeed;
    [HideInInspector]
    public float mana;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public bool isInvincible;
    [HideInInspector]
    public bool isHit;
    [HideInInspector]
    public List<GameObject> items = new List<GameObject>();
    [HideInInspector]
    public List<StatusEffect> buffs = new List<StatusEffect>();
    [HideInInspector]
    public List<StatusEffect> debuffs = new List<StatusEffect>();

    public List<StandardSkill> activeSkills = new List<StandardSkill>();
       
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool lockAnimation = false;

    [HideInInspector]
    public float timeDistortion = 1;
    public float speedMultiply = 5;
    public GameObject activeLockOnTarget = null;

    #endregion
}
