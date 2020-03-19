using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterAttributeMenu : MenuControls
{
    [HideInInspector]
    public int[] percentageValues = new int[] { 0, 25, 50, 75, 100 };
    [HideInInspector]
    public int[] expanderValues = new int[] { 1, 3, 5, 7, 9 };

    [SerializeField]
    private ItemGroup item;

    [SerializeField]
    private TextMeshProUGUI pointsField;

    [SerializeField]
    private AudioClip juwelInSound;

    [SerializeField]
    private AudioClip juwelOutSound;

    [SerializeField]
    private AudioSource audiosource;

    [SerializeField]
    private List<CharacterAttributeStats> statObjects = new List<CharacterAttributeStats>();
    
    private int attributePoints = 0;
    private int attributePointsMax;
    private int pointsSpent;
    private int pointsLeft;
    private bool initLoad = true;

    private void Start()
    {
        foreach (CharacterAttributeStats statObject in this.statObjects) statObject.init();
        
        updatePoints();
        this.initLoad = false;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if(!this.initLoad) updatePoints();        
    }

    public void playJuwelSound(bool insert)
    {
        if (insert) CustomUtilities.Audio.playSoundEffect(this.gameObject, this.juwelInSound);
        else CustomUtilities.Audio.playSoundEffect(this.gameObject, this.juwelOutSound);
    }

    public int getPointsLeft()
    {
        return this.pointsLeft;
    }

    public void updatePoints()
    {
        this.attributePoints = this.player.GetComponent<PlayerUtils>().GetAmount(this.item);
        this.attributePointsMax = this.item.maxAmount;

        this.pointsSpent = 0;

        foreach (CharacterAttributeStats statObject in this.statObjects)
        {
            this.pointsSpent += statObject.getPointsSpent();
        }

        this.pointsLeft = this.attributePoints - this.pointsSpent;

        string text = CustomUtilities.Format.formatString(this.pointsLeft, this.attributePointsMax)+" / " + CustomUtilities.Format.formatString(this.attributePointsMax, this.attributePointsMax);
        this.pointsField.text = text;
    }
}
