using UnityEngine;
using System.Collections;

public class BlastEffect : MonoBehaviour {

	public float startSizeX;
	public float startSizeY;
	public float startSizeZ;
	public float minSizeX;
	public float speedSizeX;

	public float blastSpeed ;

	public float startSizeAnim ;
	public float startLifetimeAnim;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<BoxCollider> ().size = new Vector3(startSizeX,startSizeY,startSizeZ);
		this.gameObject.GetComponent<ParticleSystem> ().startSize = startSizeAnim;
		//this.gameObject.GetComponent<ParticleSystem> ().startLifetime = startLifetimeAnim;

		GameObject hand = GameObject.FindGameObjectWithTag ("lhand");
		GameObject shoulder = GameObject.FindGameObjectWithTag ("lShoulder");
		
		Vector3 direction = hand.transform.position - shoulder.transform.position;

		//this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.left * blastSpeed;

		this.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * blastSpeed;

		//Invoke ("destr", startLifetimeAnim);
		Destroy (this.gameObject, startLifetimeAnim);
	}
	
	// Update is called once per frame
	void Update () {

		if (this.gameObject.GetComponent<BoxCollider> ().size.x > minSizeX)
			this.gameObject.GetComponent<BoxCollider> ().size -= new Vector3(speedSizeX,0,0);
	}

	//void destr()
	//{
	//	Destroy (this.gameObject);
	//}
}
