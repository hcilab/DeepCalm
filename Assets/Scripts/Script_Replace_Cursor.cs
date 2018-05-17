using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Script_Replace_Cursor : MonoBehaviour
{
	RectTransform hand_position;
	// Use this for initialization
	void Start ()
	{
		hand_position = GetComponent<RectTransform> ();
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		hand_position.localPosition = Input.mousePosition;
	}
}

