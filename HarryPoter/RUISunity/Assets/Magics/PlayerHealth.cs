using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using HarryPotter.Magics;
using System.IO;

public class PlayerHealth : MonoBehaviour {

	public float defaultHealth;
	private float health;
	public Slider healthSlider;                                 // Reference to the UI's health bar.
	public Text hp;
	public Text pts;
	public float points;
	public long time;
	public int defaultRestartTime = 10;
	private int restartTime;
	private bool gameOver = false;
	private int hscore = 0;
	private GameObject[] enemiesDarthSidius;
	// Use this for initialization

	private void initial()
	{
		//Debug.Log ("intial player health");
		Magic.gameOver = false;
		CancelInvoke ();
	//	Debug.Log ("im here");
		gameOver = false;
		time = 0;
		health = defaultHealth;
		points = 0;
		healthSlider.value = health/defaultHealth * 100;
		InvokeRepeating ("RegenerateHealth", 3f, 3f);

		String str = health.ToString ();
		hp.text = str;
		pts.text = this.points.ToString ();

	}

	void Start () {
		initial ();

		enemiesDarthSidius = GameObject.FindGameObjectsWithTag ("Enemy");
	}
	
	// Update is called once per frame
	void Update () {
		//hp.text = health.ToString();
		//if (gameOver) {
		//	GameObject.FindGameObjectWithTag("gameOverLight").GetComponent<Light>().intensity += 0.1f;
		//}
	}

	public void TakeDmg(float dmg)
	{
		time ++;
		if (GameObject.FindGameObjectWithTag ("Shield") != null) {
		//	Debug.Log ("Absorbed dmg");
			return;
		}
		//Debug.Log (health.ToString () + "hp");
		health = health - dmg < 0 ? 0 : health - dmg;
		healthSlider.value = health/defaultHealth * 100;
		//Debug.Log (health.ToString () + "hp");
		String str = health.ToString ();
		hp.text = str;

		if (health == 0)
			GameOver();
	}

	public void AddPoints (float points)
	{
		this.points += points;
		pts.text = this.points.ToString ();
	}

	public long GetTimer()
	{
		return time;
	}
	private void GameOver()
	{
		hscore = Convert.ToInt32(File.ReadAllText("HighScore.txt"));

		int ppts = (int) points;

		if (ppts > hscore) {
			hscore = ppts;
			File.WriteAllText("HighScore.txt", Convert.ToString (ppts));
		}

		Debug.Log ("gameover");
		ThrowMagic tmagic = MagicFactory.GetMagic ("throw") as ThrowMagic;
		CancelInvoke ("RegenerateHealth");
		tmagic.ClearBuffer ();
		gameOver = true;
		Magic.gameOver = true;
		foreach (var enemyObj in GameObject.FindGameObjectsWithTag("Deementor")) {
			enemyObj.SetActive(false);
		}

		foreach (var enemyObj in GameObject.FindGameObjectsWithTag("SlowDeementor")) {
			//enemyObj.SetActive(false);
			Destroy(enemyObj);
		}

		enemiesDarthSidius = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (var enemyObj in enemiesDarthSidius) {
			enemyObj.SetActive(false);
		}

		foreach (var script in GameObject.FindGameObjectWithTag("Scripts").GetComponents<SpawnEnemies>()) {
			script.enabled = false;
		}

		//show gameover screen
		GameObject.FindGameObjectWithTag ("gameOverLight").GetComponent<Canvas> ().enabled = true;
		GameObject.FindGameObjectWithTag ("gameOverLight").GetComponent<Canvas> ().GetComponent<Text> ().text = "Score: " + points;

		restartGame ();
	}

	public void RegenerateHealth()
	{

		if (health < defaultHealth && health != 0)
			health++;
		healthSlider.value = health/defaultHealth * 100;
//		Debug.Log (health.ToString () + "hp");
		String str = health.ToString ();
		hp.text = str;
	}

	private void restartGame()
	{
		restartTime = defaultRestartTime;
		InvokeRepeating ( "countDown" ,1f, 1f);
	}

	private void countDown()
	{
		Text txt = GameObject.FindGameObjectWithTag ("gameOverLight").GetComponent<Canvas> ().GetComponent<Text> ();

		if (restartTime > 0) {
			// yep, very bad
			foreach (var enemyObj in GameObject.FindGameObjectsWithTag("SlowDeementor")) {
				Destroy(enemyObj);
			}
			foreach (var enemyObj in GameObject.FindGameObjectsWithTag("Deementor")) {
				Destroy(enemyObj);
			}

			txt.text = "Game will restart in " + restartTime-- + " seconds.\n Your score: " + points + "\n High score: " + hscore;
			return ;
		}

	

		GameObject.FindGameObjectWithTag ("gameOverLight").GetComponent<Canvas> ().enabled = false;
		txt.text = "";
		
		foreach (var enemyObj in enemiesDarthSidius) {
			enemyObj.SetActive(true);
			enemyObj.GetComponent<EnemyHealth>().init();
		}
		
		foreach (var script in GameObject.FindGameObjectWithTag("Scripts").GetComponents<SpawnEnemies>()) {
			script.enabled = true;
		}

		initial ();
		CancelInvoke ("countDown");
	}
}
