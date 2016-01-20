using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	Transform player;               // Reference to the player's position.
	//PlayerHealth playerHealth;      // Reference to the player's health.
	//EnemyHealth enemyHealth;        // Reference to this enemy's health.
	NavMeshAgent nav;               // Reference to the nav mesh agent.
	public float maxSpeed;
	public float minSpeed;
	public float maxDist;
	bool playerReached;

	void Awake ()
	{/*
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		//playerHealth = player.GetComponent <PlayerHealth> ();
		//enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent <NavMeshAgent> ();
		nav.enabled = true;
		*/
	}

	void Start()
	{
		transform.LookAt (GameObject.FindGameObjectWithTag ("Player").transform);
		this.gameObject.GetComponent<Rigidbody> ().freezeRotation = true;
	}
	
	void Update ()
	{/*
		// If the enemy and the player have health left...
		//if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
		//{
			// ... set the destination of the nav mesh agent to the player.
		nav.enabled = true;	
		nav.SetDestination (player.position);
		//}
		// Otherwise...
		//else
		//{
			// ... disable the nav mesh agent.
			//nav.enabled = false;
		//}*/

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		
		Vector3 direction = player.position - this.gameObject.transform.position;


		float dist = direction.magnitude;


		this.gameObject.GetComponent<Rigidbody> ().velocity = direction.normalized * (dist/maxDist * (maxSpeed-minSpeed) + minSpeed);


	} 

	void OnCollisionEnter(Collision c)
	{
		Debug.Log ("Entered here");

		if(c.gameObject.tag == "Shield")
		{
			Debug.Log ("heree shield");
			GetComponent<EnemyHealth>().Harm(1000);
		}

		if (c.gameObject.tag == "ignore")
			return;
		//Debug.Log ("Collinding with " + c.gameObject.tag);
		if (c.gameObject.tag == "Respawn") {
			//c.gameObject.GetComponentInChildren<PlayerHealth>().TakeDmg (1000);
			GameObject.FindGameObjectWithTag("Scripts").GetComponent<PlayerHealth>().TakeDmg(1000);
			//tag = "AboutToKillMe";
		}
	}
}