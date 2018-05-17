using System.Collections;
using UnityEngine;

public class Script_Camera_Controller : MonoBehaviour {
	private Transform player;
	private Vector3 Test;

	void Start () {
		player = GameObject.FindGameObjectWithTag("player").transform;
	}

	void Update () {
		gameObject.transform.position = new Vector3 (player.position.x, 0, -40);
	}

}
