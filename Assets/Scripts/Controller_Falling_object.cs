using UnityEngine;
using System.Collections;

public class Controller_Falling_object : MonoBehaviour
{
	Vector3 move_direction;
	float fall_rate;
	// Use this for initialization
	void Start ()
	{
		move_direction = Vector3.zero;
		fall_rate = -1f;

	}
	
	// Update is called once per frame
	void Update ()
	{
		move_direction += new Vector3 (Input.GetAxis ("Horizontal"), fall_rate, 0);
		gameObject.GetComponent<Transform>().transform.position = move_direction;
		Debug.Log ("Falling object x value is " + gameObject.transform.position.x);
	}
}

