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

    [SerializeField]
    private FloatValue fadeDuration;

    [SerializeField]
    private SimpleSignal fadeSignal;

    public void Initialize(Player player)
    {
        this.player = player;
        this.player.SpawnOut(); //Disable Player

        if (this.nextTeleport.showAnimationIn) StartCoroutine(materializePlayer()); //Start Animation       
        else
        {
            SetPosition(this.nextTeleport.position);
            this.player.SpawnIn(false);
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
        //SceneLoader.Load(this.nextTeleport.scene);
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
            respawnObject.SpawnOut(this.player);  //reverse
            yield return new WaitForSeconds(respawnObject.getAnimationLength());
        }

        LoadScene();
    }

    private IEnumerator materializePlayer()
    {
        Vector2 position = this.nextTeleport.position;
        SetPosition(position);
        this.player.SpawnIn(true);

        if (this.player.stats.respawnAnimation != null)
        {
            yield return new WaitForSeconds(2f);

            RespawnAnimation respawnObject = Instantiate(this.player.stats.respawnAnimation, position, Quaternion.identity);
            respawnObject.SpawnIn(this.player);
            yield return new WaitForSeconds((respawnObject.getAnimationLength() + 1f));
        }
        else this.player.SpawnCompleted();        
    }

    private IEnumerator loadSceneCo(string targetScene)
    {
        this.fadeSignal.Raise();
        yield return new WaitForSeconds(this.fadeDuration.getValue());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f) asyncOperation.allowSceneActivation = true;            
            yield return null;
        }
    }
}
