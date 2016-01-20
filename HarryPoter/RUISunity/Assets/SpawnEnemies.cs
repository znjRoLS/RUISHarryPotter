using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemies : MonoBehaviour
{
	//public PlayerHealth playerHealth;       // Reference to the player's heatlh.
	public GameObject enemy;                // The enemy prefab to be spawned.
	public float defaultSpawnTime;            // How long between each spawn.
	private float spawnTime;
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	
	void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
		spawnTime = defaultSpawnTime;
	}
	
	void Update()
	{

		spawnTime -= Time.deltaTime;

		if (spawnTime < 0f) {
		
			spawnTime = defaultSpawnTime;
			Spawn ();
		}
	}

	void Spawn ()
	{
		// If the player has no health left...
		//	if(playerHealth.currentHealth <= 0f)
		//	{
		//		// ... exit the function.
		//		return;
		//	}
		
		// Find a random index between zero and one less than the number of spawn points.
		if (enemy.tag == "Deementor") {
			GetComponent<DisplayText> ().ShowText ("Get ready fast Deementor coming in 3 seconds!");
			Invoke ("SpawnDelayed", 3f);
		} else {
			SpawnDelayed ();
		}
	}

	void SpawnDelayed()
	{
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		GameObject nesto = (GameObject)Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		nesto.SetActive (true);
	}
}


	