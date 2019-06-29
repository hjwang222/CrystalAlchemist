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
    public string targetScene;

    [Required]
    [Tooltip("Spawnpunkt des Spielers")]
    public Vector2 playerPositionInNewScene;

    [Required]
    public SimpleSignal vcamSignal;

    [Header("Fading")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    public void Awake()
    {
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(this.fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(LoadScene(other.GetComponent<Player>()));
            Debug.Log("End");
        }
    }

  
    private IEnumerator LoadScene(Player player)
    {
        yield return null;
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

