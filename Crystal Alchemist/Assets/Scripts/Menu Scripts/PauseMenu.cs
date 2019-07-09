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

    [SerializeField]
    private GameObject blackScreen;

    [SerializeField]
    private GameObject parentMenue;

    [SerializeField]
    private GameObject childMenue;

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

        this.parentMenue.SetActive(true);
        this.childMenue.SetActive(false);

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
        this.blackScreen.SetActive(false);
    }

    public void showControls()
    {
        this.parentMenue.SetActive(false);
        this.childMenue.SetActive(true);
    }
}
