using UnityEngine;
using System.Collections;

public class Script_Shooting : MonoBehaviour
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


		// Player hits
		Debug.DrawRay (transform.position, Vector3.forward, Color.green);
		if (Physics.Raycast (transform.position, right, out hit, 2)||Physics.Raycast (transform.position, left, out hit, 2)) {
			//Debug.Log ("Ray: " + Physics.Raycast (transform.position, right, 2));
			if (hit.collider.gameObject.name.Contains ("target")) {
				msg_update += "Hit on target";
				hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
				msg_update += "\tName of hit object is " + hit.collider.gameObject.name + "\n";
				Destroy (hit.collider.gameObject);
			}
		}
		
		// mouse_hits
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Transform object_over = Check_Over (ray, "target_hit");
		if (object_over != null) {
			Debug.Log ("Over registered. object_over.transform.name: " + object_over.transform.name + "\n");
		}


		Rotate_Towards_Mouse ();
		Debug.Log (msg_update);
	}

	Transform Check_Over(Ray ray, string name_prefix){

		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			Debug.Log ("Hit ON TARGET");
			//This_Target.GetComponent<Controller_Prefab_Target> ().Play_Audio ();
			//hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
			//gameObject.GetComponent<Collider> ().isTrigger = true;
			Debug.Log ("Name of hit object is " + hit.collider.gameObject.name);
			//Destroy (hit.collider.gameObject);
			return hit.collider.gameObject.transform;
		}
		return null;
	}

	void OnMouseOver(){
		Debug.Log ("\n\n\tMOUSE OVE");
	}

	void Shoot(Camera cam, Transform transform, Transform transform_player){

		return;
	}

	void Check_For_Click_On(Camera cam, Vector3 position_mouse, Vector3 position_target){
		return;
	}

	void Check_Over(Vector3 position_mouse, Vector3 position_target){
		return;
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

