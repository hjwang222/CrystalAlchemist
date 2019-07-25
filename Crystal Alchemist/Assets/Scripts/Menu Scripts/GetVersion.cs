using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetVersion : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI versionText;

    void Start()
    {
        this.versionText.text = "Version: "+Application.version + "\n(Pre-Alpha)";
    }

}
