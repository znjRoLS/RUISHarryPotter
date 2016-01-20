using UnityEngine;
using System.Collections;

public class LookCameraPosition : MonoBehaviour {

	public GameObject mainCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = mainCamera.transform.position;
		transform.rotation = mainCamera.transform.rotation;
	}
}
