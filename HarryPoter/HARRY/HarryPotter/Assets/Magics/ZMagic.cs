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
using UnityEngine;

namespace HarryPotter.Magics
{
	public class ZMagic : OffensiveMagic
	{
		public ZMagic (string magicName, int cooldown) : base(magicName)
		{
			this.cooldown = cooldown;
		}

		public override void Effect()
		{
			Debug.Log ("ZZZZZZZZZZ");
		}
	}
}
