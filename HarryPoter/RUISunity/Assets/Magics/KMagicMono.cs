using UnityEngine;
using System.Collections;
using HarryPotter.Magics;

public class KMagicMono : MonoBehaviour {
	public GameObject hand;
	public GameObject shoulder;
	GameObject purple;
	// Use this for initialization
	void Start () {
		hand = GameObject.FindGameObjectWithTag ("hand");
		shoulder = GameObject.FindGameObjectWithTag ("Shoulder");
		if (hand == null)
			Debug.Log ("null hand");
	}
	
	// Update is called once per frame
	void Update () {
		if ((purple = GameObject.FindGameObjectWithTag ("purpleLight")) != null) {
			Magic.casting = true;
			Debug.Log ("KKKKKK HNERE OR NO!?");
			Ray ray = new Ray ();
		
			RaycastHit rHit;
		
			ray.origin = hand.transform.position;
			ray.direction = hand.transform.position - shoulder.transform.position;
		
			bool hit = Physics.Raycast (ray, out rHit);
		
			//Debug.Log ("rhit logs " + rHit);

			if (hit) {
				GameObject hitObj = rHit.transform.gameObject;
				if (hitObj.tag == "Deementor" || hitObj.tag == "Enemy" || hitObj.tag == "SlowDeementor") {
					hitObj.GetComponent<EnemyHealth> ().Harm (2.5f);
				}

				Vector3 diff = hand.transform.position - shoulder.transform.position;
				purple.transform.position = (hand.transform.position + rHit.point) / 2;
				purple.transform.rotation = Quaternion.LookRotation (diff.normalized);

			} else {

				Debug.Log ("WHY AM I MISSING!?");
				purple.transform.position = (hand.transform.position + Vector3.forward * 10) / 2;
			}
		
		
			//lightning.transform.position += Vector3.down * 5;
		
			Vector3 pathBetween = rHit.point - hand.transform.position;
			//Vector3 pathBetween = Vector3.forward * 100;
			//lightning.transform.eulerAngles = new Vector3(90,180  + Mathf.Rad2Deg * Mathf.Atan( (float)pathBetween.x / pathBetween.z ),180 );
		
			purple.transform.localScale = pathBetween.magnitude / 80 * Vector3.one;
		} else {
			Magic.casting = false;
		}
	}
}
