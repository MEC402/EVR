using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrackManager : MonoBehaviour {
	public GameObject camera;
	private AudioSource[] tracks;

	private int mosqueTrack;
	private bool mosqueIsPlaying = false;
	// Use this for initialization
	void Start () {
		tracks = GetComponents<AudioSource>();
		mosqueTrack = Random.Range(0,1);
		//StartCoroutine(fadeTrackIn(tracks[mosqueTrack]));
	}
	
	// Update is called once per frame
	void Update () {
		if(mosqueIsPlaying == false){
		mosqueIsPlaying = true;
		fadeIn(tracks[mosqueTrack]);
		}

		if(camera.transform.rotation.y > 50){
			camera.transform.position += new Vector3(50,50,50);
		}
	}

	public void fadeIn(AudioSource currentAudio){
		StartCoroutine(fadeTrackIn(tracks[mosqueTrack]));
	}

	private IEnumerator fadeTrackIn (AudioSource currentAudio)
	{
		currentAudio.volume = 0f;
		currentAudio.Play ();
		while(currentAudio.volume < 1){
			currentAudio.volume += Time.deltaTime / 10;
			yield return null;
		}

	}
	private IEnumerator fadeTrackOut(AudioSource currentAudio){
		while(currentAudio.volume > 0){
			currentAudio.volume -= Time.deltaTime / 1;
			yield return null;
		}
		currentAudio.Stop();
		mosqueIsPlaying = false;
	}

}
