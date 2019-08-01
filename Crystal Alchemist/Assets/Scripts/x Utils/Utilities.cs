using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;


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
public class affectedResource
{
    public ResourceType resourceType;

    [ShowIf("resourceType", ResourceType.item)]
    [Tooltip("Benötigtes Item")]
    public Item item;

    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
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
        public static bool checkDistance(Character character, GameObject gameObject, float min, float max, float startDistance, float distanceNeeded, bool useStartDistance, bool useRange)
        {
            float distance = Vector3.Distance(character.transform.position, gameObject.transform.position);

            //Debug.Log(startDistance + " : " + distanceNeeded + " : "+distance);

            if (useStartDistance && distanceNeeded > 0)
            {
                if (distance > (startDistance + distanceNeeded)) return true;
            }
            else if (!useStartDistance && distanceNeeded > 0)
            {
                if (distance > distanceNeeded) return true;
            }
            else if (useRange)
            {
                if (distance >= min && distance <= max) return true;
            }

            return false;
        }

        public static float checkDistanceReduce(Character character, GameObject gameObject, float deadDistance, float saveDistance)
        {
            float distance = Vector3.Distance(character.transform.position, gameObject.transform.position);
            float percentage = 100 - (100 / (saveDistance - deadDistance) * (distance - deadDistance));

            percentage = Mathf.Round(percentage / 25) *25;

            if (percentage > 100) percentage = 100;
            else if (percentage < 0) percentage = 0;

            return percentage;            
        }

        public static bool checkBehindObstacle(Character character, GameObject gameObject)
        {
            float offset = 0.1f;
            Vector2 targetPosition = new Vector2(character.transform.position.x - (character.direction.x * offset),
                                                     character.transform.position.y - (character.direction.y * offset));

            
            if (character.shadowRenderer != null)
            {
                targetPosition = new Vector2(character.shadowRenderer.transform.position.x - (character.direction.x * offset),
                                             character.shadowRenderer.transform.position.y - (character.direction.y * offset));
            }
            
            Vector2 start = gameObject.transform.position;
            Vector2 direction = (targetPosition - start).normalized;

            RaycastHit2D hit = Physics2D.Raycast(start, direction, 100f);            

            if (hit && !hit.collider.isTrigger)
            {
                if (hit.collider.gameObject != character.gameObject)
                {
                    //Debug.DrawLine(start, hit.transform.position, Color.green);
                    return true;
                }
                else
                {
                    //Debug.DrawLine(start, hit.transform.position, Color.red);
                    return false;
                }
            }


            return true;
        }

        public static bool checkCollision(Collider2D hittedCharacter, StandardSkill skill)
        {
            if (skill != null && skill.triggerIsActive)
            {
                if (hittedCharacter.gameObject == skill.sender.gameObject)
                {
                    if (!skill.affectSelf) return false;
                    else return true;
                }
                else
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
            }

            return false;
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject target, int range)
        {
            Vector2 direction = character.direction;
            Vector2 temp = (character.transform.position - target.transform.position).normalized;

            float direction_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float temp_angle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg;

            float angle = Mathf.Abs(direction_angle - temp_angle);

            float min = 180 - range;
            float max = 180 + range;

            //Debug.Log(angle + ">>" + min + ":" + max);

            if (angle >= min && angle <= max) return true;
            return false;
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject)
        {
            return checkIfGameObjectIsViewed(character, gameObject, 0.5f);
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject, float distance)
        {
            if (character != null && character.shadowRenderer == null)
            {
                Debug.Log("Schatten-Objekt ist leer!");
                return false;
            }

            int layerMask = 9; //Map
            float width = 0.2f;
            float offset = 0.1f;

            Vector2 position = new Vector2(character.shadowRenderer.transform.position.x - (character.direction.x * offset),
                                           character.shadowRenderer.transform.position.y - (character.direction.y * offset));

            RaycastHit2D hit = Physics2D.CircleCast(position, width, character.direction, distance, layerMask);

            if (hit != false && !hit.collider.isTrigger && hit.collider.gameObject == gameObject) return true;

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
                    if (text.Replace(" ", "").Length <= 0)
                    {
                        text = Format.getDialogBoxText("Du benötigst", price, item, "...");
                        if (GlobalValues.useAlternativeLanguage) text = Format.getDialogBoxText("You need", price, item, "...");
                    }
                    player.showDialogBox(text);
                    return false;
                }
            }

            return false;
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class StatusEffectUtil
    {
        public static bool isCharacterStunned(Character character)
        {
            foreach (StatusEffect debuff in character.debuffs)
            {
                if (debuff.stunTarget) return true;
            }

            return false;
        }

        public static void RemoveAllStatusEffects(List<StatusEffect> statusEffects)
        {
            List<StatusEffect> dispellStatusEffects = new List<StatusEffect>();

            //Store in temp List to avoid Enumeration Exception
            foreach (StatusEffect effect in statusEffects)
            {
                dispellStatusEffects.Add(effect);
            }

            foreach (StatusEffect effect in dispellStatusEffects)
            {
                effect.DestroyIt();
            }

            dispellStatusEffects.Clear();
        }

        public static void RemoveStatusEffect(StatusEffect statusEffect, bool allTheSame, Character character)
        {
            List<StatusEffect> statusEffects = null;
            List<StatusEffect> dispellStatusEffects = new List<StatusEffect>();

            if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = character.debuffs;
            else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.buffs;

            //Store in temp List to avoid Enumeration Exception
            foreach (StatusEffect effect in statusEffects)
            {
                if (effect.statusEffectName == statusEffect.statusEffectName)
                {
                    dispellStatusEffects.Add(effect);
                    if (!allTheSame) break;
                }
            }

            foreach (StatusEffect effect in dispellStatusEffects)
            {
                effect.DestroyIt();
            }

            dispellStatusEffects.Clear();
        }

        public static void AddStatusEffect(StatusEffect statusEffect, Character character)
        {
            if (statusEffect != null && character.characterType != CharacterType.Object)
            {
                bool isImmune = false;

                for (int i = 0; i < character.immunityToStatusEffects.Count; i++)
                {
                    StatusEffect immunityEffect = character.immunityToStatusEffects[i];
                    if (statusEffect.statusEffectName == immunityEffect.statusEffectName)
                    {
                        isImmune = true;
                        break;
                    }
                }

                if (!isImmune)
                {
                    List<StatusEffect> statusEffects = null;
                    List<StatusEffect> result = new List<StatusEffect>();

                    //add to list for better reference
                    if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = character.debuffs;
                    else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.buffs;

                    for (int i = 0; i < statusEffects.Count; i++)
                    {
                        if (statusEffects[i].statusEffectName == statusEffect.statusEffectName)
                        {
                            //Hole alle gleichnamigen Effekte aus der Liste
                            result.Add(statusEffects[i]);
                        }
                    }

                    //TODO, das geht noch besser
                    if (result.Count < statusEffect.maxStacks)
                    {
                        //Wenn der Effekte die maximale Anzahl Stacks nicht überschritten hat -> Hinzufügen
                        instantiateStatusEffect(statusEffect, statusEffects, character);
                    }
                    else
                    {
                        if (statusEffect.canOverride && statusEffect.endType == StatusEffectEndType.time)
                        {
                            //Wenn der Effekt überschreiben kann, soll der Effekt mit der kürzesten Dauer entfernt werden
                            StatusEffect toDestroy = result[0];
                            toDestroy.DestroyIt();

                            instantiateStatusEffect(statusEffect, statusEffects, character);
                        }
                        else if (statusEffect.canDeactivateIt && statusEffect.endType == StatusEffectEndType.mana)
                        {
                            StatusEffect toDestroy = result[0];
                            toDestroy.DestroyIt();
                        }
                    }
                }
            }
        }

        private static void instantiateStatusEffect(StatusEffect statusEffect, List<StatusEffect> statusEffects, Character character)
        {
            GameObject statusEffectClone = Instantiate(statusEffect.gameObject, character.transform.position, Quaternion.identity, character.transform);
            statusEffectClone.transform.parent = character.activeStatusEffectParent.transform;
            DontDestroyOnLoad(statusEffectClone);
            StatusEffect statusEffectScript = statusEffectClone.GetComponent<StatusEffect>();
            statusEffectScript.target = character;
            //statusEffectClone.hideFlags = HideFlags.HideInHierarchy;

            //add to list for better reference
            statusEffects.Add(statusEffectClone.GetComponent<StatusEffect>());
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Format
    {
        public static void SetButtonColor(Button button, Color newColor)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = newColor;
            button.colors = cb;
        }

        public static string getDialogBoxText(string part1, int price, Item item, string part2)
        {
            string result = part1 + " " + price + " ";

            switch (item.resourceType)
            {
                case ResourceType.item:
                    {
                        if (item.isKeyItem)
                        {
                            result = part1 + " " + getLanguageDialogText(item.itemName, item.itemNameEnglish);
                        }
                        else
                        {
                            string typ = getLanguageDialogText(item.itemGroup, item.itemGroupEnglish);


                            if (price == 1 && (typ != "Schlüssel" || GlobalValues.useAlternativeLanguage)) typ = typ.Substring(0, typ.Length - 1);

                            result += typ;
                        }
                    }; break;
                case ResourceType.life: result += "Leben"; break;
                case ResourceType.mana: result += "Mana"; break;
            }

            return result + " " + part2;
        }

        public static string getLanguageDialogText(string originalText, string alternativeText)
        {
            if (GlobalValues.useAlternativeLanguage && alternativeText.Replace(" ", "").Length > 1) return alternativeText;
            else return originalText;
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
        
        public static void getStartTime(float factor, out int hour, out int minute)
        {
            DateTime origin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now - origin;
            double difference = Math.Floor(diff.TotalSeconds);
            float minutes = (float)(difference * (double)factor); //elapsed ingame minutes
            float fhour = ((minutes / 60f) % 24f);
            float fminute = (minutes % 60f);

            hour = Mathf.RoundToInt(fhour);
            minute = Mathf.RoundToInt(fminute);
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

        public static void setDirectionAndRotation(Character sender, Character target,
                                                    float positionOffset, float positionHeight, float snapRotationInDegrees, float rotationModifier,
                                                   out float angle, out Vector2 start, out Vector2 direction, out Vector3 rotation)
        {
            direction = sender.direction.normalized;

            start = new Vector2(sender.transform.position.x + (direction.x * positionOffset),
                                sender.transform.position.y + (direction.y * positionOffset) + positionHeight);

            //if sender is not frozen

            if (target != null) direction = (Vector2)target.transform.position - start;

            float temp_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            direction = Utilities.Rotation.DegreeToVector2(temp_angle);

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
        public static int getAmountOfSameSkills(StandardSkill skill, List<StandardSkill> activeSkills)
        {
            int result = 0;

            for (int i = 0; i < activeSkills.Count; i++)
            {
                StandardSkill activeSkill = activeSkills[i];
                if (activeSkill.skillName == skill.skillName) result++;
            }

            return result;
        }

        public static StandardSkill setSkill(Character character, StandardSkill prefab)
        {
            StandardSkill skillInstance = MonoBehaviour.Instantiate(prefab, character.skillSetParent.transform) as StandardSkill;
            skillInstance.sender = character;
            skillInstance.gameObject.SetActive(false);

            return skillInstance;
        }

        public static StandardSkill instantiateSkill(StandardSkill skill, Character sender)
        {
            return instantiateSkill(skill, sender, null, 1);
        }

        public static StandardSkill instantiateSkill(StandardSkill skill, Character sender, Character target)
        {
            return instantiateSkill(skill, sender, target, 1);
        }

        public static StandardSkill instantiateSkill(StandardSkill skill, Character sender, Character target, float reduce)
        {
            if (skill != null
                && sender.currentState != CharacterState.attack
                && sender.currentState != CharacterState.defend)
            {
                GameObject activeSkill = Instantiate(skill.gameObject, sender.transform.position, Quaternion.identity);
                activeSkill.SetActive(true);

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

                /*
                if (skill.comboAmount > 0 
                    && skill.cooldownAfterCombo > skill.cooldown
                    && getAmountOfSameSkills(skill, sender.activeSkills) >= skill.comboAmount)
                    skill.cooldownTimeLeft = skill.cooldownAfterCombo;*/

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

        public static void updateSkillset(StandardSkill skill, Player player)
        {
            if (skill != null)
            {
                bool found = false;

                foreach (StandardSkill elem in player.skillSet)
                {
                    if (elem.skillName == skill.skillName) { found = true; break; }
                }

                if (!found) player.skillSet.Add(skill);
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
