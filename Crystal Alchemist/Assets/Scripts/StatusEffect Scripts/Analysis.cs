using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analysis : Script
{
    public GameObject specialGameObject;
    private List<GameObject> gameObjectApplied = new List<GameObject>();
    
    public override void onDestroy()
    {
        for (int i = 0; i < this.gameObjectApplied.Count; i++)
        {
            Destroy(this.gameObjectApplied[i]);
        }
        this.gameObjectApplied.Clear();
    }

    public override void onExit(Collider2D hittedCharacter)
    {

    }

    public override void onStay(Collider2D hittedCharacter)
    {

    }

    public override void onInitialize()
    {
        if (this.specialGameObject != null)
        {
            //TODO: Bug, dass es doppelt hinzugefügt wird
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            this.gameObjectApplied.Clear();

            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject tmp = Instantiate(this.specialGameObject, enemies[i].transform.position, Quaternion.identity, enemies[i].transform);
                tmp.GetComponent<AnalyseUI>().target = enemies[i];
                tmp.hideFlags = HideFlags.HideInHierarchy;
                this.gameObjectApplied.Add(tmp);
            }

            for (int i = 0; i < objects.Length; i++)
            {
                GameObject tmp = Instantiate(this.specialGameObject, objects[i].transform.position, Quaternion.identity, objects[i].transform);
                tmp.GetComponent<AnalyseUI>().target = objects[i];
                tmp.hideFlags = HideFlags.HideInHierarchy;
                this.gameObjectApplied.Add(tmp);
            }
        }
    }

    public override void onUpdate()
    {
        
    }

    public override void onEnter(Collider2D hittedCharacter)
    {

    }
}
