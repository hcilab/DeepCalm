using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Click_Target_Data_Producer : MonoBehaviour
{
	[Range(0, 10)]
	public int script_delay;
	// Use this for initialization

	public int buffer_size;
	List<Vector3[]>  all_clicked_targets;

	bool recording = false;

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTargetClicked(List<Vector3[]> vars){
		
	}
}

