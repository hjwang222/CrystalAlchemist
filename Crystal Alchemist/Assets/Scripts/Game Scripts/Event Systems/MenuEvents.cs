﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class MenuEvents : MonoBehaviour
{
    public static MenuEvents current;    

    private void Awake() => current = this;
    
    public Action OnInventory;
    public Action OnSkills;
    public Action OnMap;
    public Action OnAttributes;
    public Action OnPause;
    public Action OnDeath;
    public Action OnEditor;
    public Action OnSave;
    public Action<MiniGame> OnMiniGame;
    public Action<Ability> OnAbilitySelected;
    public Func<Ability> OnAbilitySet;
    public Action OnDialogBox;
    public Action OnMenuDialogBox;

    public void OpenMenuDialogBox() => this.OnMenuDialogBox?.Invoke();
    public void OpenDialogBox() => this.OnDialogBox?.Invoke();
    public void OpenInventory() => this.OnInventory?.Invoke();
    public void OpenSkillBook() => this.OnSkills?.Invoke();
    public void OpenMap() => this.OnMap?.Invoke();
    public void OpenAttributes() => this.OnAttributes?.Invoke();
    public void OpenPause() => this.OnPause?.Invoke();
    public void OpenDeath() => this.OnDeath?.Invoke();
    public void OpenCharacterCreation() => this.OnEditor?.Invoke();
    public void OpenSavepoint() => this.OnSave?.Invoke();
    public void OpenMiniGame(MiniGame minigame) => this.OnMiniGame?.Invoke(minigame);
    public void SelectAbility(Ability ability) => this.OnAbilitySelected?.Invoke(ability);
    public Ability SetAbility() => this.OnAbilitySet?.Invoke();
}