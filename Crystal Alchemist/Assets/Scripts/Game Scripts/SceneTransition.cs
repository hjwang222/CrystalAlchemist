using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneTransition : MonoBehaviour
{
    [Header("New Scene Variables")]
    [Tooltip("Name der nächsten Map")]
    public string targetScene;
    [Tooltip("Spawnpunkt des Spielers")]
    public Vector2 playerPositionInNewScene;

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
            other.GetComponent<Player>().currentState = CharacterState.idle;
            other.GetComponent<Player>().transform.position = playerPositionInNewScene;
            StartCoroutine(this.FadeCo(other.GetComponent<Player>()));      
        }
    }

    public IEnumerator FadeCo(Player player)
    {
        if (fadeOutPanel != null)
        {
            Instantiate(this.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(this.fadeWait);

        //if(player != null) player.SavePlayer();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(this.targetScene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}

