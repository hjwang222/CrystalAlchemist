using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private TeleportStats teleportStat;

    public TeleportStats lastTeleport;

    public void setLastTeleport(string targetScene, Vector2 position, bool last)
    {
        this.lastTeleport.position = position;
        this.lastTeleport.location = targetScene;
        this.lastTeleport.lastTeleportSet = last;

        foreach (Skill skill in player.skillSet)
        {
            skill.preLoad();
        }
    }

    public bool lastTeleportEnabled()
    {
        return this.lastTeleport.lastTeleportSet;
    }

    public void teleportPlayer(bool showAnimationOut, bool showAnimationIn, bool loadScene) //LoadConfig
    {
        StartCoroutine(DematerializePlayer(this.player.fadingDuration.getValue(), showAnimationIn, showAnimationOut, loadScene, this.teleportStat));
    }

    public void teleportPlayerLast(bool showAnimationOut, bool showAnimationIn, bool loadScene) //LoadConfig
    {
        StartCoroutine(DematerializePlayer(this.player.fadingDuration.getValue(), showAnimationIn, showAnimationOut, loadScene, this.lastTeleport));
    }

    public void teleportPlayerLast() //Skill Teleport
    {
        StartCoroutine(DematerializePlayer(this.player.fadingDuration.getValue(), true, true, true, this.lastTeleport));
    }

    public void teleportPlayerLast(bool showAnimationOut, bool showAnimationIn) //Death Screen
    {
        StartCoroutine(DematerializePlayer(this.player.fadingDuration.getValue(), showAnimationIn, showAnimationOut, true, this.lastTeleport));
    }

    public void teleportPlayer(float duration, bool showAnimationOut, bool showAnimationIn) //Scene Transition
    {
        StartCoroutine(DematerializePlayer(duration, showAnimationIn, showAnimationOut, true, this.teleportStat));
    }

    private IEnumerator DematerializePlayer(float duration, bool showAnimationIn, bool showAnimationOut, bool loadScene, TeleportStats stat)
    {
        string targetScene = stat.location;
        Vector2 position = stat.position;

        this.player.prepareSpawnOut(); //Player disable

        if (showAnimationOut && this.player.stats.respawnAnimation != null) //Show Animation for DEspawn
        {
            RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, this.transform.position, Quaternion.identity);
            respawnObject.setCharacter(this.player, true, false);  //reverse
            yield return new WaitForSeconds(respawnObject.getAnimationLength());
        }

        if (loadScene) StartCoroutine(loadSceneCo(position, targetScene, duration, showAnimationIn)); //load new scene    
        else StartCoroutine(materializePlayer(position, showAnimationIn)); //Play Spawn  
    }

    private IEnumerator loadSceneCo(Vector2 position, string targetScene, float duration, bool showAnimationIn)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(duration);

                asyncOperation.allowSceneActivation = true;
                StartCoroutine(materializePlayer(position, showAnimationIn)); //Play Spawn
            }
            yield return null;
        }
    }

    private IEnumerator materializePlayer(Vector2 position, bool showAnimationIn)
    {
        this.player.transform.position = position;

        if (showAnimationIn && this.player.stats.respawnAnimation != null)
        {
            yield return new WaitForSeconds(2f);

            RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, position, Quaternion.identity);
            respawnObject.setCharacter(this.player, false, false);
            yield return new WaitForSeconds((respawnObject.getAnimationLength() + 1f));
        }
        else
        {
            this.player.completeSpawnFromAnimation();
        }

        this.player.updateAnimDirection(this.player.direction);
    }
}
