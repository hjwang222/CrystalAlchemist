using UnityEngine;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Player/Teleport Stats")]
public class TeleportStats : ScriptableObject, ISerializationCallbackReceiver
{
    public string teleportName = "";
    public string scene;
    public Vector2 position;
    public bool showAnimationIn = false;
    public bool showAnimationOut = false;

    [AssetIcon]
    public Sprite icon;

    public void Clear()
    {
        this.teleportName = "";
        this.scene = null;
        this.position = Vector2.zero;
        this.showAnimationIn = true;
        this.showAnimationOut = false;
    }

    public TeleportStats(TeleportStats stat)
    {
        //Called from TeleportList and Savepoint
        this.name = stat.name;
        SetValue(stat);
    }

    public void SetValue(string teleportName, string targetScene, Vector2 position, bool showIn, bool showOut, Sprite icon)
    {
        if (targetScene != null && targetScene != "")
        {
            this.teleportName = teleportName;
            this.scene = targetScene;
            this.position = position;
            this.showAnimationIn = showIn;
            this.showAnimationOut = showOut;
            this.icon = icon;
        }
        else Clear();
    }

    public void SetValue(string targetScene, Vector2 position)
    {
        //Start new Game
        SetValue("",targetScene, position, true, false, null);
    }

    public void SetValue(TeleportStats stats)
    {
        //Death Screen and Constructor und LoadSystems
        SetValue(stats.teleportName, stats.scene, stats.position, true, true, stats.icon);
    }

    public void SetValue(string targetScene, Vector2 position, bool showIn, bool showOut)
    {
        //Scene Transition
        SetValue("", targetScene, position, showIn, showOut, null);
    }

    public string GetTeleportName()
    {
        return FormatUtil.GetLocalisedText(this.teleportName, LocalisationFileType.maps);
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}
