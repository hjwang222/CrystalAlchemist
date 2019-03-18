using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Script : MonoBehaviour
{
    [HideInInspector]
    public Character target;

    [HideInInspector]
    public Skill skill;

    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float value;

    public GameObject instantiatNewGameObject;

    public void setValue(float value)
    {
        this.value = value;
    }

    public void setSkill(Skill skill)
    {
        this.skill = skill;
    }

    public void setTarget(Character target)
    {
        this.target = target;
    }

    abstract public void onDestroy();

    abstract public void onInitialize();

    abstract public void onUpdate();


    abstract public void onEnter(Collider2D hittedCharacter);

    abstract public void onExit(Collider2D hittedCharacter);

    abstract public void onStay(Collider2D hittedCharacter);

}
