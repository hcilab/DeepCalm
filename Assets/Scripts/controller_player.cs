using UnityEngine;
using System.Collections;

public class controller_player : MonoBehaviour {
	[Range(0, 100)]
	public float speed;
	public float jumpSpeed = 8.0f;
	float gravity;
	GameObject Aura;
	private Vector3 moveDirection = Vector3.zero;
	[Range(5, 20)]
	public int x_max = 10;
	[Range(5, 20)]
	public int x_min = 7;
	[Range(5, 20)]
	public int y_max = 10;
	[Range(5, 20)]
	public int y_min = 7;
	[Range(5, 20)]
	public int z_max = 10;
	[Range(5, 20)]
	public int z_min = 7;
	private bool can_move = false;
	int counter;
	private Vector3 character_size;
	private float size_delta; //used by Set_Speed
	private float max_strength;
	private float strength; //user by grow_strong
	CharacterController controller;


	void start(){
		max_strength = 10;
		size_delta = 0;
		strength = 1;
		gravity = 8;
		gameObject.GetComponent<CharacterController> ().Move (Vector3.zero);

	}

	void Update() {
		//Aura = GameObject.Find("aura");
		breath ();
		//Set_Speed ('x', gameObject.transform.position.x);
		//controller = gameObject.GetComponent<CharacterController>();
		Debug.Log("Checking grounded first time " + gameObject.GetComponent<CharacterController>().isGrounded);
		if (!gameObject.GetComponent<CharacterController>().isGrounded) {
			Debug.Log ("in the air\tdeltaTime is "+Time.deltaTime);
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (gravity*Time.deltaTime), gameObject.transform.position.y);
		}
		Debug.Log ("Speed " + speed);
		if (Input.GetKeyDown (KeyCode.G)) {
			counter++;
			Debug.Log ("G key is down " + counter);
			float unsigned_delta = 1.2f;
			if (strength + unsigned_delta <= max_strength) {
				strength = strength + unsigned_delta;
			} else {
				strength = strength - unsigned_delta;
			}
		}

		Vector3 dir = new Vector3 ();
		dir = Vector3.zero;
		//dir.x = Input.acceleration.y;
		dir.y = Input.acceleration.x;
		if (dir.sqrMagnitude > 1) {
			dir.Normalize ();
		}
		dir.y *= Time.deltaTime;

		UnityEngine.Debug.Log("Arrow press? " + Input.GetKeyDown(KeyCode.LeftArrow));

		//Set_Speed ('x',gameObject.transform.position.x);
		CharacterController controller = gameObject.GetComponent<CharacterController>();
		//Debug.Log("CharacaterController_slope_limit " +  controller.radius);
		//Debug.Log("Character Controller: " + controller.ToString());
		//Debug.Log ("Grounded? " + controller.isGrounded);
		if (controller.isGrounded) {
			//Debug.Log ("Am Grounded");
			//Debug.Log("Horizontal " + Input.GetAxis("Horizontal"));
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			//Debug.Log ("moveDirection " + moveDirection);
			moveDirection = transform.TransformDirection(moveDirection);
			//moveDirection *= speed*Time.deltaTime;
			if (Input.GetKeyDown (KeyCode.Space)) {
				//Debug.Log ("Jump was pressed");
				//Debug.Log ("Value of jumpspeed: " + jumpSpeed);
				moveDirection.y += jumpSpeed;
			}
		}
		moveDirection.y *= dir.y;
		//Debug.Log ("moveDirection " + moveDirection);
		controller.Move(moveDirection);

	}

	void breath(){
		//Debug.Log ("Breathing");
		float x = Mathf.Abs(Mathf.Sin (Time.time) * x_max);
		float y = Mathf.Abs(Mathf.Sin (Time.time) * y_max);
		float z = Mathf.Abs(Mathf.Sin (Time.time) * z_max);
		Vector3 breath = new Vector3 (x, y, z);
		//Debug.Log ("Breath: " + breath);
		//Debug.Log ("Value of breath:" + x + " " + y + " " + z);
		gameObject.GetComponent<Transform> ().localScale = breath;
		//GetComponent<Transform> ().transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.localScale.y * 0.5f, gameObject.transform.position.z);
		
	}
	//currently just makes you jump higher by reducing you gravity as you get higher
	void grow_strong(float unsigned_delta){

	}

	float Get_Adjusted_Speed(char axis, float pos_in){
		Debug.Log ("Getting adjusted speed");
		float current = 0f;
		float delta = 0f;
		if (axis.Equals ('x')) {
			current = gameObject.transform.localScale.x;
		} else if (axis.Equals ('y')) {
			current = gameObject.transform.localScale.y;
		} else {
			current = gameObject.transform.localScale.z;
		}

		delta = current - size_delta;
		float current_relative_speed = Mathf.Abs(delta);
		//increasing case first
		if (delta >= 0) {
			return speed;
		} else {
			float fraction = Mathf.Abs(size_delta/delta);
			return speed * fraction;
		} 

	}

	void Set_Speed(char axis, float pos_in){
		Debug.Log ("Setting speed");
		float current = 0f;
		float delta = 0f;
		if (axis.Equals ('x')) {
			current = gameObject.transform.localScale.x;
		} else if (axis.Equals ('y')) {
			current = gameObject.transform.localScale.y;
		} else {
			current = gameObject.transform.localScale.z;
		}

		delta = current - pos_in;
		float current_relative_speed = Mathf.Abs(delta);
		//increasing case first
		if (delta > 0) {
			int c = 0;
		} else if(delta < 0){
			float fraction = x_max/current;
			Debug.Log ("Value of current" + current);
			Debug.Log ("Value of fraction: " + fraction);
			speed = (speed * fraction)+speed;
		}
		//only has to be bound to one axis -> the inputed one
		//need to cancel out the affect of becreasing, increasing size.
		//need to switch when sign of delta switches

	}

	void Attack(){
		
	}
}