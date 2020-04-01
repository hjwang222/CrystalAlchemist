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

        this.TMPcharacterName.text = FormatUtil.getLanguageDialogText(this.npc.stats.characterName, this.npc.stats.englischCharacterName);
        this.lifeBar.fillAmount((this.npc.life / this.npc.maxLife));
        this.statusEffectBar.setCharacter(this.npc);

        if (this.npc.itemDrop != null && this.npc.currentState != CharacterState.dead)
        {
            this.ImageitemPreview.gameObject.SetActive(true);
            this.ImageitemPreview.sprite = this.npc.itemDrop.stats.getSprite(); //TODONEW
        }
    }
}
