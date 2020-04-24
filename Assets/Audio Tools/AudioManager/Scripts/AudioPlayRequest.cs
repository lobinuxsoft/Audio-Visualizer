using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayRequest : MonoBehaviour {

    [SerializeField] bool playOnStart = false;

    [SerializeField] AudioClip clip;

	// Use this for initialization
	void Start () {
        if (playOnStart)
        {
           AudioManager.Instance.PlayMusic(clip);
        }
	}
}
