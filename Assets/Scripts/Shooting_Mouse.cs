using UnityEngine;
using System.Collections;

public class Shooting_Mouse : MonoBehaviour
{
	public GameObject target;
	GameObject player;

	float t;
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		string msg_update = "\nScript_Aiming.Update()\n";

		RaycastHit hit;

		//Debug.Log ("Checking for hit");
		Vector3 right = transform.TransformDirection (Vector3.right);
		Vector3 left = transform.TransformDirection (Vector3.left);
		Vector3 up = transform.TransformDirection (Vector3.up);
		Vector3 down = transform.TransformDirection (Vector3.down);
		//Debug.Log ("Value of forward: " + right);
		
		// mouse_hits
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown (0)) {
			Transform object_over = Check_Over (ray);
			if (object_over != null) {
				if (object_over.tag.Equals ("click_target")) {
					object_over.gameObject.BroadcastMessage("On_Target_Clicked");
					// send message to player
					// send message to click target generator
				} else {
					// Do something else for another type of object
					Debug.Log("Detected click on object but not click target");
				}
			} 
		}

		Debug.Log (msg_update);
	}

	Transform Check_Over(Ray ray){
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			Debug.Log ("Click on object");
			//This_Target.GetComponent<Controller_Prefab_Target> ().Play_Audio ();
			//hit.collider.gameObject.GetComponent<Controller_Prefab_Target> ().Dissolve ();
			//gameObject.GetComponent<Collider> ().isTrigger = true;
			Debug.Log ("Name of hit object is " + hit.collider.gameObject.name);
			//Destroy (hit.collider.gameObject);
			return hit.collider.gameObject.transform;
		}
		return null;
	}
		

	float Get_Distance_Between_Positions(Vector3 pos_1, Vector3 pos_2){
		// pos_2 = mousePosition
		Vector3 mouse_world = Camera.main.ScreenToWorldPoint (pos_2);
		return Vector3.Distance (pos_1, pos_2);
	}
		
	void OnMouseOver(){
		Debug.Log ("\n\n\tMOUSE OVER");
	}

}

