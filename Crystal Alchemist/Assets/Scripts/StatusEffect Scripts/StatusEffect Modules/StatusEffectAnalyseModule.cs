using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectAnalyseModule : StatusEffectModule
{
    private List<GameObject> gameObjectApplied = new List<GameObject>();
    private List<GameObject> analyseAdded = new List<GameObject>();

    [SerializeField]
    private GameObject analyseGameObject;

    public override void doAction()
    {   
        if (this.analyseGameObject != null)
        {
            List<GameObject> targets = new List<GameObject>();
            targets.AddRange(GameObject.FindGameObjectsWithTag("Object"));
            targets.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

            foreach (GameObject gameObject in targets)
            {
                if (!this.analyseAdded.Contains(gameObject) 
                    && (gameObject.GetComponent<AI>() != null 
                    || (gameObject.GetComponent<Breakable>() != null && gameObject.GetComponent<Breakable>().itemDrop != null)
                    || (gameObject.GetComponent<Treasure>() != null && gameObject.GetComponent<Treasure>().itemDrop != null))) //check if already added
                {
                    GameObject tmp = Instantiate(this.analyseGameObject, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                    tmp.GetComponent<AnalyseUI>().setTarget(gameObject);
                    this.gameObjectApplied.Add(tmp);
                    this.analyseAdded.Add(gameObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < this.gameObjectApplied.Count; i++)
        {
            Destroy(this.gameObjectApplied[i]);
        }

        this.gameObjectApplied.Clear();
        this.analyseAdded.Clear();
    }
}
