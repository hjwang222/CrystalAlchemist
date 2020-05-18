using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class CharacterAttributeMenu : MenuBehaviour
{
    [BoxGroup("Required")]
    [Required]
    public PlayerInventory inventory;

    [BoxGroup("Required")]
    [Required]
    public SimpleSignal healtSignal;

    [BoxGroup("Required")]
    [Required]
    public SimpleSignal manaSignal;

    [HideInInspector]
    public int[] percentageValues = new int[] { 0, 25, 50, 75, 100 };
    [HideInInspector]
    public int[] expanderValues = new int[] { 1, 3, 5, 7, 9 };

    [BoxGroup]
    [SerializeField]
    private ItemGroup item;

    [BoxGroup]
    [SerializeField]
    private TextMeshProUGUI pointsField;

    [BoxGroup]
    [SerializeField]
    private AudioClip juwelInSound;

    [BoxGroup]
    [SerializeField]
    private AudioClip juwelOutSound;

    [BoxGroup]
    [SerializeField]
    private AudioSource audiosource;

    [BoxGroup]
    [SerializeField]
    private List<CharacterAttributeStats> statObjects = new List<CharacterAttributeStats>();
    
    private int attributePoints = 0;
    private int attributePointsMax;
    private int pointsSpent;
    private int pointsLeft;

    public override void Start()
    {
        base.Start();
        foreach (CharacterAttributeStats statObject in this.statObjects) statObject.init();        
        updatePoints();
    }

    public void playJuwelSound(bool insert)
    {
        if (insert) AudioUtil.playSoundEffect(this.gameObject, this.juwelInSound);
        else AudioUtil.playSoundEffect(this.gameObject, this.juwelOutSound);
    }

    public int getPointsLeft()
    {
        return this.pointsLeft;
    }

    public void updatePoints()
    {
        this.attributePoints = this.inventory.GetAmount(this.item);
        this.attributePointsMax = this.item.maxAmount;

        this.pointsSpent = 0;

        foreach (CharacterAttributeStats statObject in this.statObjects)
        {
            this.pointsSpent += statObject.getPointsSpent();
        }

        this.pointsLeft = this.attributePoints - this.pointsSpent;

        string text = FormatUtil.formatString(this.pointsLeft, this.attributePointsMax)+" / " + FormatUtil.formatString(this.attributePointsMax, this.attributePointsMax);
        this.pointsField.text = text;
    }
}
