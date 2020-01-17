using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeVolumeMenu : TitleScreenMenues
{
    [SerializeField]
    private AudioSource musicSource;

    private void OnEnable()
    {
        setVolumeText(getVolumeFromEnum());
    }

    private float getVolumeFromEnum()
    {
        switch (this.volumeType)
        {
            case VolumeType.effects: return GlobalValues.soundEffectVolume;
            case VolumeType.music: return GlobalValues.backgroundMusicVolume;
        }

        return 0;
    }

    public void ChangeVolume()
    {
        if (this.volumeType == VolumeType.music)
        {
            GlobalValues.backgroundMusicVolume = this.slider.value;
            setVolumeText(GlobalValues.backgroundMusicVolume);
            this.musicSource.volume = GlobalValues.backgroundMusicVolume;
        }
        else if (this.volumeType == VolumeType.effects)
        {
            GlobalValues.soundEffectVolume = this.slider.value;
            setVolumeText(GlobalValues.soundEffectVolume);
        }
    }

    private void setVolumeText(float volume)
    {
        this.slider.value = volume;
        this.textField.text = Mathf.RoundToInt(volume * 100) + "%";
    }    
}
