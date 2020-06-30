using System.Collections.Generic;
using UnityEngine;

public class HUDCutScene : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject>();

    [SerializeField]
    private BoolValue CutSceneValue;

    private void Start()
    {
        GameEvents.current.OnCutScene += this.SetCutScene;
        SetCutScene();
    }

    private void OnDestroy() => GameEvents.current.OnCutScene += this.SetCutScene;    

    private void SetCutScene()
    {
        foreach(GameObject gameObj in this.gameObjects) gameObj.SetActive(!this.CutSceneValue.GetValue());        
    }
}
