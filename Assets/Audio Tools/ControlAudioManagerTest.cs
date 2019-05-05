using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAudioManagerTest : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] Slider slider;
    int musicIndex = 0;

    private void Start()
    {
        slider.value = AudioManager.GetInstance().GetMasterVolume();
    }

    public void ChangeMusic(int value)
    {
        musicIndex += value;
        if(musicIndex < 0)
        {
            musicIndex = 0;
        }

        if (musicIndex > audioClips.Count - 1)
        {
            musicIndex = audioClips.Count - 1;
        }

        AudioManager.GetInstance().PlayMusic(audioClips[musicIndex]);
    }

   public void SetMusicVolume(float volume)
    {
        AudioManager.GetInstance().SetMusicVolumeTo(volume);
    }
}
