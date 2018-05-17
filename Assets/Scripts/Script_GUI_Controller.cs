using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Script_GUI_Controller : MonoBehaviour
{

	GameObject GUI_dialogue;
	public Vector3 GUI_dialogue_player_offset;

	GameObject GUI_score;
	public Vector3 GUI_score_player_offset;

	GameObject GUI_health_bar;
	public Vector3 GUI_health_bar_player_offset;

	Button button_option_0;
	SceneManager sm = new SceneManager();
	UnityEngine.Events.UnityAction action;
	// Use this for initialization
	private Text score_text;

	void Start(){
		GUI_dialogue = GameObject.Find ("GUI_Controller/GUI_Dialogue");

		GUI_score = GameObject.Find ("GUI_Controller/GUI_Score");
		score_text = GameObject.Find ("GUI_Score/Canvas/Panel/Text_Value").GetComponent<Text> ();
		Debug.Log ("Current score_text: " + score_text.text);
		GUI_score.transform.SetParent(GameObject.Find("Main_Camera").transform);

		GUI_health_bar = GameObject.Find ("GUI_Controller/GUI_Health_Bar");
		GUI_health_bar.transform.SetParent(GameObject.Find("Main_Camera").transform);

		Update_Health_Bar (GameObject.FindGameObjectWithTag ("player").GetComponent<Script_Player_Flex_Sensor> ().max_health);
	}


	/* GUI stuff for Calibration
	void OnLevelWasLoaded(){
		Debug.Log ("Level was loaded");
		if(!SceneManager.GetActiveScene().name.Equals("Scene_Calibration")){
			
			GUI_dialogue.transform.name = "GUI_Dialogue";

			GUI_score.transform.name = "GUI_Score";
			score_text = GameObject.Find ("GUI_Score/Canvas/Panel/Text_Value").GetComponent<Text> ();
			GameObject.Find("GUI_Score").transform.SetParent(GameObject.Find("Camera").transform);
			Debug.Log ("Current score_text: " + score_text.text);

			GUI_Health_Bar = Instantiate (Prefab_GUI_Health_Bar);
			GUI_Health_Bar.transform.name = "GUI_Health_Bar";
			Update_Health_Bar (GameObject.FindGameObjectWithTag ("player").GetComponent<Script_Player_Flex_Sensor> ().max_health);
		}
	}
	*/

	void Update ()
	{
		
	}
		
	public void Update_Dialogue_Text(string s){
		GameObject.Find ("Canvas/Panel_Output/Text_Output_Body").GetComponent<Text> ().text = s;
	}

	public void Update_Dialogue_Header(string s){
		GameObject.Find ("Canvas/Panel_Output/Text_Output_Header").GetComponent<Text> ().text = s;
	}

	public void Update_Dialogue_Position(Vector3 player_position){
		GUI_dialogue.transform.position = player_position + GUI_dialogue_player_offset;
	}

	void Hide(GameObject parent_go){
		Debug.Log ("Hiding: " + parent_go.transform.name);
		parent_go.GetComponent<CanvasGroup> ().alpha = 0;
		parent_go.GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	public void Show(GameObject parent_go){
		Debug.Log ("Showing: " + parent_go.transform.name);
		parent_go.GetComponent<CanvasGroup> ().alpha = 1;
	}

	public void Update_Button_Option_0_Text(string s){
		GameObject.Find ("GUI_Dialogue/Canvas/Panel_Options/Button_Option_0/Text").GetComponent<Text> ().text = s;
	}

	public void Update_Button_Option_1_Text(string s){
		GameObject.Find ("GUI_Dialogue/Canvas/Panel_Options/Button_Option_1/Text").GetComponent<Text> ().text = s;
	}

	public void On_Button_Option_0_Clicked(){
		Debug.Log ("GUI_Controller: On_Button_Option_0_Clicked()");
		string s = GameObject.Find ("GUI_Dialogue/Canvas/Panel_Options/Button_Option_0/Text").GetComponent<Text> ().text;
		switch (s){
		case "Start":
			GameObject.Find("Game_Controller").SendMessage ("Start_Current_Level");
			break;
		default:
			Debug.Log ("On_Button_Option_0_Clicked()\n\tFell through to default");
			break;
		}
		Hide (GameObject.Find ("GUI_Dialogue"));
		EventSystem.current.SetSelectedGameObject(null);
	}

	// TODO: Change this to option 2 stuff (currently copy option 1)
	public void On_Button_Option_1_Clicked(){
		string s = GameObject.Find ("GUI_Dialogue/Canvas/Panel_Options/Button_Option_1/Text").GetComponent<Text> ().text;
		switch (s){
		case "Start":
			break;
		default:
			Debug.Log ("On_Button_Option_0_Clicked()\n\tFell through to default");
			break;
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void Update_Score(float score){
		score_text = GameObject.Find ("GUI_Score/Canvas/Panel/Text_Value").GetComponent<Text> ();
		Debug.Log ("score_text: " + score_text);
		score_text.text = score.ToString ();
	}

	public void Update_Health_Bar(float health){
		// should make sure that this is receiving a float b/w 0 and 1
		GameObject.Find ("GUI_Health_Bar/Canvas/Slider").
			GetComponent<Slider> ().value = health;
		//Debug.Log ("Updated GUI_Health_Bar to: " + health);
	}
}

