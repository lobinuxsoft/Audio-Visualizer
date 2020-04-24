using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioManagerSettings audioManagerSettings;
    
    float outMasterVolume = 0f;
    private float tMasterLerp = 0;
    
    float outMusicVolume = 0f;
    private float tMusicLerp = 0;
    
    float outSfxVolume = 0f;
    private float tSfxLerp = 0;

    AudioSource activeTrack;

    List<AudioSource> musicTracks = new List<AudioSource>();

    List<AudioSource> sfxTracks = new List<AudioSource>();

    bool keepFadeIn = false;

    bool keepFadeOut = false;
    
    GameObject musicContainer;
    GameObject sfxContainer;

    static AudioManager instance;
    
    UnityEvent OnMusicChange = new UnityEvent();

    public void AddOnMusicChange(UnityAction method) { OnMusicChange.AddListener(method); }
    public void RemoveOnMusicChange(UnityAction method) { OnMusicChange.RemoveListener(method); }

    public static AudioManager Instance => instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
            Initialization();
        }
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
        if (audioManagerSettings.MusicList.Count > 0)
        {
            foreach (var item in audioManagerSettings.MusicList)
            {
                CreateMusicTrack(item);
            }
        }

        if (audioManagerSettings.SfxList.Count > 0)
        {
            foreach (var item in audioManagerSettings.SfxList)
            {
                CreateSfxTrack(item);
            }
        }
    }

    IEnumerator InitVolumeDefault()
    {
        outMasterVolume = audioManagerSettings.masterVolume;
        audioManagerSettings.AmAudioMixer.SetFloat("MasterVolume", outMasterVolume);
        outMusicVolume = audioManagerSettings.musicVolume;
        audioManagerSettings.AmAudioMixer.SetFloat("MusicVolume", outMusicVolume);
        outSfxVolume = audioManagerSettings.sfxVolume;
        audioManagerSettings.AmAudioMixer.SetFloat("SfxVolume", outSfxVolume);

        yield return new WaitForEndOfFrame();
    }

    void RuntimeVolumeControl()
    {

        if (outMasterVolume != audioManagerSettings.masterVolume)
        {
            outMasterVolume = Mathf.Lerp(outMasterVolume, audioManagerSettings.masterVolume, audioManagerSettings.MasterLerpCurve.Evaluate(tMasterLerp));
            audioManagerSettings.AmAudioMixer.SetFloat("MasterVolume", outMasterVolume);

            tMasterLerp += audioManagerSettings.MasterSpeedLerp * Time.deltaTime;
        }
        else
        {
            tMasterLerp = 0;
        }

        if (outMusicVolume != audioManagerSettings.musicVolume)
        {
            outMusicVolume = Mathf.Lerp(outMusicVolume, audioManagerSettings.musicVolume, tMusicLerp);
            audioManagerSettings.AmAudioMixer.SetFloat("MusicVolume", outMusicVolume);
            
            tMusicLerp += audioManagerSettings.MusicSpeedLerp * Time.deltaTime;
        }
        else
        {
            tMusicLerp = 0;
        }

        if (outSfxVolume != audioManagerSettings.sfxVolume)
        {
            outSfxVolume = Mathf.Lerp(outSfxVolume, audioManagerSettings.sfxVolume, tSfxLerp);;
            audioManagerSettings.AmAudioMixer.SetFloat("SfxVolume", outSfxVolume);
            
            tSfxLerp += audioManagerSettings.SfxSpeedLerp * Time.deltaTime;
        }
        else
        {
            tSfxLerp = 0;
        }

    }

    void CreateMusicTrack(AudioClip clip)
    {
        if (audioManagerSettings.MusicList.Count > 0)
        {
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
            go.GetComponent<AudioSource>().outputAudioMixerGroup = audioManagerSettings.AmAudioMixer.FindMatchingGroups("Music")[0];
            go.GetComponent<AudioSource>().clip = clip;
            musicTracks.Add(go.GetComponent<AudioSource>());
        }
    }
    
    void CreateSfxTrack(AudioClip clip)
    {
        if (audioManagerSettings.SfxList.Count > 0)
        {
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
            go.GetComponent<AudioSource>().outputAudioMixerGroup = audioManagerSettings.AmAudioMixer.FindMatchingGroups("SoundFX")[0];
            go.GetComponent<AudioSource>().clip = clip;
            sfxTracks.Add(go.GetComponent<AudioSource>());
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
        AudioSource source = musicTracks.Find(x => x.name == name);
        
        return source;
    }

    AudioSource GetSfxTrackByName(string name)
    {
        AudioSource source = sfxTracks.Find(x => x.name == name);
        
        return source;
    }

    bool MusicTrackExist(string name)
    {
        AudioClip music = null;

        if (audioManagerSettings.MusicList.Count > 0)
        {
            music = audioManagerSettings.MusicList.Find(x => x.name == name);
        }
        
        return (music != null);
    }

    bool SfxTrackExist(string name)
    {
        AudioClip soundfx = null;
        
        if (audioManagerSettings.SfxList.Count > 0)
        {
            soundfx = audioManagerSettings.SfxList.Find(x => x.name == name);
        }

        return (soundfx != null);
    }
}
