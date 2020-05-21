using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorPart : MonoBehaviour
{
    [InfoBox("Neccessary to set for Character Creation", InfoMessageType.Info)]
    public CharacterCreatorPartProperty property;

    [SerializeField]
    private bool isPreview = false;

    [ShowIf("isPreview", true)]
    public List<Race> restrictedRaces = new List<Race>();

    [ShowIf("isPreview", true)]
    public string previewDirection = "Down";
}
