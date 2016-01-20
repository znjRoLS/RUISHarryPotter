using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour {

	public Text text;
	public Text hp;
	public float flashSpeed = 5f;
	bool show = false;
	Color flashColour = new Color(221f, 16f, 16f, 255f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// If the player has just been damaged...
		if(show)
		{
			// ... set the colour of the damageImage to the flash colour.
			text.color = flashColour;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			text.color = Color.Lerp (text.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		
		// Reset the damaged flag.
		show = false;
	}

	public void ShowText(string t)
	{
		text.text = t;
		show = true;
	}
}
