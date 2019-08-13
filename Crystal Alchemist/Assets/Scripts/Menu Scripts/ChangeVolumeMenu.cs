using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeVolumeMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI musicUGUI;
    [SerializeField]
    private TextMeshProUGUI effectUGUI;
    [SerializeField]
    private AudioSource musicSource;


    private void OnEnable()
    {
        setVolumeText(this.musicUGUI, GlobalValues.backgroundMusicVolume);
        setVolumeText(this.effectUGUI, GlobalValues.soundEffectVolume);
    }

    public void addVolume(TextMeshProUGUI ugui)
    {
        changeVolume(ugui, 1);
    }

    public void reduceVolume(TextMeshProUGUI ugui)
    {
        changeVolume(ugui, -1);
    }

    private void changeVolume(TextMeshProUGUI ugui, int value)
    {
        if (ugui.gameObject.transform.parent.GetComponent<TextMeshProUGUI>().text.Contains("Musik") || ugui.gameObject.transform.parent.GetComponent<TextMeshProUGUI>().text.Contains("Music"))
        {
            GlobalValues.backgroundMusicVolume = addVolume(GlobalValues.backgroundMusicVolume, value);
            setVolumeText(ugui, GlobalValues.backgroundMusicVolume);
            this.musicSource.volume = GlobalValues.backgroundMusicVolume;
        }
        else
        {
            GlobalValues.soundEffectVolume = addVolume(GlobalValues.soundEffectVolume, value);
            setVolumeText(ugui, GlobalValues.soundEffectVolume);
        }
    }

    private void setVolumeText(TextMeshProUGUI ugui, float volume)
    {
        ugui.text = Mathf.RoundToInt(volume * 100) + "%";
    }

    private float addVolume(float volume, float addvolume)
    {
        if (addvolume != 0)
        {
            //if (this.cursorSound != null) Utilities.playSoundEffect(this.audioSource, this.cursorSound);
            volume += (addvolume / 100);
            if (volume < 0) volume = 0;
            else if (volume > 2f) volume = 2f;
        }

        return volume;
    }
    
}
