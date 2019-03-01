using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrackManager : MonoBehaviour {
	public Camera headset;

    public Vector3 headsetRotation;
	private AudioSource[] tracks;
    public AudioSource currentClip;
	private int mosqueTrack;
    private int fadeRate = 5;
	private bool mosqueIsPlaying = false;
    private bool peopleMoving = false;


	void Start () {
		tracks = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        headsetRotation = headset.transform.rotation.eulerAngles;

        //Mosque
        if (mosqueIsPlaying == false && (260f < headsetRotation.y && headsetRotation.y < 330f)){
            
		    mosqueIsPlaying = true;
            mosqueTrack = Random.Range(0, 1);
            currentClip = tracks[mosqueTrack];
            fadeIn();
        }
        if(mosqueIsPlaying == true && (headsetRotation.y < 260 || 330 < headsetRotation.y))
        {
            StopAllCoroutines();
            fadeOut();
        }
		
        //People
        if(peopleMoving = false && (180 < headsetRotation.y && headsetRotation.y < 230))
        {
            
            currentClip = tracks[2];
            fadeIn();
        }
        if (peopleMoving = true && (180 > headsetRotation.y && headsetRotation.y < 230))
        {
            StopAllCoroutines();
            fadeOut();
        }
    }

	private void fadeIn(){
		StartCoroutine(fadeTrackIn());
	}

    private void fadeOut()
    {
        StartCoroutine(fadeTrackOut());
    }

    private IEnumerator fadeTrackIn ()
	{
		currentClip.volume = 0f;
        currentClip.Play ();
		while(currentClip.volume < 1){

            currentClip.volume += Time.deltaTime / fadeRate;
			yield return null;
		}

	}
	private IEnumerator fadeTrackOut(){
		while(currentClip.volume > 0){
            currentClip.volume -= Time.deltaTime / fadeRate *2;
			yield return null;
		}
        if(currentClip.Equals(tracks[0]) || currentClip.Equals(tracks[1]))
        {
            mosqueIsPlaying = false;
        }
        else
        {
            peopleMoving = false;
        }
       	
	}

}
