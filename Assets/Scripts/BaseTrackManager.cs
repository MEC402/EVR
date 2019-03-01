using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrackManager : MonoBehaviour {

	private AudioSource[] tracks;
	// Use this for initialization
	void Start () {
		tracks = GetComponents<AudioSource>();
		tracks[0].Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
