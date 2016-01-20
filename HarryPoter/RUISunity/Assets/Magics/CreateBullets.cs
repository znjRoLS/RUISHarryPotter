using UnityEngine;
using System.Collections;

public class CreateBullets : MonoBehaviour {
	
	public GameObject blastEffect;
	public float blastStartOffset;
	
	private GameObject hand;
	private GameObject shoulder;

	// Use this for initialization
	void Start () {
		hand = GameObject.FindGameObjectWithTag ("lhand");
		shoulder = GameObject.FindGameObjectWithTag ("lShoulder");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ThrowZMagic()
	{
		Vector3 direction = hand.transform.position - shoulder.transform.position;

		GameObject playa = GameObject.FindGameObjectWithTag ("Player");

		//GameObject nesto = (GameObject)MonoBehaviour.Instantiate (blastEffect, playa.transform.position + Vector3.left*blastStartOffset + Vector3.up * 100f, playa.transform.rotation);
		GameObject nesto = (GameObject)MonoBehaviour.Instantiate (blastEffect, playa.transform.position + direction*blastStartOffset + Vector3.up * 100f, playa.transform.rotation);

		nesto.transform.LookAt (direction);

		Debug.Log ("direction magnitude" + direction.magnitude);

		nesto.SetActive (true);
	}
}
