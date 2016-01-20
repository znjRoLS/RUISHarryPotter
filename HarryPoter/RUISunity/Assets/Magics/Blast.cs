using UnityEngine;
using System.Collections;

public class Blast : MonoBehaviour {

	public GameObject blastEffect;

	public float blastStartOffset ;
	public float blastRepeat;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("SpawnNewBlast", 1f, blastRepeat);
	}
	
	// Update is called once per frame
	void Update () {

	}


	void SpawnNewBlast()
	{
		Debug.Log ("nesto");
		GameObject.FindGameObjectWithTag ("Scripts").GetComponent<CreateBullets> ().ThrowZMagic ();
	}
}
