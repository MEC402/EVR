using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrackManager : MonoBehaviour {
	public Camera headset;

    public Vector3 headsetRotation;
	public AudioClip[] tracks = new AudioClip[3];
	public AudioSource audio;

    private int fadeRate = 5;
    private int fadeRateMultiplier = 2;

	private int randomTrack;
	private bool mosqueIsPlaying = false;
    private bool peopleMoving = false;

	void Start () {
	audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        headsetRotation = headset.transform.rotation.eulerAngles;

        //Mosque
        if (mosqueIsPlaying == false && (260f < headsetRotation.y && headsetRotation.y < 330f)){
			StopAllCoroutines();
		    mosqueIsPlaying = true;
		    randomTrack = Random.Range(0,2);
		    audio.clip = tracks[randomTrack];
            fadeIn(audio);
        }

        if(mosqueIsPlaying == true && (headsetRotation.y < 260 || 330 < headsetRotation.y))
        {
        	StopAllCoroutines();
			fadeOut(audio);
			mosqueIsPlaying = false;
        }
		
        //People
		if(peopleMoving == false && (180f < headsetRotation.y && headsetRotation.y < 220f))
        {
			StopAllCoroutines();
          	peopleMoving = true;
			audio.clip = tracks[2];
			fadeIn(audio);
        }		
        if (peopleMoving == true && (180 > headsetRotation.y || 230 < headsetRotation.y))
        {
			StopAllCoroutines();
			fadeOut(audio);
			peopleMoving = false;
        }
    }

	private void fadeIn(AudioSource audio){
		StartCoroutine(fadeTrackIn(audio)); 
	}

	private void fadeOut(AudioSource audio)
    {
        StartCoroutine(fadeTrackOut(audio));
    }
    private IEnumerator fadeTrackIn (AudioSource audio)
	{
		audio.volume = 0f;
        audio.Play ();
		while(audio.volume < 1){

			audio.volume += Time.deltaTime / fadeRate;
			yield return null;
		}
		yield return new WaitForSeconds(audio.clip.length);
		if(audio.clip.Equals(tracks[0])){
			audio.clip = tracks[1];
		}
		else if(audio.clip.Equals(tracks[1])){
			audio.clip = tracks[0];
		}
		fadeIn(audio);
	}

	private IEnumerator fadeTrackOut(AudioSource audio){
		while(audio.volume > 0){
			audio.volume -= Time.deltaTime / fadeRate * fadeRateMultiplier;
			yield return null;
		}    	
	}
}
