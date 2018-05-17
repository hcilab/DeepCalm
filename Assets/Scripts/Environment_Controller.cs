using UnityEngine;
using System.Collections;

public class Environment_Controller : MonoBehaviour
{
	public GameObject prefab_cloud;
	public int num_clouds;
	public Vector2 x_range;
	public Vector2 y_range;
	public Vector2 z_range;
	float rotation = 0;
	float x = 0.0f;
	float y = 0.0f;
	float z = 0.0f;
	int cloud_num;

	void Start ()
	{
		for (int i = 0; i < num_clouds; i++) {
			Spawn_New_Cloud ();
		}
	}

	void Spawn_New_Cloud(){
		GameObject new_cloud = Instantiate (prefab_cloud);
		new_cloud.transform.SetParent (transform);
		new_cloud.transform.name = "cloud_" + cloud_num;
		if (cloud_num % 2 == 0) {
			new_cloud.transform.Rotate(Vector3.right * 180);
		}
		if (cloud_num % 1 == 0) {
			new_cloud.transform.Rotate(Vector3.up * 180);
		}
		if (cloud_num % 3 == 0) {
			new_cloud.transform.Rotate(Vector3.left * 180);
		}
		if (cloud_num % 3 == 0) {
			new_cloud.transform.Rotate(Vector3.down * 180);
		}
		x = Random.Range (x_range.x, x_range.y);
		y = Random.Range (y_range.x, y_range.y);
		z = Random.Range (z_range.x, z_range.y);
		new_cloud.transform.position = new Vector3 (x, y, z);
		cloud_num ++;
	}
		
}

