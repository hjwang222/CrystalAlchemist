using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTextFromSkill : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textField;

    [SerializeField]
    private Skill skill;

    // Update is called once per frame
    private void FixedUpdate()
    {
        this.textField.text = Utilities.Format.setDurationToString(skill.getDurationLeft());
    }
}
