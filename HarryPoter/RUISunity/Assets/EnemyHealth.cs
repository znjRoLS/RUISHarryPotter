using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class EnemyHealth : MonoBehaviour {
	GameObject player;
	bool dead;
	Animator anim;
	public float defaultHealth;
	private float health;
	Component scripta;
	Vector3 pos;
	public Image image;
	// Use this for initialization
	void Start () {
		health = defaultHealth;
		player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<PlayerHealth> ();
		anim = GetComponent <Animator>();
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {
		//image.color = Color.Lerp (new Color (0, 255 * health / 100, 0), new Color (255 * (100-health) / 100, 0, 0), 3f);
		if(tag=="Enemy")
			image.color = new Color(255 * (defaultHealth - health)/defaultHealth , 255 * health/defaultHealth, 0, 151);
	}

	public void Harm(float dmg)
	{
		if (!dead) {
			//image.color = Color.Lerp(new Color(255,0,0), new Color(0,255,0), health/100);
			health -= dmg;
			if (health <= 0) {
				if (tag == "Enemy") {
					dead = true;
					anim.SetTrigger ("Dead");
					pos = transform.position;
					//Destroy (this.gameObject, 10f);

					Invoke ("KillDelayed", 3f);
					GameObject.FindGameObjectWithTag("Scripts").GetComponent<PlayerHealth> ().AddPoints (1000);
				} else {
					Destroy (this.gameObject);
					GameObject.FindGameObjectWithTag("Scripts").GetComponent<PlayerHealth> ().AddPoints (100);
				}
			}
		}
	}

	void KillDelayed()
	{
		this.gameObject.SetActive(false);
		this.GetComponent<EnemyBehave> ().kill ();
		Invoke ("SpawnDelayed", 5f);
	}

	void SpawnDelayed()
	{
		//this.transform.position = new Vector3 (1000f, -1001f, 1231f);
		//GameObject nesto = (GameObject)Instantiate (this, pos, this.transform.rotation);
		//GameObject nesto = (GameObject)Instantiate (this, pos, this.transform.rotation);
		this.gameObject.SetActive (true);
		init ();
	}

	public void init()
	{
		dead = false;
		health = defaultHealth;
		this.GetComponent<EnemyBehave> ().init ();
	}

}
