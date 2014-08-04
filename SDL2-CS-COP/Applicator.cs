using System;

namespace SDL2_CS_COP
{
	/// <summary>
	/// Applicator: A processing system for combined data sets.
	/// </summary>
	public abstract class Applicator: ICOP_System
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SDL2_CS_COP.Applicator"/> class.
		/// </summary>
		public Applicator ()
		{
			/*
             *  In SDL2-CS-COP, there is no difference between an applicator and a system
			 */
		}
		public abstract void Process (SDL2_CS_COP.World world, System.Collections.Generic.List<Entity> entities);
		public abstract bool CanProcess (Entity theEntity);
	}
}

