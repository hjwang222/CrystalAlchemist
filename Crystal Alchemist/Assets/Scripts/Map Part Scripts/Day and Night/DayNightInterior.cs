using UnityEngine;

public class DayNightInterior : MonoBehaviour
{
    [SerializeField]
    private GameObject lightGameObject;

    private void Start() => switchInteriorLights();
    
    public void switchInteriorLights()
    {        
        if (MasterManager.timeValue.night) lightGameObject.SetActive(true);
        else lightGameObject.SetActive(false);
    }
}
