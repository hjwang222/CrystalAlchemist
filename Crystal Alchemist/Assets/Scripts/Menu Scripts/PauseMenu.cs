using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private float delay = 0.3f;
    private Player player;
    [SerializeField]
    private GameObject cursor;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause")) exitMenu();
    }

    private void OnEnable()
    {
        this.cursor.SetActive(true);
        this.player.currentState = CharacterState.inDialog;
    }

    private void OnDisable()
    {
        this.cursor.SetActive(false);
    }

    public void exitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void exitMenu()
    {
        this.player.delay(this.delay);
        this.transform.parent.gameObject.SetActive(false);
    }
}
