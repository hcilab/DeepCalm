using UnityEngine;
using System.Collections;

public class Script_Strobe : MonoBehaviour
{
	public float max, min;
	public float Steps;
	int Step_Previous;
	float Step;
	public Vector4 Offset;
	public Material Material_Player;
	public char c;
	// strobe alpha
	// Use this for initialization
	Color color_max;
	Color color_min;

	void Start (){
		if (GameObject.FindGameObjectsWithTag ("player").Length > 1) {
			Destroy (this);
		}
		color_max = Material_Player.color;
		color_min = new Vector4 (color_max.r, color_max.g, color_max.b, 0.0f);
		string msg_start = "Start()\n";


		if (Material_Player == null) {
			msg_start += "\tCan't find material\n";
		}
		Step_Previous = 0;

		// Error checking
		if ('c' == ' ') {
			msg_start += "\t*** Error:\tC cannot be ' '\n";
			return;
		}
		if (Step == 0) {

		}

		Vector4 test = new Vector4 (0.0f, 0.0f, 0.0f, 0.0f);
		if (Offset == test) {
			msg_start += "\t*** Error:\tOffset cannot be 0\n";
			return;
		}

		if (Steps.Equals (0.0f)) {
			msg_start += "\n\t*** Error:\tSteps cannot be nulll -> Script_Strobe.Start()\n";
			Debug.Log (msg_start);
			msg_start = "";
		}
		msg_start += "\tcolor_min: " + color_min + "\n";
		msg_start += "\tcolor_max: " + color_max + "\n";
		msg_start += "\tcolor_current: " + gameObject.GetComponent<MeshRenderer> ().material.color + "\n";

		Set_Step ();
		//Offset.w = Step;
		Debug.Log (msg_start);
	}
	
	// Update is called once per frame
	void Update (){
		// copy the nurrent color
		Strobe ();
	}

	void Set_Step(){
		Step = (float)1.0f / (float)Steps;
	}
		
	// Helper functions
	void Strobe(){
		string msg_strobe = "";
		msg_strobe += ("\nStrobe: " + Step_Previous + "\n");

		if (gameObject.GetComponents<MeshRenderer> ().Length == 0) {
			msg_strobe += ("\t*** Error:\t gameObject must have MeshRenderer -> gameObject.transform.name: \n" + gameObject.transform.name);
			Debug.Log (msg_strobe);
			return;
		}

		if(gameObject.GetComponent<MeshRenderer> ().material.color.a >= 1.0f){
			msg_strobe += "\ncolor.a >= 1\tResetting color to min\n";
			gameObject.GetComponent<MeshRenderer> ().material.color = color_min;
			Step_Previous++;
			msg_strobe += "\tcolor: \n " + gameObject.GetComponent<MeshRenderer> ().material.color;
			//Debug.Log (msg_strobe);
			return;
		}

		/*if (Step_Previous%Steps == 0) {
			msg_strobe += "\tmod " + Steps + " == 0\n";
			Material temp = gameObject.GetComponent<MeshRenderer> ().material;
			temp.color = color_min;
			temp = gameObject.GetComponent<MeshRenderer> ().material = temp;
			Step_Previous++;
			msg_strobe += "\tcolor: " + gameObject.GetComponent<MeshRenderer> ().material.color;
			Debug.Log (msg_strobe);
			return;
		}  else {*/
			
		float a = gameObject.GetComponent<MeshRenderer> ().material.color.a + (Offset.w*Time.deltaTime);
			msg_strobe += "\tcolor.a: " + a + "\n";
		Vector4 v = new Vector4 (color_min.r, color_min.g, color_min.b, a);
			Color n = v;
			Material t = gameObject.GetComponent<MeshRenderer> ().material;
			t.color = v;
			gameObject.GetComponent<MeshRenderer> ().material = t;
			Step_Previous++;
			msg_strobe += ("\tnew color: " + gameObject.GetComponent<MeshRenderer>().material.color + "\n");
			//Debug.Log (msg_strobe);
			return;
		//}
		//Debug.Log ("\tReturning from Strobe\n" + "\t" + msg_strobe);
		return;
	}





}

