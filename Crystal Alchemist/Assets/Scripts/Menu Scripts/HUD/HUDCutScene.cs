using System.Collections.Generic;
using UnityEngine;

public class HUDCutScene : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject>();

    void Start() => GameEvents.current.OnCutScene += this.SetCutScene;    

    private void OnDestroy() => GameEvents.current.OnCutScene += this.SetCutScene;    

    private void SetCutScene(bool value)
    {
        foreach(GameObject gameObj in this.gameObjects) gameObj.SetActive(!value);        
    }
}
