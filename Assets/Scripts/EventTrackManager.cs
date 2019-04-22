using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrackManager : MonoBehaviour {

	public Camera headset;

	public Vector3 headsetRotation;
	public float rotation_speed;
	private Vector3 current_headset_rotation;
	private float fast_rotation = 2.0f;

	public AudioSource[] audioManager;
	private int source;

	public StateMachine msm;

	private int rest_68 = 0;
	private int rest_72 = 1;
	private int slow_72 = 2;
	private int fast_72 = 3;
	private int fast_76 = 4;

	// Use this for initialization
	void Start () {
		audioManager = GetComponents<AudioSource>();
		source = rest_68;

		headsetRotation = headset.transform.rotation.eulerAngles;
		current_headset_rotation = headset.transform.rotation.eulerAngles;
		rotation_speed = 0.0f;

		msm = new StateMachine();
		//Debug.Log(msm);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
			rotation_speed = 0.0f;
		}
		else if (Input.GetKey(KeyCode.S)){
			rotation_speed = 1.0f;
		}
		else if (Input.GetKey(KeyCode.D)){
			rotation_speed = 2.0f;
		}

		if (!audioManager[source].isPlaying){
		 	current_headset_rotation = headset.transform.rotation.eulerAngles;
			//rotation_speed = Mathf.Sqrt((headsetRotation.x-current_headset_rotation.x)*(headsetRotation.x-current_headset_rotation.x) +
//										(headsetRotation.y-current_headset_rotation.y)*(headsetRotation.y-current_headset_rotation.y));

			



			// Trigger the event
			if (rotation_speed == 0.0f) {
				msm.process_event(StateMachine.Event_names.Rest);
			}
			else if (rotation_speed > 0.0f && rotation_speed < fast_rotation) {
				msm.process_event(StateMachine.Event_names.Slow_pan);
			}
			else if (rotation_speed >= fast_rotation) {
				msm.process_event(StateMachine.Event_names.Fast_pan);
			}

			// Play correct track
			StateMachine.State current_state = msm.getCurrentState();
			stop_tracks();

			switch(current_state.getName()){
				case StateMachine.State_names.Rest_68:
					source = rest_68;
					break;
				case StateMachine.State_names.Rest_72:
					source = rest_72;
					break;
				case StateMachine.State_names.Slow_72:
					source = slow_72;
					break;
				case StateMachine.State_names.Fast_72:
					source = fast_76;
					break;
				case StateMachine.State_names.Fast_76:
					source = fast_76;
					break;
			}
			audioManager[source].Play();
		}
		
	}

	void stop_tracks(){
		foreach ( AudioSource track in audioManager){
			track.Stop();
		}
	}
}
