using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string UI;

    [SerializeField]
    private FloatValue timePlayed;

    private void Awake()
    {
        SceneManager.LoadScene(UI, LoadSceneMode.Additive);            
    }

    private void OnDestroy()
    {
        this.timePlayed.SetValue(this.timePlayed.GetValue()+Time.timeSinceLevelLoad);
    }
}
