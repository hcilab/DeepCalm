using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller_Falling_object_UI : MonoBehaviour
{
	Vector3 move_direction;
	float fall_rate;
	Transform target;
	Camera cam;
	Image background;
	Image money;
	float background_width;
	float min_x;
	float min_y;
	float max_x;
	float max_y;
	Canvas canvas;
	Vector3 origin;
	float first_break;

	// Use this for initialization
	void Start () {			
		first_break = 100f;

		money = canvas.GetComponent<Image> ();
		origin = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,gameObject.transform.position.z);
		canvas = gameObject.GetComponent<Canvas> ();
		Image background_image = gameObject.GetComponent<Image> ();
		background_width = background_image.GetComponent<RectTransform> ().sizeDelta.x;
		min_x = gameObject.transform.position.x - background_width;
		max_x = gameObject.transform.position.y + background_width;
		Debug.Log ("Value of max_X is " + max_x + "value of min_x is " + min_x);
		cam = gameObject.GetComponentInParent<Camera>();
		move_direction = Vector3.zero;
		fall_rate = -1f;
	}
	
	// Update is called once per frame
	void Update () {
		float y = origin.y - first_break;
		if (gameObject.transform.position.y >= y) {
			GameObject copy = GetComponent<GameObject> ().gameObject;
		}
		Keep_In_Boundaries ();
	}

	void Keep_In_Boundaries(){
		//check left
		move_direction += new Vector3 (Input.GetAxis ("Horizontal"), fall_rate, 0);
		//Debug.Log ("Value of cam " + cam.ToString ());
		Debug.Log ("Value of target " + target);
		Vector3 screenPos = cam.WorldToScreenPoint (target.position);
		Debug.Log ("Target is " + screenPos.x + " pixels from the left");

		if (screenPos.x <= min_x|| screenPos.x >= max_x) {
			Debug.Log ("Applying transformation");
			move_direction += Vector3.left;
		}
		gameObject.GetComponent<Transform>().transform.position += move_direction;
		Debug.Log ("Falling object x value is " + gameObject.transform.position.x);
	}
}

