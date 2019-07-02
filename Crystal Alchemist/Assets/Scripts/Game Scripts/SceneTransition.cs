using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Sirenix.OdinInspector;

public class SceneTransition : MonoBehaviour
{

    [Header("New Scene Variables")]
    [Tooltip("Name der nächsten Map")]
    [Required]
    [SerializeField]
    private string targetScene;

    [Tooltip("Spawnpunkt des Spielers")]
    [SerializeField]
    private Vector2 playerPositionInNewScene;

    [Required]
    [SerializeField]
    private SimpleSignal vcamSignal;

    [Required]
    [SerializeField]
    private BoolSignal fadeSignal;


    public void Awake()
    {
        this.fadeSignal.Raise(true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(LoadScene(other.GetComponent<Player>()));
        }
    }

  
    private IEnumerator LoadScene(Player player)
    {
        this.fadeSignal.Raise(false);
        yield return new WaitForSeconds(2f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(this.targetScene);
        asyncOperation.allowSceneActivation = false;
        this.vcamSignal.Raise();


        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {                
                asyncOperation.allowSceneActivation = true;
                player.setNewPosition(this.playerPositionInNewScene);
            }
            yield return null;
        }      
    }


    /*
    public IEnumerator FadeCo(Player player)
    {   
        if (fadeOutPanel != null)
        {
            Instantiate(this.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(this.fadeWait);      

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }*/
}

