using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game/Player/Game Progress")]
public class PlayerGameProgress : ScriptableObject
{
    [SerializeField]
    private List<string> permanentProgress = new List<string>();

    [SerializeField]
    private List<string> temporaryProgress = new List<string>();

    public void Initialize()
    {
        permanentProgress.RemoveAll(item => item == null);
        temporaryProgress.RemoveAll(item => item == null);
    }

    public void Clear()
    {
        permanentProgress.Clear();
        temporaryProgress.Clear();
    }

    public void AddProgress(string key, bool isPermanent)
    {
        if(!Contains(key, isPermanent))
        {
            if (isPermanent) permanentProgress.Add(GetKey(key));
            else temporaryProgress.Add(GetKey(key));
        }
    }

    public List<string> GetPermanent()
    {
        return this.permanentProgress;
    }

    public void SetPermanent(List<string> value)
    {
        if (value != null) this.permanentProgress.AddRange(value);
    }

    public bool Contains(string key, bool isPermanent)
    {
        string savedKey = GetKey(key);
        if (isPermanent && this.permanentProgress.Contains(savedKey)) return true;
        else if (!isPermanent && this.temporaryProgress.Contains(savedKey)) return true;
        return false;
    }

    private string GetKey(string key)
    {
        return SceneManager.GetActiveScene().name + " " + key;
    }

    public int GetAmount()
    {
        return this.permanentProgress.Count + this.temporaryProgress.Count;
    }

}
