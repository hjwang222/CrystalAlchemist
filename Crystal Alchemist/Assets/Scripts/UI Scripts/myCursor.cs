using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myCursor : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundEffect;
    private AudioSource audioSource;
    private bool isPlaying = false;

    public InfoBox infoBox;

    [SerializeField]
    private Image image;

    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private GameObject cursorSelected;

    private void Start()
    {
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;

        this.image.sprite = null;
        this.cursorSelected.SetActive(false);
        this.cursor.SetActive(true);
    }

    private void OnEnable()
    {
        this.isPlaying = false;
    }

    private void OnDisable()
    {
        this.isPlaying = false;
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
        //Debug.Log(isPlaying);

        if (!this.isPlaying)
        {
            this.isPlaying = true;
            StartCoroutine(playAudioEffect());          
        }
    }

    private IEnumerator playAudioEffect()
    {
        CustomUtilities.Audio.playSoundEffect(this.audioSource, this.soundEffect);
        float length = this.soundEffect.length;
        yield return new WaitForSeconds(length);
        this.isPlaying = false;
        //Debug.Log(isPlaying+" "+length);
    }
}
