using UnityEngine;

public class PlayerAnimationPart : MonoBehaviour
{
    public string directory;
    public string subFolder;
    public string fileName;

    public bool setSortOrder = false;
    public int sortOrder = 0;
    public bool isTail = false;

    public string getFullPath()
    {
        return this.directory + "/" + this.subFolder + "/" + this.fileName + ".png";
    }
}
