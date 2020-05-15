using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string UI;

    [SerializeField]
    private FloatValue timePlayed;

    //Event System here

    private void Awake()
    {
        SceneManager.LoadScene(UI, LoadSceneMode.Additive);            
    }

    private void OnDestroy()
    {
        this.timePlayed.setValue(this.timePlayed.getValue()+Time.timeSinceLevelLoad);
    }
}
