using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/Item Drop")]
public class ItemDrop : ScriptableObject
{
    [SerializeField]
    public ItemStats stats;

    [SerializeField]
    public Collectable collectable;

    public void Initialize(int amount)
    {
        ItemStats temp = Instantiate(this.stats);
        temp.name = this.stats.name;
        this.stats = temp;
        this.stats.Initialize(amount);
        this.collectable.SetItem(stats);
    }

    public Collectable Instantiate(Vector2 position)
    {
        Collectable temp = Instantiate(this.collectable, position, Quaternion.identity);        
        temp.SetItem(this.stats);
        return temp;
    }
}