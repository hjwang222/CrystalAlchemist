using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretArea : MonoBehaviour
{
    private Tilemap map;
    [SerializeField]
    private float delay = .0025f;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip secretSoundEffect;

    void Start()
    {
        this.map = GetComponent<Tilemap>();
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            Utilities.playSoundEffect(this.audioSource, this.secretSoundEffect, GlobalValues.backgroundMusicVolume);
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        for (float f = .05f; f <= 1.1; f += .05f)
        {
            setColor(f);
            yield return new WaitForSeconds(this.delay);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -.05f; f -= .05f)
        {
            setColor(f);
            yield return new WaitForSeconds(this.delay);
        }
    }

    private void setColor(float f)
    {
        Color newcolor = this.map.color;
        newcolor.a = f;
        this.map.color = newcolor;        
    }

}
