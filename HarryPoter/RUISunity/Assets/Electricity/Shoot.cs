using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public int framesBeforeSpan;
	public GameObject lightningObject;
	public GameObject enemy;
	public float duration;
	GameObject lightning;

	// Use this for initialization
	void Start () {
		
		lightning = (GameObject)Instantiate(lightningObject);
		
		lightning.transform.position = (this.transform.position + enemy.transform.position)/2;
		
		lightning.transform.eulerAngles = new Vector3(90,180,180);
		
		Destroy ( lightning , duration);
	}
	
	// Update is called once per frame
	void Update () {
		
		lightning.transform.position = (this.transform.position + enemy.transform.position)/2;
		
		Vector3 pathBetween = enemy.transform.position - this.transform.position;
		Debug.Log (pathBetween);
		
		lightning.transform.eulerAngles = new Vector3(90,180  + Mathf.Rad2Deg * Mathf.Atan( (float)pathBetween.x / pathBetween.z ),180 );
		
		lightning.transform.localScale = pathBetween.magnitude/80 * Vector3.one;
		
		Debug.Log (pathBetween.x + " " + pathBetween.z + " " + Mathf.Atan ((float)pathBetween.x / pathBetween.z));
	}
}

