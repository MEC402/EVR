using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public Camera playerCamera;
	public AudioClip[] trackList = new AudioClip[8];
	private AudioSource[] test;

	private AudioSource audio;

	// Use this for initialization
	void Start () {
		// trackList has a size of 8
		audio.clip = trackList[0];

		audio.Play();

		test = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

	}
}


