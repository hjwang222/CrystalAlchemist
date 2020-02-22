using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private Vector2 lastSaveGamePosition;
    private string lastSaveGameScene;

    public void setLastTeleport(string targetScene, Vector2 position)
    {
        this.lastSaveGamePosition = position;
        this.lastSaveGameScene = targetScene;

        foreach (Skill skill in player.skillSet)
        {
            skill.preLoad();
        }
    }

    public bool getLastTeleport()
    {
        return getLastTeleport(out string scene, out Vector2 position);
    }

    public bool getLastTeleport(out string scene, out Vector2 position)
    {
        scene = this.lastSaveGameScene;
        position = this.lastSaveGamePosition;

        if (scene != null && position != null) return true;
        else return false;
    }

    public void teleportPlayer(string targetScene, Vector2 position, bool showAnimation)
    {
        StartCoroutine(LoadScene(targetScene, position, this.player.fadingDuration.getValue(), showAnimation));
    }

    public void teleportPlayer(string targetScene, Vector2 position, float duration, bool showAnimation)
    {
        StartCoroutine(LoadScene(targetScene, position, duration, showAnimation));
    }

    private IEnumerator LoadScene(string targetScene, Vector2 position, float duration, bool showAnimation)
    {
        this.player.currentState = CharacterState.respawning;
        this.gameObject.GetComponent<PlayerAttacks>().deactivateAllSkills();

        if (showAnimation && this.player.stats.respawnAnimation != null)
        {
            RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, this.transform.position, Quaternion.identity);
            respawnObject.setCharacter(this.player, true);
            respawnObject.setStatReset(false);
            yield return new WaitForSeconds(respawnObject.getAnimationLength());
            this.player.enableSpriteRenderer(false);
            //yield return new WaitForSeconds(2f);
        }
        else
        {
            this.player.enableSpriteRenderer(false);
        }

        this.player.fadeSignal.Raise(false);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(duration);

                asyncOperation.allowSceneActivation = true;
                StartCoroutine(positionCo(position, showAnimation));
            }
            yield return null;
        }
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
