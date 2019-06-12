using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

#region Objects
[System.Serializable]
public struct LootTable
{
    [VerticalGroup("Split")]
    public Item item;

    [HorizontalGroup("Split/Box", 50)]
    [ShowIf("item")]
    [Range(0, 100)]
    public int dropRate;

    [ShowIf("item")]
    [HorizontalGroup("Split/Box", 50)]
    [Range(1, 99)]
    public int amount;
}

public enum ResourceType
{
    none,
    life,
    mana,
    item, 
    skill
}

#endregion

public class Utilities : MonoBehaviour
{
    #region Konstanten
    public const float maxFloatInfinite = 9999;
    public const int maxIntInfinite = 9999;
    public const float minFloat = 0.1f;

    public const float maxFloatSmall = 99;
    public const int maxIntSmall = 99;

    public const float maxFloatPercent = 1000;
    public const float minFloatPercent = -100;
    #endregion


    #region Character and Skill Utils

    public static float setResource(float resource, float max, float addResource)
    {
        if (addResource != 0)
        {
            if (resource + addResource > max) addResource = max - resource;
            else if (resource + addResource < 0) resource = 0;

            resource += addResource;
        }

        return resource;
    }

    public static void fireSkill(StandardSkill skill, Character sender)
    {
        instantiateSkill(skill, sender, null, 1);
    }

    public static StandardSkill instantiateSkill(StandardSkill skill, Character sender, Character target, float reduce)
    {
        GameObject activeSkill = Instantiate(skill.gameObject, sender.transform.position, Quaternion.identity);

        if (!skill.isStationary) activeSkill.transform.parent = sender.transform;

        if (target != null) activeSkill.GetComponent<StandardSkill>().target = target;
        activeSkill.GetComponent<StandardSkill>().sender = sender;
        activeSkill.GetComponent<StandardSkill>().addLifeTarget /= reduce;
        activeSkill.GetComponent<StandardSkill>().addManaTarget /= reduce;
        activeSkill.GetComponent<StandardSkill>().addResourceSender /= reduce;
        sender.activeSkills.Add(activeSkill.GetComponent<StandardSkill>());

        return activeSkill.GetComponent<StandardSkill>();
    }

    public static void playSoundEffect(AudioSource audioSource, AudioClip soundeffect)
    {
        float volume = 1f;
        if (soundeffect != null && audioSource != null)
        {
            volume = GlobalValues.soundEffectVolume;
            audioSource.pitch = GlobalValues.soundEffectPitch;

            audioSource.PlayOneShot(soundeffect, volume);
        }
    }

    public static void setItem(LootTable[] lootTable, bool multiLoot, List<Item> items)
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

    #endregion


    #region UI Utils

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

    #endregion


    #region Check Utils

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

        return false;
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

    public static void SetParameter(Animator animator, string parameter, bool value)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetBool(parameter, value);
            }
        }
    }

    public static void SetParameter(Animator animator, string parameter, float value)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetFloat(parameter, value);
            }
        }
    }

    public static bool HasParameter(Animator animator, string parameter)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) return true;
            }
        }

        return false;
    }

    private static bool hasEnoughCurrency(ResourceType currency, Player player, Item item, int price)
    {
        bool result = false;

        if (currency == ResourceType.none) result = true;        
        else if(currency != ResourceType.skill)
        {
            if (player.getResource(currency, item) + price >= 0)
            {
                player.updateResource(currency, item, price);
                result = true;
            }
            else
            {
                result = false;
            }
        }

        return result;
    }

    public static int getAmountFromInventory(ItemGroup itemgroup, List<Item> inventory, bool maxAmount)
    {        
            foreach (Item elem in inventory)
            {
                if (itemgroup == elem.itemGroup)
                {
                    if (!maxAmount) return elem.amount;
                    else return elem.maxAmount;
                }
            }        

        return 0;
    }

    public static void updateInventory(Item item, Character character, int amount)
    {
        if (item != null)
        {
            Item found = null;

            foreach (Item elem in character.inventory)
            {
                if (item.itemGroup == elem.itemGroup)
                {
                    found = elem;
                }
            }

            if (found == null)
            {
                GameObject temp = Instantiate(item.gameObject, character.transform);
                temp.SetActive(false);
                temp.hideFlags = HideFlags.HideInHierarchy;
                character.inventory.Add(temp.GetComponent<Item>());
            }
            else
            {
                found.amount += amount;
            }
        }
    }


    public static void updateSkillset(StandardSkill skill, Character character)
    {
        if (skill != null)
        {
            bool found = false;

            foreach (StandardSkill elem in character.skillSet)
            {
                if (elem.skillName == skill.skillName) { found = true; break; }
            }

            if (!found) character.skillSet.Add(skill);
        }
    }




    public static float getInputMenu(string axis)
    {        
            float changeAnalogStick = Mathf.RoundToInt(Input.GetAxisRaw(axis));
            float changeDPad = Input.GetAxisRaw("Cursor " + axis);
            if (changeAnalogStick != 0) return changeAnalogStick;
            else if (changeDPad != 0) return changeDPad;
            return 0;        
    }



    public static bool canOpen(ResourceType currency, Item item, Player player, int price)
    {
        if (player != null && player.currentState != CharacterState.inDialog)
        {
            if (hasEnoughCurrency(currency, player, item, -price)) return true;
            else
            {
                player.showDialogBox(GlobalValues.noMoneyText);
                return false;
            }
        }

        return false;
    }

    public static void set3DText(TextMeshPro tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
    {
        if (tmp != null)
        {
            if(text != null) tmp.text = text + "";
            if(bold) tmp.fontStyle = FontStyles.Bold;
            if(outlineColor != null) tmp.outlineColor = outlineColor;
            if(fontColor != null) tmp.color = fontColor;
            if(outlineWidth > 0) tmp.outlineWidth = outlineWidth;
        }
    }

    public static void set3DText(TextMeshProUGUI tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
    {
        if (tmp != null)
        {
            if (text != null) tmp.text = text + "";
            if (bold) tmp.fontStyle = FontStyles.Bold;
            if (outlineColor != null) tmp.outlineColor = outlineColor;
            if (fontColor != null) tmp.color = fontColor;
            if (outlineWidth > 0) tmp.outlineWidth = outlineWidth;
        }
    }

    #endregion


    #region Direction and Rotation Utils

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static void setDirectionAndRotation(Vector2 senderPosition, Vector2 senderDirection, Character target,
                                                float positionOffset, float positionHeight, float snapRotationInDegrees, float rotationModifier,
                                               out float angle, out Vector2 start, out Vector2 direction, out Vector3 rotation)
    {
        direction = senderDirection;

        start = new Vector2(senderPosition.x + (direction.x * positionOffset),
                            senderPosition.y + (direction.y * positionOffset) + positionHeight);

        if (target != null)
        {
            direction = (Vector2)target.transform.position - start;
            float temp_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            direction = Utilities.DegreeToVector2(temp_angle);
        }

        angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + rotationModifier;

        if (snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;
            direction = Utilities.DegreeToVector2(angle);
        }

        rotation = new Vector3(0, 0, angle);
    }

    #endregion


    #region Later Used Utils

    public static void changeMaterial(SpriteRenderer spriteRenderer, bool showOutline, Color outlineColor, Color mainColor, float invert)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_ShowOutline", showOutline ? 1f : 0);
        if (outlineColor != null) mpb.SetColor("_OutlineColor", outlineColor);
        if (mainColor != null) mpb.SetColor("_Color", mainColor);
        if (invert >= 0 && invert <= 1) mpb.SetFloat("_Threshold", invert);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    #endregion


    public static IEnumerator delayInputPlayerCO(float delay, Player player)
    {
        //Damit der Spieler nicht gleich wieder die DialogBox aktiviert : /
        yield return new WaitForSeconds(delay);

        if (player != null)
        {
            player.currentState = CharacterState.idle;
        }
    }
}
