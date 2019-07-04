using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundEffect;
    private AudioSource audioSource;
    private bool isPlaying = false;

    [SerializeField]
    private Image image;

    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private GameObject cursorSelected;

    private void Awake()
    {
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;

        this.image.sprite = null;
        this.cursorSelected.SetActive(false);
        this.cursor.SetActive(true);
    }

    public void setSelectedGameObject(Image image)
    {
        if(image == null)
        {
            this.image.sprite = null;
            this.cursorSelected.SetActive(false);            
            this.cursor.SetActive(true);
        }
        else
        {
            this.image.sprite = image.sprite;            
            this.cursorSelected.SetActive(true);
            this.cursor.SetActive(false);
        }
    }

    public void playSoundEffect()
    {
        if (!this.isPlaying)
        {
            StartCoroutine(playAudioEffect());          
        }
    }

    private IEnumerator playAudioEffect()
    {
        this.isPlaying = true;
        Utilities.playSoundEffect(this.audioSource, this.soundEffect);
        float length = this.soundEffect.length;
        yield return new WaitForSeconds(length);
        this.isPlaying = false;
    }
}
