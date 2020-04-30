using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleport : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private TeleportStats nextTeleport;

    [SerializeField]
    private PlayerTeleportList teleportList;

    public void Initialize(Player player)
    {
        this.player = player;
        this.player.SpawnOut(); //Disable Player

        if (this.nextTeleport.showAnimationIn) StartCoroutine(materializePlayer()); //Start Animation       
        else
        {
            this.player.SpawnWithAnimationCompleted();
            SetPosition(this.nextTeleport.position);
        }
    }

    public void SwitchScene()
    {
        this.player.SpawnOut(); //Disable Player
        if (this.nextTeleport.showAnimationOut) StartCoroutine(DematerializePlayer());       
        else LoadScene();
    }

    public bool TeleportAbilityEnabled()
    {
        return this.teleportList.TeleportEnabled();
    }

    public void AddTeleport(string scene, Vector2 position)
    {
        this.teleportList.AddTeleport(scene, position);
    }

    private void LoadScene()
    {
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(this.nextTeleport.scene);
        //asyncOperation.allowSceneActivation = true;
        StartCoroutine(loadSceneCo(this.nextTeleport.scene));
    }

    private void SetPosition(Vector2 position)
    {
        this.player.transform.position = position;
        this.player.UpdateAnimator(this.player.values.direction);
    }

    private IEnumerator DematerializePlayer()
    {
        if (this.player.stats.respawnAnimation != null) //Show Animation for DEspawn
        {
            RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, this.transform.position, Quaternion.identity);
            respawnObject.setCharacter(this.player, true, false);  //reverse
            yield return new WaitForSeconds(respawnObject.getAnimationLength());
        }

        LoadScene();
    }

    private IEnumerator materializePlayer()
    {
        Vector2 position = this.nextTeleport.position;
        SetPosition(position);

        if (this.player.stats.respawnAnimation != null)
        {
            yield return new WaitForSeconds(2f);

            RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, position, Quaternion.identity);
            respawnObject.setCharacter(this.player, false, false);
            yield return new WaitForSeconds((respawnObject.getAnimationLength() + 1f));
        }
        else this.player.SpawnWithAnimationCompleted();        
    }

    private IEnumerator loadSceneCo(string targetScene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(this.player.fadingDuration.getValue());
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
