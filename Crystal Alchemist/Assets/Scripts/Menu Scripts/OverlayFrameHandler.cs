using UnityEngine;

public class OverlayFrameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject blackScreen;

    private void Start() => GameEvents.current.OnMenuOverlay += BlackScreenActive;
    
    private void OnDestroy() => GameEvents.current.OnMenuOverlay += BlackScreenActive;
    
    private void BlackScreenActive(bool value) => this.blackScreen.SetActive(value);    
}
