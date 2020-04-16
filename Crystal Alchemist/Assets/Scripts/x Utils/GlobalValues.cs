using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings/Game Static Values")]
public class GlobalValues : ScriptableObject
{
    public Color[] red = new Color[] { new Color32(255, 0, 0, 255) , new Color32(125, 0, 0, 255) };
    public Color[] blue = new Color[] { new Color32(0, 125, 255, 255), new Color32(0, 25, 125, 255) };
    public Color[] green = new Color[] { new Color32(0, 255, 0, 255), new Color32(0, 125, 0, 255) };

    public float playerDelay = 0.1f;
    public float transitionDuration = 1f;
    public string saveGameFiletype = "dat";
    public Vector3 nullVector = new Vector3(0, 0, 999);
}
