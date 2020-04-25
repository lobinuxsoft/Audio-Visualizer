using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UISimpleFader))]
public class AudioPanel : MonoBehaviour
{
    public static AudioPanel instance;

    [SerializeField] private AudioManagerSettings audioManagerSettings;
    [SerializeField] private UISliderControl master;
    [SerializeField] private UISliderControl music;
    [SerializeField] private UISliderControl soundsFx;

    [SerializeField] private Button acceptButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private float defaultMaster;
    [SerializeField] private float defaultMusic;
    [SerializeField] private float defaultSfx;

    public UnityEvent onPanelShow;
    public UnityEvent onPanelHide;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        acceptButton.onClick.AddListener(AcceptChanges);
        cancelButton.onClick.AddListener(CancelChanges);

        master.onValueChange.AddListener(ChangeMaster);
        music.onValueChange.AddListener(ChangeMusic);
        soundsFx.onValueChange.AddListener(ChangeSFX);
        
        audioManagerSettings.masterVolume = Mathf.Lerp(-80, 0, master.GetValue());
        audioManagerSettings.musicVolume = Mathf.Lerp(-80, 0, music.GetValue());
        audioManagerSettings.sfxVolume = Mathf.Lerp(-80, 0, soundsFx.GetValue());
    }

    private void ChangeMaster(float value)
    {
        audioManagerSettings.masterVolume = Mathf.Lerp(-80, 0, value);
    }
    
    private void ChangeMusic(float value)
    {
        audioManagerSettings.musicVolume = Mathf.Lerp(-80, 0, value);
    }

    private void ChangeSFX(float value)
    {
        audioManagerSettings.sfxVolume = Mathf.Lerp(-80, 0, value);
    }

    private void CancelChanges()
    {
        master.SetValue(defaultMaster);
        music.SetValue(defaultMusic);
        soundsFx.SetValue(defaultSfx);

        HidePopup();
    }

    private void AcceptChanges()
    {
        defaultMaster = master.GetValue();
        defaultMusic = music.GetValue();
        defaultSfx = soundsFx.GetValue();
        HidePopup();
    }
    
    public void ShowPopup()
    {
        defaultMaster = master.GetValue();
        defaultMusic = music.GetValue();
        defaultSfx = soundsFx.GetValue();
        
        onPanelShow.Invoke();
    }

    public void HidePopup()
    {
        onPanelHide.Invoke();
    }
}
