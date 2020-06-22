using AssetIcons;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/Item Drop")]
public class ItemDrop : ScriptableObject
{
    [SerializeField]
    public ItemStats stats;

    [SerializeField]
    public Collectable collectable;

    [AssetIcon]
    private Sprite GetSprite()
    {
        if(this.stats != null) return stats.getSprite();
        return null;
    }

    public ItemDrop Instantiate(int amount)
    {
        ItemDrop clone = Instantiate(this);
        clone.name = this.name;
        clone.Initialize(amount); //Set correct stats name for unique items
        return clone;
    }

    public void Initialize(int amount)
    {
        ItemStats temp = Instantiate(this.stats);
        temp.name = this.name;
        temp.Initialize(amount);
        this.stats = temp;
    }

    public Collectable Instantiate(Vector2 position)
    {
        Collectable temp = Instantiate(this.collectable, position, Quaternion.identity);
        temp.name = this.name;
        temp.SetItem(this);
        return temp;
    }

    public Collectable Instantiate(Vector2 position, bool bounce)
    {
        Collectable temp = Instantiate(this.collectable, position, Quaternion.identity);
        temp.SetBounce(bounce);
        temp.name = this.name;
        temp.SetItem(this);
        temp.SetSelfDestruction();
        return temp;
    }
}