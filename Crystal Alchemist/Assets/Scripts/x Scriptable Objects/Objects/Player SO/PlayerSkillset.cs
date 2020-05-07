using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Player Skillset")]
public class PlayerSkillset : ScriptableObject
{
    [SerializeField]
    private List<Ability> abilities = new List<Ability>();

    public void Clear()
    {
        Initialize();
    }

    public void Initialize()
    {
        this.abilities.Clear();
        foreach (Ability ability in MasterManager.abilities) AddAbility(ability);        
    }

    public void SetSender(Character sender)
    {
        this.abilities.RemoveAll(item => item == null);
        foreach (Ability ability in this.abilities) ability.SetSender(sender); 
    }

    public void Updating()
    {
        this.abilities.RemoveAll(item => item == null);
        foreach (Ability ability in this.abilities) ability.Update();        
    }

    public Ability getAbilityByName(string name)
    {
        foreach (Ability ability in this.abilities)
        {
            if (name == ability.name) return ability;
        }

        return null;
    }

    public Ability getSkillByID(int ID, SkillType category)
    {
        foreach (Ability ability in this.abilities)
        {
            SkillBookInfo info = ability.info;

            if (info != null
                && category == info.category
                && ID == info.orderIndex) return ability;
        }

        return null;
    }

    public void AddAbility(Ability ability)
    {
        Ability newAbility = AbilityUtil.InstantiateAbility(ability);
        this.abilities.Add(newAbility);
    }
}
