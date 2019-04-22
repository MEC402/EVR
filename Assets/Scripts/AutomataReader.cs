using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomataReader{

	private string[] lines;
	private StateMachine sm;

	private StateMachine.State initial;
	private List<StateMachine.State> states;

	public AutomataReader(string filename){
		this.sm = null;
		this.initial = null;
		this.states = new List<StateMachine.State>();
		this.lines = System.IO.File.ReadAllLines(filename);
	}

	private void build_state_machine(){
		if (this.lines.Length == 0){
			Debug.Log("error reading the file, lines.length == 0");
			return;
		}

		int i = 0;
		if (!lines[i].Equals("STATES")){
			Debug.Log("error reading the head, head != 'STATES'");
			return;
		}
		i++;

		while (lines[i].Equals("INITIAL")){
			this.states.Add(new StateMachine.State(lines[i]));
			i++;
		}
		i++;

		this.initial = find_state(lines[i]);
		i++;


		while (!lines[i].Equals("END")){
			string [] transition = lines[i].Split(',');
			StateMachine.State src = find_state(transition[0]);
			StateMachine.State dst = find_state(transition[2]);

			src.addTransition(transition[1], dst);

			i++;
		}

		this.sm = new StateMachine(this.states, this.initial);

	}


	public StateMachine getStateMachine(){
		if (sm == null){
			// print error
			return null;
		}
		return this.sm;
	}


	private StateMachine.State find_state(string name){
		for (int i=0; i<this.states.Count; i++){
			if (this.states[i].getName().Equals(name))
				return this.states[i];
		}
		return null;
	}

}
