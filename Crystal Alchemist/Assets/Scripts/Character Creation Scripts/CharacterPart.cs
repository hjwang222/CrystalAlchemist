using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class CharacterPart : MonoBehaviour
{
    public List<Race> restrictedRaces = new List<Race>();
    public ColorGroup colorGroup;
    public bool useName = true;
    public bool dyeable = true;
    public bool isEarHorn = false;
    public string assetPath;
    public bool savePreview = false;
}
