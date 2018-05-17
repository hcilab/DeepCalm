using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GUI_Panel : MonoBehaviour {
	public float padding_right;
	public float padding_left;
	public float padding_top;
	public float padding_bottom;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float[] Get_Paddings(){
		float[] paddings = new float[] {padding_left, padding_right, padding_top, padding_bottom};
		return paddings;
	}
}
