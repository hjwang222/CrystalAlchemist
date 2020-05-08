using System.Collections.Generic;
using UnityEngine;

public static class GameUtil
{
    public static void setPreset(CharacterPreset source, CharacterPreset target)
    {
        target.setRace(source.getRace());
        target.characterName = source.characterName;
        target.AddColorGroupRange(source.GetColorGroupRange());
        target.AddCharacterPartDataRange(source.GetCharacterPartDataRange());
    }    

    public static float setResource(float resource, float max, float addResource)
    {
        if (addResource != 0)
        {
            if (resource + addResource > max) addResource = max - resource;
            else if (resource + addResource < 0) resource = 0;

            resource += addResource;
        }

        return resource;
    }
}

