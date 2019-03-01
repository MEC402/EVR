using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcCameraMovement : MonoBehaviour {
	private float horizontalMovement;
	private float verticalMovement;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float xMovement =  Input.GetAxis("Mouse X") ;
		float yMovement = Input.GetAxis("Mouse Y") * -1;
		transform.eulerAngles += new Vector3( yMovement, xMovement, 0.0f);
	}
}
