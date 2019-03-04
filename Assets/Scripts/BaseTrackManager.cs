using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrackManager : MonoBehaviour {

	public AudioClip[] tracks;
	private AudioSource currentAudio;
	// Use this for initialization
	void Start () {
		currentAudio = GetComponent<AudioSource>();
		currentAudio.clip = tracks[0];
		currentAudio.volume = .5f;
		currentAudio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
