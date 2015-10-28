using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

//
//	public GameObject prototype;
//	GameObject first, second;
//
//	static void Create ( GameObject f, GameObject s )
//	{
//		GameObject newObj = Instantiate ( this );
//
//		Destroy (newObj, 5);
//	}
//
//
//	Lightning ( GameObject f, GameObject s )
//	{
//		first = f;
//		second = s;
//	}
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//		transform.position = (first.transform.position + second.transform.position)/2;
//		
//		Vector3 pathBetween = second.transform.position - first.transform.position;
//		Debug.Log (pathBetween);
//		
//		transform.eulerAngles = new Vector3(90,180  + Mathf.Rad2Deg * Mathf.Atan( (float)pathBetween.x / pathBetween.z ),180);
//		
//		transform.localScale = pathBetween.magnitude/80 * Vector3.one;
//		
//		Debug.Log (pathBetween.x + " " + pathBetween.z + " " + Mathf.Atan ((float)pathBetween.x / pathBetween.z));
//	}
}
