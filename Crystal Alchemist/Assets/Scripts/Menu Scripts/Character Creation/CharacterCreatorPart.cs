using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorPart : MonoBehaviour
{    
    public CharacterCreatorPartProperty property;

    [SerializeField]
    private bool isPreview = false;

    [ShowIf("isPreview", true)]
    public List<Race> restrictedRaces = new List<Race>();

    [ShowIf("isPreview", true)]
    public string previewDirection = "Down";


}
