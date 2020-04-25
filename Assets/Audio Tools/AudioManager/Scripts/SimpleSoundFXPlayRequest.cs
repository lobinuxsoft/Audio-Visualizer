using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundFXPlayRequest : MonoBehaviour {

    [SerializeField] bool playOnStart = false;
    [SerializeField] bool playOnLoop = false;

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
		if (playOnLoop)
		{
			AudioManager.Instance.PlaySfxInLoop(audioName);
		}
		else
		{
			AudioManager.Instance.PlaySfx(audioName);
		}
	}

	public void Stop()
	{
		AudioManager.Instance.StopSfxInLoop(audioName);
	}
}
