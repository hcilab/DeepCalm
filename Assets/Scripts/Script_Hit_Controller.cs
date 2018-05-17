using UnityEngine;
using System.Collections;

public class Script_Hit_Controller : MonoBehaviour
{
	bool started = false;
	public float ray_left_right_distance = 0.0f;
	public float ray_up_down_distance = 0.0f;

	// Use this for initialization
	void Start ()
	{
		ray_left_right_distance = GameObject.FindGameObjectWithTag ("player").GetComponent<Transform> ().localScale.x + 0.1f;
		ray_up_down_distance = GameObject.FindGameObjectWithTag ("player").GetComponent<Transform> ().localScale.y + 0.1f;

	}
	
	// Update is called once per frame
	void Update ()
	{
		string msg_update = "Update()\n";
		Vector3 right = transform.TransformDirection (Vector3.right);
		Vector3 left = transform.TransformDirection (Vector3.left);
		Vector3 up = transform.TransformDirection (Vector3.up);
		Vector3 down = transform.TransformDirection (Vector3.down);

		GameObject co = GameObject.Find("Main Camera");
		Camera c = co.GetComponent<Camera> ();
		RaycastHit hit;
		Ray ray = c.ScreenPointToRay(transform.position);

		if (Physics.Raycast (transform.position, right, out hit, ray_left_right_distance)||Physics.Raycast (transform.position, left, out hit, 2)) {
			//Debug.Log ("Ray: " + Physics.Raycast (transform.position, right, 2));
			if (hit.collider.gameObject.transform.tag.Equals ("target")) {
				msg_update += "Hit RIGHT";
				Destroy (hit.collider.gameObject);
			}
		}
		if (Physics.Raycast (transform.position, left, out hit, ray_left_right_distance) || Physics.Raycast (transform.position, left, out hit, 2)) {
			//Debug.Log ("Ray: " + Physics.Raycast (transform.position, right, 2));
			if (hit.collider.gameObject.name.Contains ("target")) {
				Debug.Log("Hit on target: LEFT");
				Debug.Log ("Name of hit object is " + hit.collider.gameObject.name);
				Destroy (hit.collider.gameObject);
			}
		}
		if (Physics.Raycast (transform.position, up, out hit, ray_up_down_distance)) {
			if (hit.collider.gameObject.name.Contains ("target")) {
				Debug.Log ("Hit on target: UP");
				Debug.Log ("Name of hit object is " + hit.collider.gameObject.name);
				Destroy (hit.collider.gameObject);
			}
		}
		if (Physics.Raycast (transform.position, down, out hit, ray_up_down_distance)) {
			if (hit.collider.gameObject.name.Contains ("target")) {
				Debug.Log("Hit on target: DOWN");

				Debug.Log ("Name of hit object is " + hit.collider.gameObject.name);
				Destroy (hit.collider.gameObject);
			}
		}
	}

	public void OnTriggerEnter(Collider other){
		string ote_msg = "OnTriggerEnter()\n";
		ote_msg = string.Format ("Collision with Object: {0}\n", transform.name);

		if (started) {
			ote_msg += string.Format ("\tName of object that hit me: {0}\n", other.transform.name);
			ote_msg += string.Format ("\tDestroying the object\n");
			Destroy (other.gameObject);
		} else {
			ote_msg += "Collided with object just have not started";
		}
		Debug.Log (ote_msg);
	}
}

