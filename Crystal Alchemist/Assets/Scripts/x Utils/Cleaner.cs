using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class Cleaner : MonoBehaviour
{
    [Button]
    public void CleanUp()
    {
        List<GameObject> test = new List<GameObject>();
        List<SignalListener> result = new List<SignalListener>();
        UnityUtil.GetChildren(this.transform, test);

        foreach(GameObject temp in test)
        {
            SignalListener blub = temp.GetComponent<SignalListener>();
            if (blub != null && blub.signal == null) result.Add(blub);
        }

        foreach(SignalListener temp in result)
        {
            Debug.Log(temp.gameObject+" - "+temp.transform.parent.gameObject);
            DestroyImmediate(temp);
        }
    }
}
