//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using HarryPotter.Magics;
using UnityEngine;

namespace HarryPotter.Magics
{
	public class ThrowMagic : Magic
	{
		internal Queue<Magic> magics = new Queue<Magic>();

		public ThrowMagic(string magicName) : base(magicName)
		{
		}

		public override void Effect()
		{
			try
			{
				Magic magicToThrow = magics.Dequeue();
				magicToThrow.Effect ();
			}

			catch(Exception e)
			{
				Debug.Log ("No magic to throw!");
			}
		}

		protected override void Activate()
		{
			Effect ();
		}
	}
}


