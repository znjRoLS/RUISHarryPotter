using UnityEngine;
using System.Collections;

public class UILockToOculusCamera : MonoBehaviour {

	private GameObject ui;
	public float distance;
	private static float defaultDistance = 400f;

	// Use this for initialization
	void Start () {
		ui = GameObject.FindGameObjectWithTag ("UI");

	}
	
	// Update is called once per frame
	void Update () {

		Transform cameraTrans = this.gameObject.transform;

		ui.transform.rotation = cameraTrans.rotation;
		ui.transform.position = cameraTrans.position + cameraTrans.forward * distance;

		//dont know why

		ui.transform.position += new Vector3 (0, 150f, 0);

		ui.transform.localScale = Vector3.one * distance / defaultDistance;

		//Debug.Log ("camera pos" + cameraTrans.position);
		//Debug.Log ("ui position" + ui.transform.position);

	}
}
