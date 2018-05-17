using UnityEngine;
using System.Collections;

public class Script_Health_Controller : MonoBehaviour
{
	private bool started;
	public int step_size;
	public float damage_per_step;
	int start_x_position;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (started) {
			if (((int)transform.position.x - start_x_position) % step_size == 0) {
				gameObject.SendMessage ("On_Player_Damaged", damage_per_step);
			}
		}
	}

	public void On_Level_Started(int level_number){
		if (level_number != 0) {
			started = true;
			start_x_position = (int)transform.position.x;
		}
	}

	public void On_Level_Ended(){
		started = false;
	}
}

