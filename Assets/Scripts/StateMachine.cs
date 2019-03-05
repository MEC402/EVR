using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {

	public enum Event_names {
		Rest,
		Slow_pan,
		Fast_pan
	}

	public enum State_names{
		Rest_68,
		Rest_72,
		Slow_72,
		Fast_72,
		Fast_76
	}

	State current_state;
	List<State> states;




	// Use this for initialization
	void Start () {
		build_state_machine();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void build_state_machine(){
		State rest_68 = new State(State_names.Rest_68);
		State rest_72 = new State(State_names.Rest_72);
		State slow_72 = new State(State_names.Slow_72);
		State fast_72 = new State(State_names.Fast_72);
		State fast_76 = new State(State_names.Fast_76);

		// STATE REST_68
		rest_68.addTransition(Event_names.Slow_pan, rest_72);
		rest_68.addTransition(Event_names.Fast_pan, fast_72);
		rest_68.addTransition(Event_names.Rest, rest_68);
		this.states.Add(rest_68);

		// STATE REST_72
		rest_72.addTransition(Event_names.Slow_pan, slow_72);
		rest_72.addTransition(Event_names.Fast_pan, fast_76);
		rest_72.addTransition(Event_names.Rest, rest_68);
		this.states.Add(rest_72);

		// STATE SLOW_72
		slow_72.addTransition(Event_names.Slow_pan, slow_72);
		slow_72.addTransition(Event_names.Fast_pan, fast_76);
		slow_72.addTransition(Event_names.Rest, rest_68);
		this.states.Add(slow_72);

		// STATE FAST_72
		fast_72.addTransition(Event_names.Slow_pan, slow_72);
		fast_72.addTransition(Event_names.Fast_pan, fast_76);
		fast_72.addTransition(Event_names.Rest, rest_68);
		this.states.Add(fast_72);

		// STATE FAST_76
		fast_76.addTransition(Event_names.Slow_pan, slow_72);
		fast_76.addTransition(Event_names.Fast_pan, fast_76);
		fast_76.addTransition(Event_names.Rest, rest_72);
		this.states.Add(fast_76);

		// Set initial state
		this.current_state = rest_68;
	}

	private class State{

		State_names name;
		Dictionary<Event_names, State> transitions = new Dictionary<Event_names, State>();

		public State(State_names name){
			this.name = name;
		}

		public void addTransition(Event_names e, State s){
			this.transitions.Add(e,s);
		}

		public State process_event(Event_names e){
			return transitions[e];
		}
	}
}
