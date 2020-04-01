using UnityEngine;

public class TreasureAnimationHelper : MonoBehaviour
{
    [SerializeField]
    private Treasure treasure;

    public void EnableTreasure()
    {
        this.treasure.SetEnabled(true);
    }

    public void DisableTreasure()
    {
        this.treasure.SetEnabled(false);
    }
}
