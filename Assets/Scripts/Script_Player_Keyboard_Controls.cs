using UnityEngine;
using System.Collections;

public class Script_Player_Keyboard_Controls : MonoBehaviour
{
	public float speed_horizontal;
	public float speed_vertical;
	bool started = false;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButtonDown(0)){
			started = true;
		}

		if (Input.anyKey) {
			// Debug.Log ("Any key down");
			if (Input.GetKey (KeyCode.UpArrow)) {
				gameObject.GetComponent<CharacterController> ().Move (new Vector3 (0, speed_vertical * Time.deltaTime, 0));

			} 

			if (Input.GetKey(KeyCode.DownArrow)) {
				gameObject.GetComponent<CharacterController> ().Move (new Vector3 (0, (speed_vertical * Time.deltaTime) * -1, 0));
			} 

			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("Space bar down");
				speed_horizontal = speed_horizontal * -1;
			}

			if (Input.GetKeyDown (KeyCode.S)) {
				Debug.Log ("S has been pressed");
				GameObject GUI_controller = GameObject.Find ("GUI_Controller");
				GUI_controller.SendMessage ("Hide", GameObject.FindGameObjectWithTag ("GUI_health_bar"));
				GUI_controller.SendMessage ("Hide", GameObject.Find ("GUI_Dialogue"));
				GameObject.FindGameObjectWithTag ("player").SendMessage ("Start_Moving");
			}
		}
	}

	public void Set_Start(bool b){
		started = true;
	}
}

