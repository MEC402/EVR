using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSliceManager : MonoBehaviour {
	public AudioClip[] restSlices;
	public AudioClip[] fastSlices;
	public AudioClip[] slowSlices;

	public Camera headset;
	public float locationDifference;

	private AudioSource[] audioTracks;

	// Used to calculate rotation speed.
	private Vector3 headsetPosition;
	private float lastLocation;
	private float currentLocation;

	//manages what track is played from which array.
	public int restTrack;
	public int fastTrack;
	public int slowTrack;

	// Use this for initialization
	void Start () {
		audioTracks = GetComponents<AudioSource>();
		lastLocation = 0;
		currentLocation = 0;
		locationDifference = 0;
		restTrack = 0;
		fastTrack = 0;
		slowTrack = 0;

		audioTracks[0].clip = restSlices[0];
		audioTracks[0].volume = .1f;
		audioTracks[1].volume = .1f;
		StartCoroutine(playCurrentTrack(0)); 
	}
	
	// Update is called once per frame
	void Update () {
		headsetPosition = headset.transform.rotation.eulerAngles;
		currentLocation = (headsetPosition.x + headsetPosition.y + headsetPosition.z) * 10;
		locationDifference = Mathf.Abs(currentLocation - lastLocation);
		lastLocation = currentLocation;
	}

	private IEnumerator playCurrentTrack (int trackSource)
	{
		AudioSource audio;
		if(trackSource == 0){
			audio = audioTracks[0];
		}
		else{
			audio = audioTracks[1];
		}
		audio.Play();

		if(audioTracks[trackSource].clip.Equals(restSlices[restTrack])){
			if(restTrack + 1 == restSlices.Length){
				restTrack = 0;
			}
			else{
				restTrack++;
			}
		}
		if(audioTracks[trackSource].clip.Equals(slowSlices[slowTrack])){
			if(slowTrack + 1 == slowSlices.Length){
				slowTrack = 0;
			}
			else{
				slowTrack++;
			}
		}
		if(audioTracks[trackSource].clip.Equals(fastSlices[fastTrack])){
			if(fastTrack + 1 == fastSlices.Length){
				fastTrack = 0;
			}
			else{
				fastTrack++;
			}
		}

		while (audio.isPlaying) {
			if (trackSource == 0) {
				if (locationDifference < 3) {
					audioTracks [1].clip = restSlices [restTrack];

				}
				if (locationDifference >= 3 && locationDifference < 7 ) {
					audioTracks [1].clip = slowSlices [slowTrack];

				}
				if (locationDifference >= 7) {
					audioTracks [1].clip = fastSlices [fastTrack];

				}
			}
			else {
				if (locationDifference < 3) {
					audioTracks [0].clip = restSlices [restTrack];

				}
				if (locationDifference >= 3 && locationDifference < 7) {
					audioTracks [0].clip = slowSlices [slowTrack];

				}
				if (locationDifference >= 7) {
					audioTracks [0].clip = fastSlices [fastTrack];
				}
			}
			yield return null;
		}

		if(trackSource == 0){
			StartCoroutine(playCurrentTrack(1));
		}
		else {
			StartCoroutine(playCurrentTrack(0));
		}
	}
}
