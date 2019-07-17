using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private GameObject combat;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject interaction;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        showButtons();
    }

    private void showButtons()
    {        
        if (this.player != null 
            && this.player.currentState == CharacterState.inMenu)
        {
            if (!this.menu.activeInHierarchy)
            {
                this.combat.SetActive(false);
                this.interaction.SetActive(false);
                this.menu.SetActive(true);
            }
        }
        else if (this.player != null 
            && this.player.currentState == CharacterState.inDialog)
        {
            this.combat.SetActive(false);
            this.interaction.SetActive(false);
            this.menu.SetActive(false);
        }
        else if (this.player != null 
            && this.player.currentState == CharacterState.interact)
        {
            if (!this.interaction.activeInHierarchy)
            {
                this.combat.SetActive(false);
                this.interaction.SetActive(true);
                this.menu.SetActive(false);
            }
        }
        else 
        {
            if (!this.combat.activeInHierarchy)
            {
                this.combat.SetActive(true);
                this.interaction.SetActive(false);
                this.menu.SetActive(false);
            }
        }
    }
}
