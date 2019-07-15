using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;


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

[System.Serializable]
public struct AffectedResource
{
    public ResourceType resourceType;
    public float amount;
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


    public static class Resources
    {
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
    }

    ///////////////////////////////////////////////////////////////

    public static class Audio
    {
        public static void playSoundEffect(AudioSource audioSource, AudioClip soundeffect)
        {
            playSoundEffect(audioSource, soundeffect, GlobalValues.soundEffectVolume);
        }

        public static void playSoundEffect(AudioSource audioSource, AudioClip soundeffect, float volume)
        {
            if (soundeffect != null && audioSource != null)
            {
                audioSource.pitch = GlobalValues.soundEffectPitch;

                audioSource.PlayOneShot(soundeffect, volume);
            }
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Collisions
    {
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

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject)
        {
            if (character != null && character.shadowRenderer == null)
            {
                Debug.Log("Schatten-Objekt ist leer!");
                return false;
            }

            int layerMask = 1 << character.gameObject.layer;
            layerMask = ~layerMask;
            float distance = 1f;

            RaycastHit2D hit = Physics2D.Raycast(character.shadowRenderer.transform.position, character.direction, distance, layerMask);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                return true;
            }

            return false;
        }
    }

    ///////////////////////////////////////////////////////////////
    
    public static class UnityUtils
    {
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

        /// <summary>
        /// set bool value for Animator
        /// </summary>
        public static void SetAnimatorParameter(Animator animator, string parameter, bool value)
        {
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    if (param.name == parameter) animator.SetBool(parameter, value);
                }
            }
        }

        /// <summary>
        /// set Trigger for Animator
        /// </summary>
        public static void SetAnimatorParameter(Animator animator, string parameter)
        {
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    if (param.name == parameter) animator.SetTrigger(parameter);
                }
            }
        }

        /// <summary>
        /// set float value for Animator
        /// </summary>
        public static void SetAnimatorParameter(Animator animator, string parameter, float value)
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
    }

    ///////////////////////////////////////////////////////////////

    public static class Items
    {
        public static void setItem(LootTable[] lootTable, bool multiLoot, List<Item> items)
        {
            int rng = UnityEngine.Random.Range(1, Utilities.maxIntSmall);
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

        private static bool hasEnoughCurrencyAndUpdateResource(ResourceType currency, Player player, Item item, int price)
        {
            bool result = false;

            if (currency == ResourceType.none) result = true;
            else if (currency != ResourceType.skill)
            {
                if (player.getResource(currency, item) + price >= 0)
                {
                    if (!item.isKeyItem) player.updateResource(currency, item, price);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public static int getAmountFromInventory(Item item, List<Item> inventory, bool maxAmount)
        {
            Item itemFound = getItem(item, inventory);

            if (itemFound != null)
            {
                if (!maxAmount) return itemFound.amount;
                else return itemFound.maxAmount;
            }

            return 0;
        }

        private static Item getItem(Item item, List<Item> inventory)
        {
            foreach (Item elem in inventory)
            {
                if (item.isKeyItem)
                {
                    if (item.itemName.ToUpper() == elem.itemName.ToUpper()) return elem;
                }
                else
                {
                    if (item.itemGroup.ToUpper() == elem.itemGroup.ToUpper())
                    {
                        return elem;
                    }
                }
            }
            return null;
        }

        public static void updateInventory(Item item, Character character, int amount)
        {
            if (item != null)
            {
                Item found = getItem(item, character.inventory);

                if (found == null)
                {
                    GameObject temp = Instantiate(item.gameObject, character.transform);
                    temp.name = item.name;
                    temp.SetActive(false);
                    temp.hideFlags = HideFlags.HideInHierarchy;
                    character.inventory.Add(temp.GetComponent<Item>());
                }
                else
                {
                    if (!item.isKeyItem)
                    {
                        found.amount += amount;

                        if (found.amount <= 0)
                        {
                            character.inventory.Remove(found);
                        }
                    }
                }
            }
        }

        public static bool canOpenAndUpdateResource(ResourceType currency, Item item, Player player, int price, string defaultText)
        {
            string text = defaultText;            

            if (player != null
                && player.currentState != CharacterState.inDialog
                && player.currentState != CharacterState.inMenu)
            {
                if (hasEnoughCurrencyAndUpdateResource(currency, player, item, -price)) return true;
                else
                {
                    if (text.Replace(" ", "").Length <= 0) text = Format.getDialogBoxText("Du benötigst", price, item, "...");
                    player.showDialogBox(text);
                    return false;
                }
            }

            return false;
        }        
    }

    ///////////////////////////////////////////////////////////////

    public static class Format
    {
        public static string getDialogBoxText(string part1, int price, Item item, string part2)
        {
            string result = part1 + " " + price + " ";

            switch (item.resourceType)
            {
                case ResourceType.item:
                    {
                        if (item.isKeyItem)
                        {
                            result = part1 + " " + item.itemName;
                        }
                        else
                        {
                            string typ = item.itemGroup;

                            if (price == 1 && typ != "Schlüssel") typ = typ.Substring(0, typ.Length - 1);
                            result += typ;
                        }
                    }; break;
                case ResourceType.life: result += "Leben"; break;
                case ResourceType.mana: result += "Mana"; break;
            }

            return result + " " + part2;
        }

        public static string setDamageNumber(float value, float schwelle)
        {
            //if (Mathf.Abs(value) < schwelle)
            //    return value.ToString("0.0");
            //if (Mathf.Abs(value) >= 10)
            //    return value.ToString("#");
            //else return value.ToString("#.0");
            return value + "";
        }

        public static string setDurationToString(float value)
        {
            return Mathf.RoundToInt(value).ToString("0");
        }

        public static string formatString(float value, float maxValue)
        {
            string formatString = "";

            for (int i = 0; i < maxValue.ToString().Length; i++)
            {
                formatString += "0";
            }

            return value.ToString(formatString);
        }

        public static void set3DText(TextMeshPro tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
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
    }

    ///////////////////////////////////////////////////////////////

    public static class Rotation
    {
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
                direction = Utilities.Rotation.DegreeToVector2(temp_angle);
            }

            angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + rotationModifier;

            if (snapRotationInDegrees > 0)
            {
                angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;
                direction = Utilities.Rotation.DegreeToVector2(angle);
            }

            rotation = new Vector3(0, 0, angle);
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Skill
    {
        public static StandardSkill instantiateSkill(StandardSkill skill, Character sender)
        {
            return instantiateSkill(skill, sender, null, 1);
        }

        public static StandardSkill instantiateSkill(StandardSkill skill, Character sender, Character target, float reduce)
        {
            if (sender.currentState != CharacterState.attack && sender.currentState != CharacterState.defend)
            {
                GameObject activeSkill = Instantiate(skill.gameObject, sender.transform.position, Quaternion.identity);

                if (!skill.isStationary) activeSkill.transform.parent = sender.activeSkillParent.transform;

                if (target != null) activeSkill.GetComponent<StandardSkill>().target = target;
                activeSkill.GetComponent<StandardSkill>().sender = sender;

                List<affectedResource> temp = new List<affectedResource>();

                for (int i = 0; i < activeSkill.GetComponent<StandardSkill>().affectedResources.Count; i++)
                {
                    affectedResource elem = activeSkill.GetComponent<StandardSkill>().affectedResources[i];
                    elem.amount /= reduce;
                    temp.Add(elem);
                }

                activeSkill.GetComponent<StandardSkill>().affectedResources = temp;

                activeSkill.GetComponent<StandardSkill>().addResourceSender /= reduce;
                sender.activeSkills.Add(activeSkill.GetComponent<StandardSkill>());

                return activeSkill.GetComponent<StandardSkill>();
            }

            return null;
        }

        public static StandardSkill getSkillByID(List<StandardSkill> skillset, int ID, SkillType category)
        {
            foreach (StandardSkill skill in skillset)
            {
                if (category == skill.category && ID == skill.orderIndex) return skill;
            }

            return null;
        }

        public static StandardSkill getSkillByName(List<StandardSkill> skillset, string name)
        {
            foreach (StandardSkill skill in skillset)
            {
                if (name == skill.skillName) return skill;
            }

            return null;
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

        public static IEnumerator delayInputPlayerCO(float delay, Player player, CharacterState newState)
        {
            //Damit der Spieler nicht gleich wieder die DialogBox aktiviert : /
            yield return new WaitForSeconds(delay);

            if (player != null)
            {
                player.currentState = newState;
            }
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Helper
    {
        public static void checkIfHelperDeactivate(Player player)
        {
            if (!checkIfHelperActivated(player.AButton)
                && !checkIfHelperActivated(player.BButton)
                && !checkIfHelperActivated(player.XButton)
                && !checkIfHelperActivated(player.YButton)
                && !checkIfHelperActivated(player.RBButton)) player.setTargetHelperActive(false);
            else player.setTargetHelperActive(true);
        }

        private static bool checkIfHelperActivated(StandardSkill skill)
        {
            if (skill != null && skill.activeTargetHelper) return true;
            else return false;
        }
    }

}
