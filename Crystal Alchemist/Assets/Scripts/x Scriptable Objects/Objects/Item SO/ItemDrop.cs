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
    [SerializeField]
    private Sprite icon;

    public void Initialize(int amount)
    {
        ItemStats temp = Instantiate(this.stats);
        temp.name = this.stats.name;
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
}