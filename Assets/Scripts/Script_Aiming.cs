using UnityEngine;
using System.Collections;

public class Script_Aiming : MonoBehaviour
{
	public Material material_clock_face;
	public Material material_aim;

	public Vector3 speed_rotation;

	Transform pivot_aim;
	Transform aim;

	private LineRenderer aim_line;
	private Camera aim_camera;
	GameObject[] aiming_pieces;
	public GameObject aim_end;
	public GameObject target;
	GameObject player;

	float t;
	float Seconds_To_Lerp;
	void Start ()
	{
		string msg_start = "\nScript_Clock.Start()\n";
		aim_camera = GetComponent<Camera> ();

		aiming_pieces = GameObject.FindGameObjectsWithTag ("aiming");
		if (aiming_pieces.Length == 0) {
			msg_start += "\n\t*** Error: aiming_pieces cannot be 0\n";
		}
		msg_start += "\taiming_pieces.Length: " + aiming_pieces.Length + "\n";
		GameObject go_aim = GameObject.Find ("Prefab_Aiming/Aim");
		aim = go_aim.transform;
		GameObject go_pivot_aim = GameObject.Find ("Prefab_Aiming");
		pivot_aim = go_pivot_aim.transform;
		//gameObject.GetComponent<MeshRenderer> ().material = material_clock_face;
		//aim.gameObject.GetComponent<MeshRenderer> ().material = material_aim;
		msg_start += "\npivot_aim.transform.rotation" + this.pivot_aim.gameObject.transform.rotation + "\n";
		t = 0.0f;

		aim_line = GetComponent<LineRenderer> ();
		Debug.Log (msg_start);
	}
	
	// Update is called once per frame
	void Update ()
	{
		string msg_update = "\nScript_Aiming.Update()\n";
		if (Input.GetMouseButtonDown (0)) {
			msg_update += "\tPasting new target\n";
			GameObject new_target = Instantiate (target);
			new_target.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObject.transform.position.z));
			new_target.transform.position = new Vector3 (new_target.transform.position.x, new_target.transform.position.y, 0);
		}
		Vector3 ray_origin = aim.transform.position;

		aim_line.SetPosition (0, Input.mousePosition);
		RaycastHit hit;

		//Debug.Log ("Checking for hit");
		Vector3 right = transform.TransformDirection (Vector3.right);
		Vector3 left = transform.TransformDirection (Vector3.left);
		Vector3 up = transform.TransformDirection (Vector3.up);
		Vector3 down = transform.TransformDirection (Vector3.down);
		//Debug.Log ("Value of forward: " + right);
		Debug.DrawRay (transform.position, Vector3.forward, Color.green);
		if (Physics.Raycast (transform.position, right, out hit, 2)||Physics.Raycast (transform.position, left, out hit, 2)) {
			//Debug.Log ("Ray: " + Physics.Raycast (transform.position, right, 2));
			if (hit.collider.gameObject.name.Contains ("target")) {
				msg_update += "Hit on target";
				hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
				msg_update += "\tName of hit object is " + hit.collider.gameObject.name + "\n";
				Destroy (hit.collider.gameObject);
			}
			if (hit.collider.gameObject.name.Contains ("under")) {
				Debug.Log ("Hit on under");
				//This_Target.GetComponent<Controller_Prefab_Target> ().Play_Audio ();
				//hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
				//gameObject.GetComponent<Collider> ().isTrigger = true;
				//Debug.Log ("Name of missed object is " + hit.collider.gameObject.name);
				//Destroy (hit.collider.gameObject);
			}
			if (hit.collider.gameObject.name.Contains ("over")) {
				Debug.Log ("Hit on over");
				//This_Target.GetComponent<Controller_Prefab_Target> ().Play_Audio ();
				//hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
				//gameObject.GetComponent<Collider> ().isTrigger = true;
				//Debug.Log ("Name of missed object is " + hit.collider.gameObject.name);
				//Destroy (hit.collider.gameObject);
			}
		}
		if (Physics.Raycast (transform.position, down, out hit, 20)) {
			if (hit.collider.gameObject.name.Contains ("under")) {
				Debug.Log ("Miss on under");
				//This_Target.GetComponent<Controller_Prefab_Target> ().Play_Audio ();
				//hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
				//gameObject.GetComponent<Collider> ().isTrigger = true;
				Debug.Log ("Name of hit object is " + hit.collider.gameObject.name);
				//Destroy (hit.collider.gameObject);
			}
			else if(hit.collider.gameObject.name.Contains("over")){
			}
		}

		if (Physics.Raycast (transform.position, up, out hit, 20)) {
			if (hit.collider.gameObject.name.Contains ("over")) {
				Debug.Log ("Miss on over");
				//This_Target.GetComponent<Controller_Prefab_Target> ().Play_Audio ();
				//hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
				//gameObject.GetComponent<Collider> ().isTrigger = true;
				Debug.Log ("Name of hit object is " + hit.collider.gameObject.name);
				//Destroy (hit.collider.gameObject);
			}
			else if(hit.collider.gameObject.name.Contains("under")){
			}
		}
		
		Rotate_Towards_Mouse ();
		Debug.Log (msg_update);
	}

	void Rotate_Towards_Mouse(){
		string rotate_towards_mouse = "\nScript_Aiming.Rotate_Towards_Mouse()\n";
		Vector3 mouse_pos = Input.mousePosition;
		rotate_towards_mouse += "\tmouse_po: " + mouse_pos + "\n";
		Vector3 pivot_aim_screen_pos = Camera.main.WorldToScreenPoint (pivot_aim.position);
		rotate_towards_mouse += "\tobject_po: " + pivot_aim_screen_pos + "\n";

		float angle = Mathf.Atan2((Input.mousePosition.z - pivot_aim_screen_pos.z), (Input.mousePosition.x - pivot_aim_screen_pos.x)) * Mathf.Rad2Deg;
		rotate_towards_mouse += "\tangle: " + angle + "\n";
		rotate_towards_mouse += "\tpivot_aim.transform.localEulerAngles: " + pivot_aim.transform.localEulerAngles + "\n";
		pivot_aim.transform.localEulerAngles = new Vector3(0,0,270 - angle);
		rotate_towards_mouse += "\tupdated pivot_aim.transform.localEulerAngles: " + pivot_aim.transform.localEulerAngles + "\n";
		//Debug.Log (rotate_towards_mouse);
	}

	public void Rotate_Clockwise(){
		string msg_rotate_clockwise = "\nScript_Clock.Rotate_Clockwise()\n";
		// Operate on this.aim;
		msg_rotate_clockwise += "\tthis.pivot_aim:\n\t\trotation: " + this.pivot_aim.rotation.eulerAngles + "\n";
		Vector3 new_rotation = (this.pivot_aim.rotation.eulerAngles);
		msg_rotate_clockwise += "\nspeed_rotation.y * Time.deltaTime: " + (360 * Time.deltaTime) + "\n";
		new_rotation += new Vector3(0, 0, 360 * Time.deltaTime);
		this.pivot_aim.eulerAngles = new Vector3(new_rotation.x, 180, new_rotation.z);
		msg_rotate_clockwise += "\tAfter rotation:\n\t\tthis.pivot_aim:\n\t\trotation: " + this.pivot_aim.rotation.eulerAngles + "\n";
		//Debug.Log (msg_rotate_clockwise);
	}

	public void Rotate_Counter_Clockwise(){
		string msg_rotate_counter_clockwise = "\nScript_Clock.Rotate_Counter_Clockwise()\n";
		// Operate on this.aim;
		msg_rotate_counter_clockwise += "\tthis.pivot_aim:\n\t\trotation: " + this.pivot_aim.rotation.eulerAngles + "\n";
		Vector3 new_rotation = (this.pivot_aim.rotation.eulerAngles);
		msg_rotate_counter_clockwise += "\nspeed_rotation.y * Time.deltaTime: " + (360 * Time.deltaTime) + "\n";
		new_rotation += new Vector3(0, 0, 360 * Time.deltaTime);
		this.pivot_aim.eulerAngles = new Vector3(new_rotation.x, 180, new_rotation.z);
		msg_rotate_counter_clockwise += "\tAfter rotation:\n\t\tthis.pivot_aim:\n\t\trotation: " + this.pivot_aim.rotation.eulerAngles + "\n";
		Debug.Log (msg_rotate_counter_clockwise);
	}

	public void Stop_Clock(){
		Destroy (gameObject);
	}
}

