using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string UI;

    [SerializeField]
    private string Menues;

    [SerializeField]
    private GameObject blackScreen;

    [SerializeField]
    private FloatValue timePlayed;

    private void OnEnable()
    {
        this.blackScreen.SetActive(true);
    }

    private void Awake()
    {
        SceneManager.LoadScene(UI, LoadSceneMode.Additive);
        SceneManager.LoadScene(Menues, LoadSceneMode.Additive);
        Destroy(this.blackScreen, 0.1f);
    }

    private void OnDestroy()
    {
        this.timePlayed.setValue(this.timePlayed.getValue()+Time.timeSinceLevelLoad);
    }    
}
