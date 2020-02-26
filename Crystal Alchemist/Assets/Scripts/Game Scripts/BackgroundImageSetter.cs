using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageSetter : MonoBehaviour
{
    [SerializeField]
    private SpriteSignal spriteSignal;

    [SerializeField]
    private FloatSignal scaleSignal;

    [SerializeField]
    private FloatSignal positionSignal;

    [SerializeField]
    private MaterialSignal materialSignal;

    [SerializeField]
    private Sprite backgroundSprite;

    [SerializeField]
    private float backgroundScale = 6;

    [SerializeField]
    private float positionY = 0;

    [SerializeField]
    private Material material;

    void Start()
    {
        this.spriteSignal.Raise(this.backgroundSprite);
        this.scaleSignal.Raise(this.backgroundScale);
        this.positionSignal.Raise(this.positionY);
        this.materialSignal.Raise(this.material);
    }
}
