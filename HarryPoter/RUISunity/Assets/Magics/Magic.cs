using UnityEngine;
using System.Collections;
using System;

namespace HarryPotter.Magics
{
	public abstract class Magic 
	{
		/// <summary>
		/// The cooldown.
		/// </summary>
		protected int cooldown;

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
			TimeSpan difference = DateTime.UtcNow - timeStamp;
			//Debug.Log (difference.TotalSeconds + " HEHEHEHE");
			//Debug.Log ("cooldown : " + cooldown);
			if (difference.TotalSeconds < cooldown) 
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Tries activate magic if conditions are met (cooldown for now, maybe mana later).
		/// </summary>
		public void TryActivate()
		{
			Debug.Log ("called activate");
			if (IsReady ()) 
			{
				timeStamp = DateTime.UtcNow;
				Activate ();
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
