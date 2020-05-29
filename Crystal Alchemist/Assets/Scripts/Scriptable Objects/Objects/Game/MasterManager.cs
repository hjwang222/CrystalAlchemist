using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Settings/Global Game Objects")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    public static DamageNumbers damageNumber { get { return Instance.damage; } }
    public static ContextClue contextClue { get { return Instance.context; } }
    public static GameObject markAttack { get { return Instance.attacking; } }
    public static GameObject markTarget { get { return Instance.targeting; } }
    public static MiniDialogBox miniDialogBox { get { return Instance.dialog; } }
    public static CastBar castBar { get { return Instance.cast; } }
    public static AnalyseInfo analyseInfo { get { return Instance.analyse; } }
    public static GameSettings settings { get { return Instance.gameSettings; } }
    public static DebugSettings debugSettings { get { return Instance.debugging; } }
    public static GlobalValues staticValues { get { return Instance.globalValues; } }
    public static List<ItemDrop> itemDrops { get { return Instance.drops; } }
    public static List<ItemGroup> itemGroups { get { return Instance.groups; } }
    public static List<Ability> abilities { get { return Instance.skills; } }
    public static TargetingSystem targetingSystem { get { return Instance.targetSystem; } }
    public static TimeValue time { get { return Instance.timeValue; } }
    public static StringValue actionButtonText { get { return Instance.actionButton; } }

    [BoxGroup("Interaction")]
    [SerializeField]
    private ContextClue context;
    [BoxGroup("Interaction")]
    [SerializeField]
    private AnalyseInfo analyse;

    [BoxGroup("Combat")]
    [SerializeField]
    private DamageNumbers damage;
    [BoxGroup("Combat")]
    [SerializeField]
    private CastBar cast;
    [BoxGroup("Combat")]
    [SerializeField]
    private TargetingSystem targetSystem;

    [BoxGroup("Values")]
    [SerializeField]
    private TimeValue timeValue;
    [BoxGroup("Values")]
    [SerializeField]
    private StringValue actionButton;

    [BoxGroup("Bubbles")]
    [SerializeField]
    private MiniDialogBox dialog;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject attacking;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject targeting;

    [BoxGroup("Settings")]
    [SerializeField]
    private GameSettings gameSettings;
    [BoxGroup("Settings")]
    [SerializeField]
    private GlobalValues globalValues;
    [BoxGroup("Settings")]
    [SerializeField]
    private DebugSettings debugging;

    [BoxGroup("Loading")]
    [SerializeField]
    private List<ItemDrop> drops = new List<ItemDrop>();
    [BoxGroup("Loading")]
    [SerializeField]
    private List<ItemGroup> groups = new List<ItemGroup>();
    [BoxGroup("Loading")]
    [SerializeField]
    private List<Ability> skills = new List<Ability>();


    [Button]
    public void LoadAll()
    {
        this.drops.Clear();
        this.groups.Clear();
        this.skills.Clear();

        this.drops.AddRange(Resources.LoadAll<ItemDrop>("Scriptable Objects/Items/Item Drops/Key Items/"));
        this.groups.AddRange(Resources.LoadAll<ItemGroup>("Scriptable Objects/Items/Item Groups/Inventory Items/"));
        this.groups.AddRange(Resources.LoadAll<ItemGroup>("Scriptable Objects/Items/Item Groups/Currencies/"));
        this.skills.AddRange(Resources.LoadAll<Ability>("Scriptable Objects/Abilities/Skills/Player Skills/"));
    }

    public static ItemDrop getItemDrop(string name)
    {
        foreach(ItemDrop drop in itemDrops)
        {
            if (drop.name == name) return drop;
        }

        return null;
    }

    public static ItemGroup getItemGroup(string name)
    {
        foreach (ItemGroup group in itemGroups)
        {
            if (group.name == name) return group;
        }

        return null;
    }
}
