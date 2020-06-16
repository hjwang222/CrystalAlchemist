using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject mainObject;

    public void PlaySoundEffect(AudioClip audioClip) => AudioUtil.playSoundEffect(this.mainObject, audioClip);
    
    public void DestroyIt()
    {
        if (this.mainObject.GetComponent<Skill>() != null) this.mainObject.GetComponent<Skill>().DestroyIt();
        else if (this.mainObject.GetComponent<Character>() != null) this.mainObject.GetComponent<Character>().DestroyIt();
        else Destroy(this.mainObject);
    }

    public void EnableTreasure()
    {
        if (this.mainObject.GetComponent<Treasure>() != null) this.mainObject.GetComponent<Treasure>().SetEnabled(true);
    }

    public void DisableTreasure()
    {
        if (this.mainObject.GetComponent<Treasure>() != null) this.mainObject.GetComponent<Treasure>().SetEnabled(false);
    }

    public void ResetRotation() => this.transform.rotation = Quaternion.identity;

}
