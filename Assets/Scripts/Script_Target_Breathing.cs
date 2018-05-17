using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Script_Target_Breathing : MonoBehaviour
{
	private Transform next;
	public float points;
	public int id;

	string msg;
	// Use this for initialization
	void Start ()
	{
		msg = "\nScript_Target_Breathing\n";
		msg += "\nStart()\n";
		id = -1;
	}

	void OnDestroy(){
		List<Vector3> vars = new List<Vector3> ();
		vars.Add (GameObject.FindGameObjectWithTag ("player").transform.position - gameObject.transform.position);
		float n = -1f;
		float.TryParse (transform.name, out n);
		Vector3 v = new Vector3 (n, 1, -1);
		vars.Add (v);
		if (GameObject.Find ("Statistics_Controller").GetComponent<Script_Breathing_Target_Data_Producer> ().is_recording) {
			GameObject.Find ("Statistics_Controller").SendMessage ("Record_Hit_Target", vars);
		}
		GameObject.FindGameObjectWithTag ("player").SendMessage ("Add_Points_To_Score", points);
	}

	public void Set_ID(int id){
		this.id = id;
	}

	public int Get_ID(){
		return this.id;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Set_Next(Transform next){
		this.next = next;
	}

	public Transform Get_Next(){
		return next;
	}

	public void Set_Points(float points){
		this.points = points;
	}

	public float Get_Points(){
		return points;
	}


}

