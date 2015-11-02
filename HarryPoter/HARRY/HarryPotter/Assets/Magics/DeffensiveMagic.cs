using System;
using HarryPotter.Magics;
namespace HarryPotter.Magics
{
	public abstract class DeffensiveMagic : Magic
	{
		public static ThrowMagic throwMagic;
		
		public DeffensiveMagic (string magicName, int cooldown) : base(magicName)
		{
			this.cooldown = cooldown;
		}
		
		public DeffensiveMagic (string magicName) : base(magicName)
		{
			cooldown = 0;
		}
		
		protected override void Activate()
		{
			Effect ();
		}
	}
}
