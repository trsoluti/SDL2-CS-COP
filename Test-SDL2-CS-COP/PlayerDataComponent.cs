using System;

namespace TestSDL2CSCOP
{
	/// <summary>
    /// Player data component for our paddle game.
	/// 
	/// This component represents the information we need to know
	/// about a player.
    /// </summary>
	/// <remarks>
	/// It is part of the <see cref="TestSDL2CSCOP.PlayerEntity"/> , and is acted on by
	/// the <see cref="TestSDL2CSCOP.MovementSystem"/>.
    /// </remarks>
	public class PlayerDataComponent: SDL2_CS_COP.Component
	{
        /// <summary>
        /// Gets or sets a value indicating whether this instance is an AI.
        /// </summary>
        /// <value><c>true</c> if this instance is an AI; otherwise, <c>false</c>.</value>
		public bool IsAi  { get; set;  }
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
		public int Points { get ; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestSDL2CSCOP.PlayerDataComponent"/> class.
		/// </summary>
		public PlayerDataComponent ()
		{
			/*
			 *     self.ai = False
			 *     self.points = 0
			 */
			this.IsAi = false;
			this.Points = 0;
		}
	}
}

