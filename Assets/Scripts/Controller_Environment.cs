using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Environment : MonoBehaviour {
	[Range(0f,20f)]
	private float Gravity = 8;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Set_Gravity(float gravity_in){
		Gravity = gravity_in;
	}

	float Get_Gravity(){
		return Gravity;
	}
}
