using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrackManager : MonoBehaviour {

	public Camera headset;

	public Vector3 headsetRotation;
	private float rotation_speed;
	private float current_headset_rotation;

	public AudioSource[] audioManager;
	private int source;

	private StateMachine msm;

	private int rest_68 = 0;
	private int rest_72 = 1;
	private int slow_72 = 2;
	private int fast_72 = 3;
	private int fast_76 = 4;

	// Use this for initialization
	void Start () {
		audioManager = GetComponents<AudioSource>();
		source = 0;

		headsetRotation = headset.transform.rotation.eulerAngles;
		current_headset_rotation = headset.transform.rotation.eulerAngles;
		rotation_speed = 0.0f;

		msm = new StateMachine();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audioManager[source].isPlaying){
		 	current_headset_rotation = headset.transform.rotation.eulerAngles;
			rotation_speed = Mathf.Sqrt((headsetRotation.x-current_headset_rotation.x)*(headsetRotation.x-current_headset_rotation.x) +
										(headsetRotation.y-current_headset_rotation.y)*(headsetRotation.y-current_headset_rotation.y));

			// Trigger the event
			if (rotation_speed == 0) {
				msm.process_event(StateMachine.Event_names.Rest);
			}
			else if (rotation_speed > 0 && rotation_speed < fast_rotation) {
				msm.process_event(StateMachine.Event_names.Slow_pan);
			}
			else {
				msm.process_event(StateMachine.Event_names.Fast_pan);
			}

			// Play correct track
			StateMachine.State current_state = msm.getCurrentState();

			switch(current_state.getName()){
				case StateMachine.State_names.Rest_68:
					audioManager[rest_68].Play();
					break;
				case StateMachine.State_names.Rest_72:
					audioManager[rest_72].Play();
					break;
				case StateMachine.State_names.Slow_72:
					audioManager[slow_72].Play();
					break;
				case StateMachine.State_names.Fast_72:
					audioManager[fast_72].Play();
					break;
				case StateMachine.State_names.Fast_76:
					audioManager[fast_76].Play();
					break;
			}
		}
		
	}
}
