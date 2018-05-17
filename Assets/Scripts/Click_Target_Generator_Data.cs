using UnityEngine;
using System.Collections;

[System.Serializable]
public class Click_Target_Generator_Data
{
	public Click_Target_Data click_target_data { get; private set; }
	public int num_targets_to_generate { get; private set; }
	public Script_Click_Target script_click_target { get; private set; }
	public Click_Target_Generator_Data(Click_Target_Data click_target_data, int num_targets_to_generate){
		this.click_target_data = click_target_data;
		this.num_targets_to_generate = num_targets_to_generate;
	}
}

