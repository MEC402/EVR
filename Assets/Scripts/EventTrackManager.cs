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
	private int r0 = 0;
	private int r1 = 1;
	private int r2 = 2;

	// Audio source for slow states
	private int s0 = 3;
	private int s1 = 4;
	private int s2 = 5;
	private int s3 = 6;

	// Audio source for fast states
	private int f0 = 7;
	private int f1 = 8;
	private int f2 = 9;
	private int f3 = 10;
	private int f3a = 11;

	private bool alternate = false;

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
		source = r0;
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
				case "R0":
						source = r0;
						break;
				case "R1":
						source = r1;
						break;
				case "R2":
						source = r2;
						break;
				case "S0":
						source = s0;
						break;
				case "S1":
						source = s1;
						break;
				case "S2":
						source = s2;
						break;
				case "S3":
						source = s3;
						break;
				case "F0":
						source = f0;
						break;
				case "F1":
						source = f1;
						break;
				case "F2":
						source = f2;
						break;
				case "F3":
						if (alternate)
							source = f3a;
						else
							source = f3;
						alternate = !alternate;
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
