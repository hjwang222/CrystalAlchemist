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

    public static void fireSkill(StandardSkill skill, Character sender)
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

    public static StandardSkill instantiateSkill(StandardSkill skill, Character sender, Character target, float reduce)
    {
        GameObject activeSkill = Instantiate(skill.gameObject, sender.transform.position, Quaternion.identity);

        if (!skill.isStationary) activeSkill.transform.parent = sender.transform;

        if(target != null) activeSkill.GetComponent<StandardSkill>().target = target;
        activeSkill.GetComponent<StandardSkill>().sender = sender;
        activeSkill.GetComponent<StandardSkill>().addLifeTarget /= reduce;
        activeSkill.GetComponent<StandardSkill>().addManaTarget /= reduce;
        activeSkill.GetComponent<StandardSkill>().addLifeSender /= reduce;
        activeSkill.GetComponent<StandardSkill>().addManaSender /= reduce;
        sender.activeSkills.Add(activeSkill.GetComponent<StandardSkill>());

        return activeSkill.GetComponent<StandardSkill>();
    }

    public static GameObject hasChildWithTag(Character character, string searchTag)
    {
        GameObject result = null;

        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == searchTag)
            {
                result = character.transform.GetChild(i).gameObject;
                return result;
            }
        }

        return result;
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

    public static bool checkCollision(Collider2D hittedCharacter, StandardSkill skill)
    {
        if (skill != null && skill.triggerIsActive)
        {
            if (skill.affectSkills
                && hittedCharacter.CompareTag("Skill")
                && hittedCharacter.GetComponent<StandardSkill>().skillName != skill.skillName)
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

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static void setDirectionAndRotation(Vector2 senderPosition, Vector2 senderDirection, float positionOffset, float positionHeight, float snapRotationInDegrees, float rotationModifier,
                                               out float angle, out Vector2 start, out Vector2 direction, out Vector3 rotation)
    {
        direction = senderDirection;

        angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + rotationModifier;

        if (snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;
            direction = Utilities.DegreeToVector2(angle);
        }

        start = new Vector2(senderPosition.x + (direction.x * positionOffset),
                            senderPosition.y + (direction.y * positionOffset) + positionHeight);

        rotation = new Vector3(0, 0, angle);
    }

}
