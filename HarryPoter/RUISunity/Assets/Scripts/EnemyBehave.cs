using UnityEngine;
using System.Collections;

public class EnemyBehave : MonoBehaviour {
	
	Animator anim;
	public GameObject hand;
	GameObject lightningObject;
	GameObject target;
	bool running;
	float changeDir;
	int waitTime;
	// Use this for initialization
	void Start () 
	{
		init ();
	}

	public void kill()
	{
		CancelInvoke ("ChangeDirection");
	}

	public void init()
	{
		waitTime = (int)(Random.value * 300 + 600);
		float changeDir = Random.value * 3 + 1;
		anim = GetComponent <Animator>();
		lightningObject = GameObject.FindGameObjectWithTag ("lightningObject");
		target = GameObject.FindGameObjectWithTag ("Player");
		
		InvokeRepeating ("ChangeDirection", changeDir, changeDir);
	}

	void ChangeDirection()
	{
		if (anim.GetBool ("RunRight")) {
			anim.SetBool ("RunRight", false);
			anim.SetBool ("RunLeft", true);
		} else if (anim.GetBool ("RunLeft")) {
			anim.SetBool ("RunLeft", false);
			anim.SetBool ("RunRight", true);
		}
	}
	int cnt = 0;
	// Update is called once per frame
	void Update () 
	{
		//hardcoded position, fix their y position

		//this.gameObject.transform.localPosition -= new Vector3 (0, this.gameObject.transform.localPosition.y, 0);

		//also, hardcore their speed by x to zero.

		Rigidbody rig = this.gameObject.GetComponent<Rigidbody>();
		rig.velocity = rig.velocity - new Vector3 (rig.velocity.x, 0, 0);

	
			transform.eulerAngles = new Vector3 (0, 90, 0);
		if (anim.GetBool ("IsStanding")) {
			anim.SetBool ("IsStanding", false);
			
			if(Random.value < 0.5)
			{
				anim.SetBool("RunRight", true);
			}
			else
				anim.SetBool("RunLeft", true);
		}
		cnt++;
		if (cnt >= waitTime) {
	
			Attack ();
		}
		
	}
	
	void OnCollisionEnter(Collision c)
	{
		if (anim.GetBool ("RunRight")) {
			anim.SetBool ("RunRight", false);
			anim.SetBool ("RunLeft", true);
		} else if (anim.GetBool ("RunLeft")) {
			anim.SetBool ("RunLeft", false);
			anim.SetBool ("RunRight", true);
		}
		
	}
	Quaternion oldRot;
	void Attack()
	{
		Debug.Log ("Attacking");
		running = false;
		//GetComponent<Rigidbody> ().useGravity = false;
		oldRot = transform.rotation;
		transform.LookAt (GameObject.FindGameObjectWithTag ("Player").transform.position);
		waitTime = (int)(Random.value * 500 + 300);
		cnt = 0;
		anim.SetBool ("RunRight", false);
		anim.SetBool ("RunLeft", false);
		anim.SetTrigger ("Throw");
		if(Random.value < 0.5)
		{
			anim.SetBool("RunRight", true);
		}
		else
			anim.SetBool("RunLeft", true);
	}
	
	void att()
	{
		//Debug.Log (target.transform.position);
		//Debug.Log (hand.transform.position);

		GameObject lightning = (GameObject)MonoBehaviour.Instantiate(lightningObject);

		lightning.transform.position = (hand.transform.position + target.transform.position)/2;
		
		lightning.transform.LookAt (target.transform.position);

		//Debug.Log (lightning.transform.position);

		GameObject.FindGameObjectWithTag("Scripts").GetComponent<PlayerHealth> ().TakeDmg (5);
		//Vector3 pathBetween = Vector3.forward * 100;

		Vector3 pathBetween = target.transform.position - hand.transform.position;

		lightning.transform.localScale = pathBetween.magnitude/80 * Vector3.one;


		//float dist = target.transform.position - hand.transform.position;

		//lightning.

		GameObject.Destroy (lightning, 0.5f);
		Invoke ("UpdateRotation", 0.49f);

		//GetComponent<Rigidbody> ().useGravity = true;
		running = true;
	}

	void UpdateRotation()
	{
		transform.rotation = oldRot;
	}
	public void updatePosition()
	{
		
	}
}
