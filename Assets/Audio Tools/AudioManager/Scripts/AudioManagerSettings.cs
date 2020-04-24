using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioManagerSettings", menuName = "AudioManager/AudioManagerSettings", order = 1)]
public class AudioManagerSettings : ScriptableObject
{
    [Range(-80f, 20f)] public float masterVolume = 0f;
    [SerializeField] private AnimationCurve masterLerpCurve;
    public AnimationCurve MasterLerpCurve => masterLerpCurve;
    [SerializeField] private float masterSpeedLerp = .5f;
    public float MasterSpeedLerp => masterSpeedLerp;
    
    [Range(-80f, 20f)] public float musicVolume = 0f;
    [SerializeField] private AnimationCurve musicLerpCurve;
    public AnimationCurve MusicLerpCurve => musicLerpCurve;
    [SerializeField] private float musicSpeedLerp = .5f;
    public float MusicSpeedLerp => musicSpeedLerp;
    
    [Range(-80f, 20f)] public float sfxVolume = 0f;
    [SerializeField] private AnimationCurve sfxLerpCurve;
    public AnimationCurve SfxLerpCurve => sfxLerpCurve;
    [SerializeField] private float sfxSpeedLerp = .5f;
    public float SfxSpeedLerp => sfxSpeedLerp;
    
    [SerializeField] private AudioMixer amsAudioMixer;
    public AudioMixer AmAudioMixer => amsAudioMixer;
    
    [SerializeField,HideInInspector] List<AudioClip> musicList = new List<AudioClip>();

    [SerializeField,HideInInspector] List<AudioClip> sfxList = new List<AudioClip>();

    public List<AudioClip> MusicList => musicList;

    public List<AudioClip> SfxList => sfxList;
}
