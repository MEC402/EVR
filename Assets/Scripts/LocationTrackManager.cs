using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrackManager : MonoBehaviour {
	public Camera headset;

    public Vector3 headsetRotation;
	public AudioSource[] audioManager;

    private int fadeRate = 7;
    private int fadeRateMultiplier = 2;


    private int source;
	private bool mosqueIsPlaying = false;
    private bool peopleMoving = false;

	void Start () {
		audioManager = GetComponents<AudioSource>();
		source = 2;
	}
	
	// Update is called once per frame
	void Update () {

        headsetRotation = headset.transform.rotation.eulerAngles;

        //Mosque
        if (mosqueIsPlaying == false && (250f < headsetRotation.y && headsetRotation.y < 330f)){
			int random = Random.Range(0,2);
			StopAllCoroutines();
		    mosqueIsPlaying = true;
			fadeIn(audioManager[random]);
			fadeOut(audioManager[2]);
			fadeOut(audioManager[3]);
        }

        if(mosqueIsPlaying == true && (headsetRotation.y < 250f || 330f < headsetRotation.y))
        {
			StopAllCoroutines();
			fadeOut(audioManager[0]);
			fadeOut(audioManager[1]);
			mosqueIsPlaying = false;
        }
		
        //People
		if(peopleMoving == false && (140f < headsetRotation.y && headsetRotation.y < 220f))
        {
			StopAllCoroutines();
          	peopleMoving = true;
			fadeIn(audioManager[2]);
			fadeOut(audioManager[0]);
			fadeOut(audioManager[1]);
			fadeOut(audioManager[3]);
        }		
        if (peopleMoving == true && (140f > headsetRotation.y || 230f < headsetRotation.y))
        {
			StopAllCoroutines();
			fadeOut(audioManager[2]);
			fadeOut(audioManager[3]);
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
		yield return new WaitForSeconds(audio.clip.length - audio.clip.length / 4);
		if(audio.clip.Equals(audioManager[0].clip)){
			fadeIn(audioManager[1]);
			fadeOut(audioManager[0]);
		}
		else if(audio.clip.Equals(audioManager[1].clip)){
			fadeIn(audioManager[0]);
			fadeOut(audioManager[1]);
		}
		else if(audio.clip.Equals(audioManager[2].clip) && source == 2){
			source++;
			fadeIn(audioManager[3]);
			fadeOut(audioManager[2]);
		}
		else{
			source--;
			fadeIn(audioManager[2]);
			fadeOut(audioManager[3]);
		}

	}

	private IEnumerator fadeTrackOut(AudioSource audio){
		while(audio.volume > 0){
			audio.volume -= Time.deltaTime / fadeRate * fadeRateMultiplier;
			yield return null;
		}    	
	}
}
