using TMPro;
using UnityEngine;

public class AnalyseEnemy : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro TMPcharacterName;
    [SerializeField]
    private SpriteFillBar lifeBar;
    [SerializeField]
    private SpriteRenderer ImageitemPreview;
    [SerializeField]
    private StatusEffectBar statusEffectBar;

    private AI npc;

    public void Initialize(AI enemy)
    {
        //set type of Analyse
        this.npc = enemy;
    }

    private void LateUpdate()
    {
        showEnemyInfo();
    }

    private void showEnemyInfo()
    {
        this.ImageitemPreview.gameObject.SetActive(false);

        this.TMPcharacterName.text = this.npc.stats.GetCharacterName();
        this.lifeBar.fillAmount((this.npc.values.life / this.npc.values.maxLife));
        this.statusEffectBar.setCharacter(this.npc.values);

        if (this.npc.values.itemDrop != null 
         && this.npc.values.currentState != CharacterState.dead
         && this.npc.values.currentState != CharacterState.respawning)
        {
            this.ImageitemPreview.gameObject.SetActive(true);
            this.ImageitemPreview.sprite = this.npc.values.itemDrop.stats.getSprite(); //TODONEW
        }
    }
}
