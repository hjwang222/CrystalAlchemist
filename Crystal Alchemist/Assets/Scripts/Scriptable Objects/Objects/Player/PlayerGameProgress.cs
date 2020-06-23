using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game/Player/Game Progress")]
public class PlayerGameProgress : ScriptableObject
{
    [SerializeField]
    private List<string> keys = new List<string>();

    public void Initialize()
    {
        keys.RemoveAll(item => item == null);
    }

    public void Clear() => keys.Clear();

    public void Add(string key)
    {
        string newKey = GetKey(key);
        if (!keys.Contains(newKey)) keys.Add(newKey);
    }

    public List<string> Get()
    {
        return this.keys;
    }

    public void Set(List<string> value) => this.keys.AddRange(value);

    public bool Contains(string key)
    {
        return this.keys.Contains(GetKey(key));
    }

    private string GetKey(string key)
    {
        return SceneManager.GetActiveScene().name + " " + key;
    }
}
