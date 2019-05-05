using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

[RequireComponent(typeof(AudioVisualizer))]
public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioMixer audioMixer;

    [SerializeField] [Range(-80f, 20f)] float musicVolume = 1f;
    [SerializeField] float outMusicVolume = 0f;

    [SerializeField] [Range(-80f, 20f)] float sfxVolume = 1f;
    [SerializeField] float outSfxVolume = 0f;

    [SerializeField] [Range(-80f, 20f)] float masterVolume = 1f;
    [SerializeField] float outMasterVolume = 0f;

    [SerializeField] AudioSource activeTrack;

    [SerializeField] AudioSource sfx;

    [SerializeField] bool keepFadeIn = false;

    [SerializeField] bool keepFadeOut = false;

    [SerializeField] List<AudioClip> musicList = new List<AudioClip>();

    [SerializeField] List<AudioClip> sfxList = new List<AudioClip>();

    [SerializeField] List<AudioSource> musicTracks = new List<AudioSource>();

    [SerializeField] List<AudioSource> sfxTracks = new List<AudioSource>();

    GameObject musicContainer;
    GameObject sfxContainer;

    AudioVisualizer audioVisualizer;

    static AudioManager audioManager;

    UnityEvent OnMusicChange = new UnityEvent();

    public void AddOnMusicChange(UnityAction method) { OnMusicChange.AddListener(method); }
    public void RemoveOnMusicChange(UnityAction method) { OnMusicChange.RemoveListener(method); }

    //Return the AudioManager component, is not exist this create one
    public static AudioManager GetInstance()
    {
        if (!audioManager)
        {
            GameObject go = new GameObject("Audio Manager");
            go.AddComponent<AudioManager>();
            go.GetComponent<AudioManager>().Initialization();
        }
        return audioManager;
    }

    private void Awake()
    {
        Initialization();
    }

    void Start()
    {
        StartCoroutine(InitVolumeDefault());
    }

    void Update()
    {
        RuntimeVolumeControl();
    }

    //Initialize the component
    void Initialization()
    {
        audioMixer = Resources.Load<AudioMixer>("Mixer");
        audioManager = this.GetComponent<AudioManager>();
        audioVisualizer = GetComponent<AudioVisualizer>();
        audioVisualizer.useAudioManager = true;
        DontDestroyOnLoad(this);

        if (musicList.Count > 0)
        {
            foreach (var item in musicList)
            {
                CreateMusicTrack(item);
            }
        }

        if (sfxList.Count > 0)
        {
            foreach (var item in sfxList)
            {
                CreateSfxTrack(item);
            }
        }
    }

    //For start the default volume value
    IEnumerator InitVolumeDefault()
    {
        outMasterVolume = masterVolume;
        audioMixer.SetFloat("MasterVolume", outMasterVolume);
        outMusicVolume = musicVolume;
        audioMixer.SetFloat("MusicVolume", outMusicVolume);
        outSfxVolume = sfxVolume;
        audioMixer.SetFloat("SfxVolume", outSfxVolume);

        yield return new WaitForEndOfFrame();
    }

    //This is for control volume in runtime
    void RuntimeVolumeControl()
    {

        if (outMasterVolume != masterVolume)
        {
            outMasterVolume = masterVolume;
            audioMixer.SetFloat("MasterVolume", outMasterVolume);
        }

        if (outMusicVolume != musicVolume)
        {
            outMusicVolume = musicVolume;
            audioMixer.SetFloat("MusicVolume", outMusicVolume);
        }

        if (outSfxVolume != sfxVolume)
        {
            outSfxVolume = sfxVolume;
            audioMixer.SetFloat("SfxVolume", outSfxVolume);
        }

    }

    //This create a music track object and components
    void CreateMusicTrack(AudioClip clip)
    {
        if (!ThisClipExistInMusicList(clip))
        {
            musicList.Add(clip);
        }

        if (!musicContainer)
        {
            musicContainer = new GameObject("Music Tracks");
            musicContainer.transform.SetParent(this.transform);
        }

        GameObject go = new GameObject(clip.name);
        go.transform.SetParent(musicContainer.transform);
        go.AddComponent<AudioSource>();
        AudioSource tempTrack = go.GetComponent<AudioSource>();
        tempTrack.loop = true;
        tempTrack.playOnAwake = false;
        tempTrack.volume = 0f;
        tempTrack.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        tempTrack.clip = clip;
        musicTracks.Add(tempTrack);
    }

    //Search if the music exist in list
    bool ThisClipExistInMusicList(AudioClip clip)
    {
        bool temp = false;
        if (musicList.Count > 0)
        {
            foreach (var item in musicList)
            {
                if (item.name == clip.name)
                {
                    temp = true;
                }
            }
        }

        return temp;
    }

    //This is for external creation, a request
    public void CeateMusicTrackRequest(AudioClip clip)
    {
        CreateMusicTrack(clip);
    }

    //Create a sounfx if not exist
    void CreateSfxTrack(AudioClip clip)
    {
        if (!ThisClipExistInSfxList(clip))
        {
            sfxList.Add(clip);
        }

        if (!sfxContainer)
        {
            sfxContainer = new GameObject("Sfx Tracks");
            sfxContainer.transform.SetParent(this.transform);
        }

        GameObject go = new GameObject(clip.name);
        go.transform.SetParent(sfxContainer.transform);
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>();
        go.GetComponent<AudioSource>().loop = false;
        go.GetComponent<AudioSource>().playOnAwake = false;
        go.GetComponent<AudioSource>().volume = 1f;
        go.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixer.FindMatchingGroups("SoundFX")[0];
        go.GetComponent<AudioSource>().clip = clip;
        sfxTracks.Add(go.GetComponent<AudioSource>());
    }

    //Search if the soundfx exist
    bool ThisClipExistInSfxList(AudioClip clip)
    {
        bool temp = false;
        if (sfxList.Count > 0)
        {
            foreach (var item in sfxList)
            {
                if (item.name == clip.name)
                {
                    temp = true;
                }
            }
        }

        return temp;
    }

    //For external creation, soundfx creation request
    public void CeateSfxTrackRequest(AudioClip clip)
    {
        CreateSfxTrack(clip);
    }

    //Get the master volume
    public float GetMasterVolume()
    {
        return masterVolume;
    }

    //Get the music volume
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    //Get the soundfx volume
    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    //This is for lerp the master volume from one value to another
    public void LerpMasterVolumeTo(float volumeValue, float speedToChange)
    {
        StartCoroutine(LerpMasterVolume(volumeValue, speedToChange));
    }

    //This is for set specific value for the master volume
    public void SetMasterVolumeTo(float volumeValue)
    {
        masterVolume = volumeValue;
    }

    IEnumerator LerpMasterVolume(float volumeValue, float speedToChange)
    {
        if (masterVolume > volumeValue)
        {
            while (masterVolume > volumeValue)
            {
                masterVolume -= speedToChange;
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
        else
        {
            while (masterVolume < volumeValue)
            {
                masterVolume += speedToChange;
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
    }

    //This is for lerp te music volume from one value to another
    public void LerpMusicVolumeTo(float volumeValue, float speedToChange)
    {
        StartCoroutine(LerpMusicVolume(volumeValue, speedToChange));
    }

    //This is for set specific value for the music volume
    public void SetMusicVolumeTo(float volumeValue)
    {
        musicVolume = volumeValue;
    }

    IEnumerator LerpMusicVolume(float volumeValue, float speedToChange)
    {
        if (musicVolume > volumeValue)
        {
            while (musicVolume > volumeValue)
            {
                musicVolume -= speedToChange;
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
        else
        {
            while (musicVolume < volumeValue)
            {
                musicVolume += speedToChange;
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
    }

    //This is for lerp te soundfx volume from one value to another
    public void LerpSfxVolumeTo(float volumeValue, float speedToChange)
    {
        StartCoroutine(LerpSfxVolume(volumeValue, speedToChange));
    }

    //This is for set specific soundfx for the master volume
    public void SetSfxVolumeTo(float volumeValue)
    {
        sfxVolume = volumeValue;
    }

    IEnumerator LerpSfxVolume(float volumeValue, float speedToChange)
    {
        if (sfxVolume > volumeValue)
        {
            while (sfxVolume > volumeValue)
            {
                sfxVolume -= speedToChange;
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
        else
        {
            while (sfxVolume < volumeValue)
            {
                sfxVolume += speedToChange;
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
    }

    //For play music, if not exist in list create one and play
    public void PlayMusic(AudioClip audioClip)
    {
        AudioSource temp;

        if (MusicTrackExist(audioClip.name))
        {
            temp = GetMusicTrackByName(audioClip.name);
        }
        else
        {
            CreateMusicTrack(audioClip);
            temp = GetMusicTrackByName(audioClip.name);
        }

        if (activeTrack != null && activeTrack.isPlaying)
        {
            if (activeTrack.name != temp.name)
            {
                StartCoroutine(CrossFade(temp, .1f));
            }
        }
        else
        {
            StartCoroutine(FadeIn(temp, .1f));
        }
    }

    //For play soundfx, if not exist in list create one and play
    public void PlaySfx(AudioClip audioClip)
    {
        AudioSource temp;

        if (SfxTrackExist(audioClip.name))
        {
            temp = GetSfxTrackByName(audioClip.name);
        }
        else
        {
            CreateSfxTrack(audioClip);
            temp = GetSfxTrackByName(audioClip.name);
        }
        temp.Play();
    }

    //For play soundfx, if not exist in list create one and play in loop mode
    public void PlaySfxInLoop(AudioClip audioClip){
        AudioSource temp;

        if (SfxTrackExist(audioClip.name))
        {
            temp = GetSfxTrackByName(audioClip.name);
            temp.loop = true;
        }
        else
        {
            CreateSfxTrack(audioClip);
            temp = GetSfxTrackByName(audioClip.name);
            temp.loop = true;
        }
        if(!temp.isPlaying)
            temp.Play();
    }

    //For stop soundfx, if not exist in list create one and stop include the loop mode
    public void StopSfxInLoop(AudioClip audioClip){
        AudioSource temp;

        if (SfxTrackExist(audioClip.name))
        {
            temp = GetSfxTrackByName(audioClip.name);
            temp.loop = false;
        }
        else
        {
            CreateSfxTrack(audioClip);
            temp = GetSfxTrackByName(audioClip.name);
            temp.loop = false;
        }
        temp.Stop();
    }

    //Stop all soundfx in loop mode
    public void StopAllSfxInLoop(){
        foreach (var clip in sfxTracks)
        {
            clip.loop = false;
        }
    }

    IEnumerator CrossFade(AudioSource audioSource, float speed)
    {
        audioSource.Play();
        if (audioSource != activeTrack)
        {

            while (activeTrack.volume > 0f || audioSource.volume < 1f)
            {
                activeTrack.volume -= speed;
                audioSource.volume += speed;
                yield return new WaitForSecondsRealtime(.1f);
            }

            activeTrack.Stop();
            activeTrack = audioSource;
        }
        OnMusicChange.Invoke();
    }

    IEnumerator FadeIn(AudioSource audioSource, float speed)
    {
        keepFadeIn = true;
        keepFadeOut = false;

        audioSource.volume = 0f;
        audioSource.Play();

        activeTrack = audioSource;

        while (audioSource.volume < 1f && keepFadeIn)
        {
            audioSource.volume += speed;
            yield return new WaitForSecondsRealtime(.1f);
        }

        OnMusicChange.Invoke();
    }

    IEnumerator FadeOut(AudioSource audioSource, float speed)
    {
        keepFadeIn = false;
        keepFadeOut = true;

        while (audioSource.volume > 0f && keepFadeOut)
        {
            audioSource.volume -= speed;
            yield return new WaitForSecondsRealtime(.1f);
        }
        audioSource.Stop();
    }

    public AudioSource GetActiveTrack()
    {
        return activeTrack;
    }

    AudioSource GetMusicTrackByName(string name)
    {
        AudioSource source = null;

        foreach (var item in musicTracks)
        {
            if (item.name == name)
            {
                source = item;
            }
        }

        return source;
    }

    AudioSource GetSfxTrackByName(string name)
    {
        AudioSource source = null;

        foreach (var item in sfxTracks)
        {
            if (item.name == name)
            {
                source = item;
            }
        }

        return source;
    }

    bool MusicTrackExist(string name)
    {
        bool temp = false;

        if (musicList.Count > 0)
        {
            foreach (var item in musicList)
            {
                if (item.name == name)
                {
                    temp = true;
                }
            }
        }

        return temp;
    }

    bool SfxTrackExist(string name)
    {
        bool temp = false;

        if (sfxList.Count > 0)
        {
            foreach (var item in sfxList)
            {
                if (item.name == name)
                {
                    temp = true;
                }
            }
        }

        return temp;
    }
}
