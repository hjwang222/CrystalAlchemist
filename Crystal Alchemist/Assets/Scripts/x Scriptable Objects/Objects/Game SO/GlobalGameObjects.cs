using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Global Game Objects")]
public class GlobalGameObjects : ScriptableObject
{
    public static DamageNumbers damageNumber;
    public static ContextClue contextClue;
    public static GameObject markAttack;
    public static GameObject markTarget;
    public static MiniDialogBox miniDialogBox;
    public static CastBar castBar;

    [BoxGroup("Interaction")]
    [SerializeField]
    private ContextClue context;

    [BoxGroup("Combat")]
    [SerializeField]
    private DamageNumbers damage;
    [BoxGroup("Combat")]
    [SerializeField]
    private CastBar cast;

    [BoxGroup("Bubbles")]
    [SerializeField]
    private MiniDialogBox dialog;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject attacking;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject targeting;


    public void Initialize()
    {
        contextClue = this.context;
        damageNumber = this.damage;
        miniDialogBox = this.dialog;
        markAttack = this.attacking;
        markTarget = this.targeting;
        castBar = this.cast;
    }
}
