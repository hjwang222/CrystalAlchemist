using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateLocation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    /*
    [SerializeField]
    private TextMeshProUGUI positionField;

    private Player player;

    private void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    
    private void Update()
    {
        this.positionField.text = "X:"+ Mathf.RoundToInt(this.player.transform.position.x)+" Y:"+Mathf.RoundToInt(this.player.transform.position.y);
    }
    */

    public void updateLocationText(string text)
    {
        this.textField.text = text;
    }

    

}
