using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private TeleportStats teleportStat;

    public void setLastTeleport(string targetScene, Vector2 position)
    {
        this.teleportStat.position = position;
        this.teleportStat.location = targetScene;

        foreach (Skill skill in player.skillSet)
        {
            skill.preLoad();
        }
    }

    public bool lastTeleportEnabled()
    {
        return this.teleportStat.getLastTeleport();
    }

    public void playTeleport(bool showAnimation)
    {
        StartCoroutine(LoadScene(this.player.fadingDuration.getValue(), showAnimation, false));
    }

    public void teleportPlayer(bool showAnimation)
    {
        StartCoroutine(LoadScene(this.player.fadingDuration.getValue(), showAnimation, true));
    }

    public void teleportPlayer(float duration, bool showAnimation)
    {
        StartCoroutine(LoadScene(duration, showAnimation, true));
    }

    private IEnumerator LoadScene(float duration, bool showAnimation, bool loadScene)
    {
        string targetScene = this.teleportStat.location;
        Vector2 position = this.teleportStat.position;

        this.player.myRigidbody.velocity = Vector2.zero;
        this.player.currentState = CharacterState.respawning;
        this.gameObject.GetComponent<PlayerAttacks>().deactivateAllSkills();
        this.player.enableSpriteRenderer(false);
        this.player.fadeSignal.Raise(false);

        if (loadScene) //Play De-Spawn
        {
            if (showAnimation && this.player.stats.respawnAnimation != null)
            {
                RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, this.transform.position, Quaternion.identity);
                respawnObject.setCharacter(this.player, true);
                respawnObject.setStatReset(false);
                yield return new WaitForSeconds(respawnObject.getAnimationLength());
            }
        
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(duration);

                    asyncOperation.allowSceneActivation = true;
                    StartCoroutine(positionCo(position, showAnimation)); //Play Spawn
                }
                yield return null;
            }
        }
        else StartCoroutine(positionCo(position, showAnimation)); //Play Spawn         
    }

    private IEnumerator positionCo(Vector2 playerPositionInNewScene, bool showAnimation)
    {
        this.transform.position = playerPositionInNewScene;

        if (showAnimation && this.player.stats.respawnAnimation != null)
        {
            yield return new WaitForSeconds(2f);

            RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, playerPositionInNewScene, Quaternion.identity);
            respawnObject.setCharacter(this.player);
            respawnObject.setStatReset(false);
            yield return new WaitForSeconds((respawnObject.getAnimationLength() + 1f));
        }
        else
        {
            //this.transform.position = playerPositionInNewScene;
            this.player.enableSpriteRenderer(true);
            this.player.currentState = CharacterState.idle;               
        }

        this.player.updateAnimDirection(this.player.direction);
    }
}
