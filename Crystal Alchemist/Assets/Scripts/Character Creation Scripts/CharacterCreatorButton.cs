using UnityEngine.UI;
using UnityEngine;

public class CharacterCreatorButton : MonoBehaviour
{
    public CharacterPreset creatorPreset;

    [SerializeField]
    private CharacterPresetSignal signal;

    public virtual void Click()
    {
        this.signal.Raise(this.creatorPreset);
    }


}
