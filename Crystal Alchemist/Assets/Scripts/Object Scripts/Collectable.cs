using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using UnityEditor;

public class Collectable : MonoBehaviour
{
    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private SpriteRenderer shadowRenderer;

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private Animator anim;

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private Item loot;

    #region Start Funktionen

    private void Start()
    {
        //Check if keyItem already in Inventory
        if (this.loot.alreadyThere()) Destroy(this.gameObject);
    }

    #endregion

    public void playSounds()
    {
        CustomUtilities.Audio.playSoundEffect(this.gameObject, this.loot.getSoundEffect());        
    }

    #region Collect Item Funktionen

    public void OnTriggerEnter2D(Collider2D character)
    {
        if (!character.isTrigger)
        {
            Player player = character.GetComponent<Player>();
            if (player != null)
            {
                player.GetComponent<PlayerUtils>().CollectItem(this.loot);
                DestroyIt();
            }
        }
    }   

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
        #endregion
}
