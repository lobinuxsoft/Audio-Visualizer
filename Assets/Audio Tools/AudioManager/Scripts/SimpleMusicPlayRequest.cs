using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMusicPlayRequest : MonoBehaviour {

    [SerializeField] bool playOnStart = false;

    [SerializeField] string audioName;

	// Use this for initialization
	void Start () {
        if (playOnStart)
        {
           Play();
        }
	}

	public void Play()
	{
		AudioManager.Instance.PlayMusic(audioName);
	}

	public void Stop()
	{
		AudioManager.Instance.StopMusic();
	}
}
