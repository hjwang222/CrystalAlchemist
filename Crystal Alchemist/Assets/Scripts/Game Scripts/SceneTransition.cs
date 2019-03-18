using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("New Scene Variables")]
    [Tooltip("Name der nächsten Map")]
    public string targetScene;
    [Tooltip("Spawnpunkt des Spielers")]
    public Vector2 playerPosition;

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
            StartCoroutine(this.FadeCo());      
        }
    }

    public IEnumerator FadeCo()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(this.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(this.fadeWait);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(this.targetScene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}

