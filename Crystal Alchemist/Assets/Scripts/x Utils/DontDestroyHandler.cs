using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyHandler : MonoBehaviour
{
    public List<GameObject> dontDestroy = new List<GameObject>();
    public static DontDestroyHandler Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            foreach (GameObject gameObject in this.dontDestroy)
            {
                GameObject temp = Instantiate(gameObject);
                temp.name = temp.name.Replace("(Clone)", "");
                DontDestroyOnLoad(temp);
            }
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }        
    }    
    
}
