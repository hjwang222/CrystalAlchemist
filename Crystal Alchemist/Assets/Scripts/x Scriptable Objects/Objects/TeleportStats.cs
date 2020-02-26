using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Teleport")]

public class TeleportStats : ScriptableObject, ISerializationCallbackReceiver
{
    public string location;
    public Vector2 position;

    public bool getLastTeleport()
    {
        if (this.location != null && this.position != null) return true;
        else return false;
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}
