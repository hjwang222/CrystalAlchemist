using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageHandler : MonoBehaviour
{
    private GameObject background;

    public void setGameObject(GameObject gameObject)
    {
        if (this.background != null) Destroy(this.background);
        if (gameObject != null) this.background = Instantiate(gameObject, this.transform.parent);
    }
}
