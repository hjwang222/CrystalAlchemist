using UnityEngine;

public class UISize : MonoBehaviour
{
    private void Start() => SettingsEvents.current.OnUISizeChanged += UpdateLayout;

    private void OnEnable() => UpdateLayout();

    private void OnDestroy() => SettingsEvents.current.OnUISizeChanged -= UpdateLayout;

    private void UpdateLayout() => this.transform.localScale = Vector2.one * MasterManager.settings.UISize;     
}
