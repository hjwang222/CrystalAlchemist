using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake() => Initialize();

    private void Initialize()
    {
        current = this;
        SaveSystem.loadOptions();
    }

    public Action OnSubmit;
    public Action OnCancel;
    public Action<ItemStats> OnCollect;
    public Action<Costs> OnReduce;
    public Action<int> OnPage;
    public Action<CharacterState> OnMenuOpen;
    public Action<CharacterState> OnMenuClose;
    public Action<bool> OnMenuOverlay;
    public Action<StatusEffect> OnEffectAdded;

    public void DoEffectAdded(StatusEffect effect) => this.OnEffectAdded?.Invoke(effect);  
    public void DoMenuOpen(CharacterState state) => this.OnMenuOpen?.Invoke(state);  
    public void DoMenuClose(CharacterState state) => this.OnMenuClose?.Invoke(state);
    public void DoMenuOverlay(bool value) => this.OnMenuOverlay?.Invoke(value);
    public void DoCollect(ItemStats stats) => this.OnCollect?.Invoke(stats);    
    public void DoReduce(Costs costs) => this.OnReduce?.Invoke(costs);    
    public void DoSubmit() => this.OnSubmit?.Invoke();  
    public void DoCancel() => this.OnCancel?.Invoke();
    public void DoPage(int page) => this.OnPage?.Invoke(page);    
}
