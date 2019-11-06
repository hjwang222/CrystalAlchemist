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
    skill,
    statuseffect
}

#endregion

public class Utilities : MonoBehaviour
{
    #region Konstanten
    public const float maxFloatInfinite = 9999;
    public const int maxIntInfinite = 9999;
    public const float minFloat = 0.1f;

    public const float maxFloatSmall = 100;
    public const int maxIntSmall = 100;

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

            percentage = Mathf.Round(percentage / 25) * 25;

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

        public static bool checkAffections(Character character, bool affectOther, bool affectSame, bool affectNeutral, Collider2D hittedCharacter)
        {
            Character target = hittedCharacter.GetComponent<Character>();

            if (!hittedCharacter.isTrigger
                && target != null
                && target.currentState != CharacterState.dead
                && target.currentState != CharacterState.respawning
           && (
               (affectOther && (character.CompareTag("Player") || character.CompareTag("NPC")) && target.CompareTag("Enemy"))
            || (affectOther && (character.CompareTag("Enemy") && (target.CompareTag("Player") || target.CompareTag("NPC"))))
            || (affectSame && (character.CompareTag("Player") == target.CompareTag("Player") || character.CompareTag("Player") == target.CompareTag("NPC")))
            || (affectSame && (character.CompareTag("NPC") == target.CompareTag("NPC") || character.CompareTag("NPC") == target.CompareTag("Player")))
            || (affectSame && (character.CompareTag("Enemy") == target.CompareTag("Enemy")))
            || (affectNeutral && target.CompareTag("Object")))) return true;

            return false;
        }

        private static bool skillAffected(Collider2D hittedCharacter, Skill skill)
        {
            Skill tempSkill = Skills.getSkillByCollision(hittedCharacter.gameObject);

            if (tempSkill != null)
            {
                if (skill.GetComponent<SkillTargetModule>() != null
                && skill.GetComponent<SkillTargetModule>().affectSkills
                && tempSkill.CompareTag("Skill")
                && tempSkill.skillName != skill.skillName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool checkCollision(Collider2D hittedCharacter, Skill skill)
        {
            if (skill != null && skill.triggerIsActive)
            {
                if (skill.sender != null && hittedCharacter.gameObject == skill.sender.gameObject)
                {
                    if (skill.GetComponent<SkillTargetModule>() != null
                        && skill.GetComponent<SkillTargetModule>().affectSelf) return true;
                    else return false;
                }
                else
                {
                    if (skillAffected(hittedCharacter, skill))
                    {
                        return true;
                    }
                    else if (!hittedCharacter.isTrigger)
                    {
                        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();
                        if (targetModule != null
                            && checkAffections(skill.sender, targetModule.affectOther, targetModule.affectSame, targetModule.affectNeutral, hittedCharacter))
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
            return checkIfGameObjectIsViewed(character, gameObject, 1f);
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject, float distance)
        {
            if (character != null && character.shadowRenderer == null)
            {
                Debug.Log("Schatten-Objekt ist leer!");
                return false;
            }

            float width = 0.2f;
            float offset = 0.1f;

            Vector2 position = new Vector2(character.shadowRenderer.transform.position.x - (character.direction.x * offset),
                                           character.shadowRenderer.transform.position.y - (character.direction.y * offset));

            RaycastHit2D[] hit = Physics2D.CircleCastAll(position, width, character.direction, distance);

            foreach (RaycastHit2D hitted in hit)
            {
                if (hitted != false && !hitted.collider.isTrigger && hitted.collider.transform.parent.gameObject == gameObject) return true;
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

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst as T;
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

        public static bool hasEnoughCurrency(ResourceType currency, Player player, Item item, int price)
        {
            bool result = false;

            if (currency == ResourceType.none) result = true;
            else if (currency != ResourceType.skill)
            {
                if (player.getResource(currency, item) - price >= 0) result = true;                
                else result = false;                
            }

            return result;
        }

        public static int getAmountFromInventory(Item item, List<Item> inventory, bool maxAmount)
        {
            Item itemFound = getItemFromInventory(item, inventory);

            if (itemFound != null)
            {
                if (!maxAmount) return itemFound.amount;
                else return itemFound.maxAmount;
            }

            return 0;
        }

        public static Item getItemFromInventory(Item item, List<Item> inventory)
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
                Item found = getItemFromInventory(item, character.inventory);

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

        public static bool canOpenAndUpdateResource(ResourceType currency, Item item, Player player, int price)
        {
            if (player != null
                && player.currentState != CharacterState.inDialog
                && player.currentState != CharacterState.respawning
                && player.currentState != CharacterState.inMenu)
            {
                if (hasEnoughCurrency(currency, player, item, price))
                {
                    reduceCurrency(currency, item, player, price);
                    return true;
                }
            }

            return false;
        }

        public static void reduceCurrency(ResourceType currency, Item item, Player player, int price)
        {
            if (!item.isKeyItem) player.updateResource(currency, item, -price);
        }

        public static Item getItemByID(List<Item> inventory, int ID, bool isKeyItem)
        {
            foreach (Item item in inventory)
            {
                if (item.isKeyItem == isKeyItem && item.itemSlot == ID) return item;
            }

            return null;
        }

        public static bool hasItemGroup(string itemGroup, List<Item> inventory)
        {
            foreach (Item elem in inventory)
            {
                if (itemGroup.ToUpper() == elem.itemGroup.ToUpper()) return true;                
            }

             return false;
        }

        public static void setItemImage(Image image, Item item)
        {
            if (item.itemSpriteInventory != null) image.sprite = item.itemSpriteInventory;
            else image.sprite = item.itemSprite;
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
            if (statusEffect != null && character.stats.characterType != CharacterType.Object)
            {
                bool isImmune = false;

                if (character.stats.isImmuneToAllDebuffs 
                    && statusEffect.statusEffectType == StatusEffectType.debuff) isImmune = true;
                else
                {
                    for (int i = 0; i < character.stats.immunityToStatusEffects.Count; i++)
                    {
                        StatusEffect immunityEffect = character.stats.immunityToStatusEffects[i];
                        if (statusEffect.statusEffectName == immunityEffect.statusEffectName)
                        {
                            isImmune = true;
                            break;
                        }
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
                        instantiateStatusEffect(statusEffect, character);
                    }
                    else
                    {
                        if (statusEffect.canOverride)
                        {
                            //Wenn der Effekt überschreiben kann, soll der Effekt mit der kürzesten Dauer entfernt werden
                            StatusEffect toDestroy = result[0];
                            toDestroy.DestroyIt();

                            instantiateStatusEffect(statusEffect, character);
                        }
                        else if (statusEffect.canDeactivateIt)
                        {
                            StatusEffect toDestroy = result[0];
                            toDestroy.DestroyIt();
                        }
                    }
                }
            }
        }

        private static void instantiateStatusEffect(StatusEffect statusEffect, Character character)
        {
            StatusEffect statusEffectClone = Instantiate(statusEffect, character.transform.position, Quaternion.identity, character.transform);
            statusEffectClone.setTarget(character);
        }

        public static List<StatusEffect> GetStatusEffect(StatusEffect statusEffect, Character character, bool getAll)
        {
            List<StatusEffect> result = new List<StatusEffect>();

            foreach(StatusEffect effect in character.buffs)
            {
                if (effect.statusEffectName == statusEffect.statusEffectName)
                {
                    result.Add(effect);
                    if (!getAll) break;
                }
            }

            foreach (StatusEffect effect in character.debuffs)
            {
                if (effect.statusEffectName == statusEffect.statusEffectName)
                {
                    result.Add(effect);
                    if (!getAll) break;
                }
            }

            return result;
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
            int rounded = Mathf.RoundToInt(value);

            if (rounded > 59) return (rounded/60).ToString("0")+"m";
            else return rounded.ToString("0")+"s";
        }

        public static string formatString(float value, float maxValue)
        {
            string formatString = "";

            for (int i = 0; i < maxValue.ToString().Length; i++)
            {
                formatString += "0";
            }

            if (value == 0) return formatString;
            else return value.ToString(formatString);
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
            float minutes = (float)(difference / (double)factor); //elapsed ingame minutes
            float fhour = ((minutes / 60f) % 24f);
            float fminute = (minutes % 60f);

            if (fhour >= 24) fhour = 0;
            if (fminute >= 60) fminute = 0;

            hour = Mathf.RoundToInt(fhour);
            minute = Mathf.RoundToInt(fminute);
        }               
    }

    ///////////////////////////////////////////////////////////////

    public static class DialogBox
    {
        public static void showDialog(Interactable interactable, Player player)
        {
            showDialog(interactable, player, null);
        }

        public static void showDialog(Interactable interactable, Player player, DialogTextTrigger trigger)
        {
            showDialog(interactable, player, trigger, null);
        }

        public static void showDialog(Interactable interactable, Player player, Item loot)
        {
            if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.GetComponent<DialogSystem>().show(player, interactable, loot);
        }

        public static void showDialog(Interactable interactable, Player player, DialogTextTrigger trigger, Item loot)
        {
            if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.gameObject.GetComponent<DialogSystem>().show(player, trigger, interactable, loot);
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

        public static void rotateCollider(Character character, GameObject gameObject)
        {
            float angle = (Mathf.Atan2(character.direction.y, character.direction.x) * Mathf.Rad2Deg) + 90;

            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public static void setDirectionAndRotation(Skill skill, out float angle, out Vector2 start, out Vector2 direction, out Vector3 rotation)
        {
            float snapRotationInDegrees = 0;
            float rotationModifier = 0;
            float positionOffset = skill.positionOffset;
            float positionHeight = 0;
            bool useCustomPosition = false;

            SkillRotationModule rotationModule = skill.GetComponent<SkillRotationModule>();
            SkillPositionZModule positionModule = skill.GetComponent<SkillPositionZModule>();

            if (rotationModule != null)
            {
                snapRotationInDegrees = rotationModule.snapRotationInDegrees;
                rotationModifier = rotationModule.rotationModifier;
            }

            if (positionModule != null)
            {
                useCustomPosition = positionModule.useGameObjectHeight;
                positionHeight = positionModule.positionHeight;                
            }                       

            start = (Vector2)skill.sender.transform.position;

            if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null) direction = (Vector2)skill.sender.GetComponent<AI>().target.transform.position - start;
            else if (skill.target != null) direction = (Vector2)skill.target.transform.position - start;
            else direction = skill.sender.direction.normalized;

            float positionX = skill.sender.spriteRenderer.transform.position.x + (direction.x * positionOffset);
            float positionY = skill.sender.spriteRenderer.transform.position.y + (direction.y * positionOffset) + positionHeight;

            if (useCustomPosition) positionY = skill.sender.shootingPosition.transform.position.y + (direction.y * positionOffset);

            start = new Vector2(positionX, positionY);
            if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null) direction = (Vector2)skill.sender.GetComponent<AI>().target.transform.position - start;
            else if (skill.target != null) direction = (Vector2)skill.target.transform.position - start;


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

    public static class Skills
    {
        public static int getAmountOfSameSkills(Skill skill, List<Skill> activeSkills, List<Character> activePets)
        {
            int result = 0;

            SkillSummon summonSkill = skill.GetComponent<SkillSummon>();

            if (summonSkill == null)
            {
                for (int i = 0; i < activeSkills.Count; i++)
                {
                    Skill activeSkill = activeSkills[i];
                    if (activeSkill.skillName == skill.skillName) result++;
                }
            }
            else
            {
                for (int i = 0; i < activePets.Count; i++)
                {
                    if (activePets[i] != null && activePets[i].stats.characterName == summonSkill.getPetName())
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public static Skill setSkill(Character character, Skill prefab)
        {
            Skill skillInstance = MonoBehaviour.Instantiate(prefab, character.skillSetParent.transform) as Skill;
            skillInstance.sender = character;
            skillInstance.gameObject.SetActive(false);
            skillInstance.preLoad();

            return skillInstance;
        }

        public static Skill instantiateSkill(Skill skill, Character sender)
        {
            return instantiateSkill(skill, sender, null, 1);
        }

        public static Skill instantiateSkill(Skill skill, Character sender, Character target)
        {
            return instantiateSkill(skill, sender, target, 1);
        }

        public static Skill instantiateSkill(Skill skill, Character sender, Character target, float reduce)
        {
            if (skill != null
                && sender.currentState != CharacterState.attack
                && sender.currentState != CharacterState.defend)
            {
                Skill activeSkill = Instantiate(skill, sender.transform.position, Quaternion.identity);
                activeSkill.gameObject.SetActive(true);
                SkillTargetModule targetModule = activeSkill.GetComponent<SkillTargetModule>();
                SkillSenderModule sendermodule = activeSkill.GetComponent<SkillSenderModule>();

                if (skill.attachToSender) activeSkill.transform.parent = sender.activeSkillParent.transform;

                if (target != null) activeSkill.target = target;
                activeSkill.sender = sender;

                List<affectedResource> temp = new List<affectedResource>();

                if (targetModule != null)
                {

                    for (int i = 0; i < targetModule.affectedResources.Count; i++)
                    {
                        affectedResource elem = targetModule.affectedResources[i];
                        elem.amount /= reduce;
                        temp.Add(elem);
                    }

                    targetModule.affectedResources = temp;
                    if(sendermodule != null) sendermodule.addResourceSender /= reduce;
                }

                sender.activeSkills.Add(activeSkill);

                /*
                if (skill.comboAmount > 0 
                    && skill.cooldownAfterCombo > skill.cooldown
                    && getAmountOfSameSkills(skill, sender.activeSkills) >= skill.comboAmount)
                    skill.cooldownTimeLeft = skill.cooldownAfterCombo;*/

                return activeSkill.GetComponent<Skill>();
            }

            return null;
        }

        public static Skill getSkillByID(List<Skill> skillset, int ID, SkillType category)
        {
            foreach (Skill skill in skillset)
            {
                SkillBookModule skillBookModule = skill.GetComponent<SkillBookModule>();

                if (skillBookModule != null 
                    && category == skillBookModule.category
                    && ID == skillBookModule.orderIndex) return skill;
            }

            return null;
        }

        public static Skill getSkillByName(List<Skill> skillset, string name)
        {
            foreach (Skill skill in skillset)
            {
                if (name == skill.skillName) return skill;
            }

            return null;
        }

        public static Skill getSkillByCollision(GameObject collision)
        {
            return collision.GetComponentInParent<Skill>();
        }

        public static void updateSkillset(Skill skill, Player player)
        {
            if (skill != null)
            {
                bool found = false;

                foreach (Skill elem in player.skillSet)
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

        private static bool checkIfHelperActivated(Skill skill)
        {
            if (skill != null 
                && skill.GetComponent<SkillAimingModule>() != null) return true;
            else return false;
        }
    }

}
