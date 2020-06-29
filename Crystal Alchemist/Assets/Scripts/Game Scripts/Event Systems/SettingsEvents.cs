using System;
using UnityEngine;

public class SettingsEvents : MonoBehaviour
{
    public static SettingsEvents current;

    private void Awake() => current = this;

    public Action OnLanguangeChanged;
    public Action OnLayoutChanged;
    public Action OnHUDChanged;
    public Action OnCameraChanged;

    public void DoHUDChange() => this.OnHUDChanged?.Invoke();
    public void DoLanguageChange() => this.OnLanguangeChanged?.Invoke();
    public void DoLayoutChange() => this.OnLayoutChanged?.Invoke();
    public void DoCameraChange() => this.OnCameraChanged?.Invoke();
}
