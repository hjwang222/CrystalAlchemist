using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorSubMenu : MonoBehaviour
{
    [Required]
    [SerializeField]
    private CharacterCreatorMenu mainMenu;

    [SerializeField]
    private List<CharacterCreatorSubMenuChild> subMenu = new List<CharacterCreatorSubMenuChild>();

    public void Start()
    {
        this.ShobSubMenu();
    }

    public void OnEnable()
    {
        this.ShobSubMenu();
    }

    public void OnDisable()
    {
        this.ShobSubMenu();
    }

    public void ShobSubMenu()
    {
        foreach(CharacterCreatorSubMenuChild child in this.subMenu)
        {
            child.gameObject.SetActive(false);

            if (child.isEnabledByRace(this.mainMenu.creatorPreset.getRace())) child.gameObject.SetActive(true);
            //if (child.isEnabledByGear()) child.gameObject.SetActive(true);
        }
    }
}
