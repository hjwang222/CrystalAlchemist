using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateLocation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    [SerializeField]
    private TextMeshProUGUI timeField;

    [SerializeField]
    private TimeValue timeValue;

    [SerializeField]
    private GameObject sun;

    [SerializeField]
    private GameObject moon;

    private void Update()
    {
        showTime();
    }

    private void showTime()
    {
        int hour = this.timeValue.getHour();

        this.timeField.text = hour.ToString("00") + ":" + this.timeValue.getMinute().ToString("00");

        if (!this.timeValue.night && !sun.activeInHierarchy)
        {
            sun.SetActive(true);
            moon.SetActive(false);
        }
        else if (this.timeValue.night && !moon.activeInHierarchy)
        {
            sun.SetActive(false);
            moon.SetActive(true);
        }
    }

    /*
    [SerializeField]
    private TextMeshProUGUI positionField;

    private Player player;

    private void Start()
    {
        
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
