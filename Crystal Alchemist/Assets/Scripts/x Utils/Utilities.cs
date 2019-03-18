using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public const float maxFloatInfinite = 9999;
    public const int maxIntInfinite = 9999;

    public const float maxFloatSmall = 99;
    public const int maxIntSmall = 99;

    public const float maxFloatPercent = 1000;
    public const float minFloatPercent = -100;

    public static void fireSkill(Skill skill, Character sender)
    {
        instantiateSkill(skill, sender, null, 1);
    }       

    public static void changeMaterial(SpriteRenderer spriteRenderer, bool showOutline, Color outlineColor, Color mainColor, float invert)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_ShowOutline", showOutline ? 1f : 0);
        if(outlineColor != null) mpb.SetColor("_OutlineColor", outlineColor);
        if(mainColor != null) mpb.SetColor("_Color", mainColor);
        if(invert >= 0 && invert <= 1) mpb.SetFloat("_Threshold", invert);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    public static string setDamageNumber(float value, float schwelle)
    {
        if (Mathf.Abs(value) < schwelle) return value.ToString("0.0");
        if (Mathf.Abs(value) >= 10) return value.ToString("#");
        else return value.ToString("#.0");
    }

    public static string setDurationToString(float value)
    {
        return Mathf.RoundToInt(value).ToString("0");
    }

    public static Skill instantiateSkill(Skill skill, Character sender, Character target, float reduce)
    {
        GameObject activeSkill = Instantiate(skill.gameObject, sender.transform.position, Quaternion.identity);

        if (!skill.isStationary) activeSkill.transform.parent = sender.transform;

        if(target != null) activeSkill.GetComponent<Skill>().target = target;
        activeSkill.GetComponent<Skill>().sender = sender;
        activeSkill.GetComponent<Skill>().addLifeTarget /= reduce;
        activeSkill.GetComponent<Skill>().addManaTarget /= reduce;
        activeSkill.GetComponent<Skill>().addLifeSender /= reduce;
        activeSkill.GetComponent<Skill>().addManaSender /= reduce;
        sender.activeSkills.Add(activeSkill.GetComponent<Skill>());

        return activeSkill.GetComponent<Skill>();
    }

    public static bool hasChildWithTag(Character character, string searchTag)
    {
        bool found = false;

        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == searchTag)
            {
                found = true;
                break;
            }
        }

        return found;
    }

    public static void playSoundEffect(AudioSource audioSource, AudioClip soundeffect, FloatValue soundEffectVolume)
    {
        float volume = 1f;
        if (soundeffect != null && audioSource != null)
        {
            if (soundEffectVolume != null)
            {
                volume = soundEffectVolume.value;
                audioSource.pitch = soundEffectVolume.pitchValue;
            }
            audioSource.PlayOneShot(soundeffect, volume);
        }
    }

    public static bool checkCollision(Collider2D hittedCharacter, Skill skill)
    {
        if (skill != null)
        {
            if (skill.affectSkills
                && hittedCharacter.CompareTag("Skill")
                && hittedCharacter.GetComponent<Skill>().skillName != skill.skillName)
            {
                return true;
            }
            else if (!hittedCharacter.isTrigger)
            {
                if ((skill.affectPlayers && hittedCharacter.CompareTag("Player"))
                    || skill.affectEnemies && hittedCharacter.CompareTag("Enemy")
                    || skill.affectObjects && hittedCharacter.CompareTag("Object")
                    )
                {
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("Skill ist NULL bei checkCollsion (Utilities)");
        }

        return false;
    }

    public static Script setScript(Script addScript, Transform parent)
    {        
        return setScript(addScript, parent, null, null);
    }

    public static Script setScript(Script addScript, Transform parent, Character target)
    {        
        return setScript(addScript, parent, null, target);
    }

    public static Script setScript(Script addScript, Transform parent, Skill skill, Character target)
    {
        Script result = null;

        if (addScript != null)
        {
            result = addScript;
            if(skill != null) result.setSkill(skill);
            if(target != null) result.setTarget(target);
            result.onInitialize();
        }

        return result;
    }

    public static void setItem(LootTable[] lootTable, bool multiLoot, List<GameObject> items)
    {
        int rng = Random.Range(1, Utilities.maxIntSmall);
        int lowestDropRate = 101;

        foreach (LootTable loot in lootTable)
        {
            if (rng <= loot.dropRate)
            {
                if (!multiLoot)
                {
                    if (lowestDropRate > loot.dropRate)
                    {
                        lowestDropRate = loot.dropRate;
                        items.Clear();
                        items.Add(loot.item);
                    }
                }
                else
                {
                    items.Add(loot.item);
                }
            }
        }

        //if (this.items.Count > 0) this.text = this.text.Replace("%XY%", this.items[0].GetComponent<Item>().amount + " " + this.items[0].GetComponent<Item>().name);
    }

    private static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    private static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static void berechneWinkel(Vector2 startPosition, Vector2 senderDirection, float offset, bool rotateIt, float snapRotationInDegrees, out float angle, out Vector2 start, out Vector2 direction)
    {
        start = new Vector2(startPosition.x + (startPosition.x * offset),
                            startPosition.y + (startPosition.y * offset));

        direction = senderDirection;

        angle = 0;
        if (rotateIt) angle = Mathf.Atan2(senderDirection.y, senderDirection.x) * Mathf.Rad2Deg;

        if (snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;
            direction = DegreeToVector2(angle);
        }
    }

}
