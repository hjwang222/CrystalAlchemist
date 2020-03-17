using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Teleport")]

public class TeleportStats : ScriptableObject, ISerializationCallbackReceiver
{
    public string location;
    public Vector2 position;
    public bool lastTeleportSet = false;

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}
