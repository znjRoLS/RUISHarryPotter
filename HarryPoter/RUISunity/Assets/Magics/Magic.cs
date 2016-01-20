using UnityEngine;
using System.Collections;
using System;

namespace HarryPotter.Magics
{
	public abstract class Magic 
	{
		public static bool gameOver = false;
		public static bool casting = false;
		bool first = true;
		/// <summary>
		/// The cooldown.
		/// </summary>
		protected int cooldown;

		/// <summary>
		/// The sprite.
		/// </summary>
		public Sprite sprite = null;

		/// <summary>
		/// The name of the magic.
		/// </summary>
		public string magicName;

		/// <summary>
		/// Last time magic was thrown 
		/// </summary>
		private DateTime timeStamp;

		public Magic(string magicName)
		{
			first = true;
			this.magicName = magicName;
			timeStamp = DateTime.UtcNow;
			cooldown = 0;
		}

		/// <summary>
		/// Determines whether this magic is ready to activate.
		/// </summary>
		/// <returns><c>true</c> if this instance is ready; otherwise, <c>false</c>.</returns>
		protected bool IsReady()
		{
			if (gameOver || casting)
				return false;
			if (first) {
				first = false;
				return true;
			}
			TimeSpan difference;
			//if (timeStamp != null)
			difference = DateTime.UtcNow - timeStamp;
			
			//Debug.Log (difference.TotalSeconds + " HEHEHEHE");
			//Debug.Log ("cooldown : " + cooldown);
			if (difference.TotalSeconds < cooldown) 
			{	
				int totalSeconds = (int)(cooldown-difference.TotalSeconds);
				if(difference.TotalSeconds > 1)
					GameObject.FindGameObjectWithTag("Scripts").GetComponent<DisplayText>().ShowText("That magic will be ready in " + totalSeconds + " seconds!");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Tries activate magic if conditions are met (cooldown for now, maybe mana later).
		/// </summary>
		public void TryActivate()
		{
			if (IsReady ()) 
			{
				timeStamp = DateTime.UtcNow;
				Activate ();
			}
		}

		public void TryEffect()
		{
			if (IsReady ()) 
			{
				timeStamp = DateTime.UtcNow;
				Effect ();
			}

		}
	
		/// <summary>
		/// What to do when instance is recognized
		/// </summary>
		protected abstract void Activate();
	
		/// <summary>
		/// Magic effect
		/// </summary>
		public abstract void Effect();

	}
}
