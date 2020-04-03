using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Settings/Global Game Objects")]
public class GlobalGameObjects : ScriptableObject
{
    public static DamageNumbers damageNumber;
    public static ContextClue contextClue;
    public static GameObject markAttack;
    public static GameObject markTarget;
    public static MiniDialogBox miniDialogBox;
    public static CastBar castBar;
    public static AnalyseInfo analyseInfo;
    public static GameSettings settings;
    public static GlobalValues staticValues;
    public static List<ItemDrop> itemDrops = new List<ItemDrop>();
    public static List<ItemGroup> itemGroups = new List<ItemGroup>();
    public static List<Ability> abilities = new List<Ability>();

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

    [BoxGroup("Loading")]
    [SerializeField]
    private List<ItemDrop> drops = new List<ItemDrop>();
    [BoxGroup("Loading")]
    [SerializeField]
    private List<ItemGroup> groups = new List<ItemGroup>();
    [BoxGroup("Loading")]
    [SerializeField]
    private List<Ability> skills = new List<Ability>();


    public void Initialize()
    {
        contextClue = this.context;
        damageNumber = this.damage;
        miniDialogBox = this.dialog;
        markAttack = this.attacking;
        markTarget = this.targeting;
        castBar = this.cast;
        analyseInfo = this.analyse;
        settings = this.gameSettings;
        staticValues = this.globalValues;
        itemDrops = this.drops;
        itemGroups = this.groups;
        abilities = this.skills;
    }

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
