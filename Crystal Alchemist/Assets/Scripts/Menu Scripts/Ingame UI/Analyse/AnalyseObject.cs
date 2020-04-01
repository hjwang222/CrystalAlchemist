using UnityEngine;

public class AnalyseObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer ImageObjectitemPreview;

    [SerializeField]
    private GameObject parent;

    private Treasure treasureChest;
    private Breakable breakable;

    public void Initialize(Breakable breakable)
    {
        this.breakable = breakable;
    }

    public void Initialize(Treasure treasure)
    {
        this.treasureChest = treasure;
    }

    private void LateUpdate()
    {
        showObjectInfo();
    }

    private void showObjectInfo()
    {
        if (this.treasureChest != null)
        {
            //Show Object Information
            if (treasureChest.itemDrop != null) Activate(treasureChest.itemDrop.stats);
            else Deactivate();            
        }
        else if (this.breakable != null)
        {
            //Show Object Information
            if (this.breakable.itemDrop != null
             && this.breakable.currentState != CharacterState.dead) Activate(this.breakable.itemDrop.stats);
            else Deactivate();
        }
    }

    private void Activate(ItemStats stats)
    {
        this.parent.SetActive(true);
        this.ImageObjectitemPreview.sprite = stats.getSprite();
    }

    private void Deactivate()
    {
        //Wenn Truhe geöffnet wurde oder Gegner tot ist
        this.parent.SetActive(false);
    }
}
