using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrackManager : MonoBehaviour {

	public Camera headset;

	public Vector3 headsetRotation;
	public float rotation_speed;
	private Queue<float> rotation_speed_queue;
	private int max_queue_size = 50;
	private Vector3 prev_headset_rotation;
	[Range(0.0f, 1.5f)]
	public float slow_rotation = 0.1f;
	[Range(0.0f, 1.5f)]
	public float fast_rotation = 0.4f;

	private AudioSource[] audioManager;
	private AudioSource current_source;
	private int source;

	public StateMachine msm;

	// Audio source for rest states
	private int r1a = 0;
	private int r1b = 1;
	private int r1c = 2;
	private int r2a = 3;
	private int r2b = 4;
	private int r2c = 5;
	private int r2d = 6;
	private int r2e = 7;


	// Audio source for slow states
	private int s1a = 8;
	private int s1b = 9;
	private int s2a = 10;
	private int s2b = 11;
	private int s3a = 12;
	private int s3b = 13;
	private int s3c = 14;

	// Audio source for fast states
	private int f1a = 15;
	private int f1b = 16;
	private int f1c = 17;
	private int f1d = 18;
	private int f1e = 19;
	private int f1f = 20;
	private int f1g = 21;
	private int f1h = 22;
	private int f2a = 23;
	private int f2b = 24;
	private int f2c = 25;
	private int f2d = 26;
	private int f2e = 27;
	private int f2f = 28;
	private int f2g = 29;
	private int f2h = 30;


	[Range(0.0f, 0.2f)]
	public float track_overlap =  0.015f;
	private int fadeRate = 5;
    private int fadeRateMultiplier = 2;


	// Use this for initialization
	void Start () {
		Debug.Log("Start");
		audioManager = GetComponents<AudioSource>();
		for(int i = 0; i < audioManager.Length; i++){
			audioManager[i].volume = .1f;
		}
		source = r1a;
		this.current_source = audioManager[source];
		this.current_source.Play();

		headsetRotation = headset.transform.rotation.eulerAngles;
		this.prev_headset_rotation = headset.transform.rotation.eulerAngles;
		rotation_speed = 0.0f;
		this.rotation_speed_queue = new Queue<float>();
		this.rotation_speed_queue.Enqueue(rotation_speed);

		AutomataReader ar = new AutomataReader("Assets/Scripts/state_machine.txt");
		this.msm = ar.getStateMachine();
		//msm = new StateMachine();
		Debug.Log(msm);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
			Debug.Log("rest");
			rotation_speed = 0.0f;
		}
		else if (Input.GetKey(KeyCode.S)){
			Debug.Log("slow");
			rotation_speed = 1.0f;
		}
		else if (Input.GetKey(KeyCode.D)){
			Debug.Log("fast");
			rotation_speed = 2.0f;
		}

		this.headsetRotation = this.headset.transform.rotation.eulerAngles;
		//this.prev_headset_rotation = headset.transform.rotation.eulerAngles;
		float curr_rotation_speed = Mathf.Sqrt((headsetRotation.x-this.prev_headset_rotation.x)*(headsetRotation.x-this.prev_headset_rotation.x) +
				(headsetRotation.y-this.prev_headset_rotation.y)*(headsetRotation.y-this.prev_headset_rotation.y));


		if (this.rotation_speed_queue.Count >= this.max_queue_size){
			this.rotation_speed = this.rotation_speed - this.rotation_speed_queue.Dequeue()/this.max_queue_size;
		}

		this.rotation_speed_queue.Enqueue(curr_rotation_speed);
		this.rotation_speed = this.rotation_speed + curr_rotation_speed/this.max_queue_size;

		Debug.Log(this.rotation_speed);


		//if (!audioManager[source].isPlaying){
		//if (current_source.time / current_source.clip.length > 0.98){
		if (this.current_source.time > this.current_source.clip.length - this.track_overlap){ 
			// Trigger the event

			if (rotation_speed <= slow_rotation) {
				Debug.Log("rest");
				msm.process_event("rest");
			}
			else if (rotation_speed > slow_rotation && rotation_speed <= fast_rotation) {
				Debug.Log("slow");
				msm.process_event("pan_s");
			}
			else if (rotation_speed > fast_rotation) {
				Debug.Log("fast");
				msm.process_event("pan_f");
			}



			// Play correct track
			StateMachine.State current_state = msm.getCurrentState();
			//stop_tracks();

			switch(current_state.getName()){
				case "R1A":
						source = r1a;
						break;
				case "R1B":
						source = r1b;
						break;
				case "R1C":
						source = r1c;
						break;
				case "R2A":
						source = r2a;
						break;
				case "R2B":
						source = r2b;
						break;
				case "R2C":
						source = r2c;
						break;
				case "R2D":
						source = r2d;
						break;
				case "R2E":
						source = r2e;
						break;


				case "S1A":
						source = s1a;
						break;
				case "S1B":
						source = s1b;
						break;
				case "S2A":
						source = s2a;
						break;
				case "S2B":
						source = s2b;
						break;
				case "S3A":
						source = s3a;
						break;
				case "S3B":
						source = s3b;
						break;
				case "S3C":
						source = s3c;
						break;


				case "F1A":
						source = f1a;
						break;
				case "F1B":
						source = f1b;
						break;
				case "F1C":
						source = f1c;
						break;
				case "F1D":
						source = f1d;
						break;
				case "F1E":
						source = f1e;
						break;
				case "F1F":
						source = f1f;
						break;
				case "F1G":
						source = f1g;
						break;
				case "F1H":
						source = f1h;
						break;

				case "F2A":
						source = f2a;
						break;
				case "F2B":
						source = f2b;
						break;
				case "F2C":
						source = f2c;
						break;
				case "F2D":
						source = f2d;
						break;
				case "F2E":
						source = f2e;
						break;
				case "F2F":
						source = f2f;
						break;
				case "F2G":
						source = f2g;
						break;
				case "F2H":
						source = f2h;
						break;


				
			}
			stop_tracks();
			//fadeOut(this.current_source);
			this.current_source = this.audioManager[source];
			this.current_source.Play();
			//fadeIn(this.current_source);
			//audioManager[source].Play();


		}
		this.prev_headset_rotation = headset.transform.rotation.eulerAngles;

	}

	private void stop_tracks(){
		foreach ( AudioSource track in this.audioManager){
			track.Stop();
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
		while(audio.volume < 1.0){
			audio.volume += Time.deltaTime / fadeRate;
			yield return null;
		}
		yield return null;// new WaitForSeconds(audio.clip.length - audio.clip.length / 4);
	}

	private IEnumerator fadeTrackOut(AudioSource audio){
		while(audio.volume > 0){
			audio.volume -= Time.deltaTime / fadeRate;
			yield return null;
		}    	
	}
}
