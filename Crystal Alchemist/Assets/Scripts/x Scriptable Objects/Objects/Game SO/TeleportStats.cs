using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Teleport Stats")]
public class TeleportStats : ScriptableObject, ISerializationCallbackReceiver
{
    public string teleportName = "";
    public string teleportNameEnglish = "";
    public string scene;
    public Vector2 position;
    public bool showAnimationIn = false;
    public bool showAnimationOut = false;

    public void Clear()
    {
        this.teleportName = "";
        this.teleportNameEnglish = "";
        this.scene = "Haus";
        this.position = Vector2.zero;
        this.showAnimationIn = true;
        this.showAnimationOut = false;
    }

    public TeleportStats(string teleportName, string targetScene, Vector2 position)
    {
        //Called from Loadsystem
        SetValue(teleportName, targetScene, position, true, true);
    }

    public TeleportStats(TeleportStats stat)
    {
        //Called from TeleportList and Savepoint
        this.name = stat.name;
        SetValue(stat);
    }

    public void SetValue(string teleportName, string targetScene, Vector2 position, bool showIn, bool showOut)
    {
        if (targetScene != null && targetScene != "")
        {
            this.teleportName = teleportName;
            this.scene = targetScene;
            this.position = position;
            this.showAnimationIn = showIn;
            this.showAnimationOut = showOut;
        }
        else Clear();
    }

    public void SetValue(string targetScene, float[] array)
    {
        //Start Point (Load)
        Vector2 position = Vector2.zero;
        if (array != null && array.Length >= 2) position = new Vector2(array[0], array[1]);
        SetValue("",targetScene, position, true, false);
    }

    public void SetValue(string targetScene, Vector2 position)
    {
        //Start new Game
        SetValue("",targetScene, position, true, false);
    }

    public void SetValue(TeleportStats stats)
    {
        //Death Screen and Constructor
        SetValue(stats.teleportName, stats.scene, stats.position, true, true);
    }

    public void SetValue(string targetScene, Vector2 position, bool showIn, bool showOut)
    {
        //Scene Transition
        SetValue("", targetScene, position, showIn, showOut);
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}
