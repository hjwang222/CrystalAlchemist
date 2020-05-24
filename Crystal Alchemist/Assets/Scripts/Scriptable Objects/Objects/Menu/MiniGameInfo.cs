using UnityEngine;

[CreateAssetMenu(menuName = "Game/Menu/MiniGame Info")]
public class MiniGameInfo : ScriptableObject
{
    public MiniGameMatches matches;
    public MiniGameRound miniGameUI;

    public string GetName()
    {
        return FormatUtil.GetLocalisedText(this.name + "_Name", LocalisationFileType.minigames);
    }

    public string GetDescription()
    {
        return FormatUtil.GetLocalisedText(this.name + "_Description", LocalisationFileType.minigames);
    }
}
