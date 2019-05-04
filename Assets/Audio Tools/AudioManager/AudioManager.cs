using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

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

    static AudioManager audioManager;

    UnityEvent OnMusicChange = new UnityEvent();

    public void AddOnMusicChange(UnityAction method) { OnMusicChange.AddListener(method); }
    public void RemoveOnMusicChange(UnityAction method) { OnMusicChange.RemoveListener(method); }


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

    void Initialization()
    {
        audioMixer = Resources.Load<AudioMixer>("Mixer");
        audioManager = this.GetComponent<AudioManager>();
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
        go.GetComponent<AudioSource>();
        go.GetComponent<AudioSource>().loop = true;
        go.GetComponent<AudioSource>().playOnAwake = false;
        go.GetComponent<AudioSource>().volume = 0f;
        go.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        go.GetComponent<AudioSource>().clip = clip;
        musicTracks.Add(go.GetComponent<AudioSource>());
    }


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

    public void CeateMusicTrackRequest(AudioClip clip)
    {
        CreateMusicTrack(clip);
    }

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

    public void CeateSfxTrackRequest(AudioClip clip)
    {
        CreateSfxTrack(clip);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    public void LerpMasterVolumeTo(float volumeValue, float speedToChange)
    {
        StartCoroutine(LerpMasterVolume(volumeValue, speedToChange));
    }

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

    public void LerpMusicVolumeTo(float volumeValue, float speedToChange)
    {
        StartCoroutine(LerpMusicVolume(volumeValue, speedToChange));
    }

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

    public void LerpSfxVolumeTo(float volumeValue, float speedToChange)
    {
        StartCoroutine(LerpSfxVolume(volumeValue, speedToChange));
    }

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
