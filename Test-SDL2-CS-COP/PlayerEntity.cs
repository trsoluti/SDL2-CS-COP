using System;

namespace TestSDL2CSCOP
{
	/// <summary>
	/// Player entity.
	/// 
    /// This represents a player in our paddle game.
    /// </summary>
	/// <remarks>
	/// Players have:
	///  - a display sprite, with position and area 
	///  - a velocity
    /// </remarks>
	public class PlayerEntity: SDL2_CS_COP.Entity
	{
        /// <summary>
        /// Speed (in pixels/frame) at which the paddle moves.
        /// </summary>
		public const int PADDLE_SPEED = 3;

        /// <summary>
        /// Gets or sets the paddle sprite.
        /// </summary>
        /// <value>The sprite component.</value>
		public SDL2_CS_COP.StandardItems.Components.Sprite Sprite { get;  set;  }
        /// <summary>
        /// Gets or sets the velocity component.
        /// </summary>
        /// <value>The velocity component.</value>
		public SDL2_CS_COP.StandardItems.Components.Velocity VelocityComponent { get ; set; }
        /// <summary>
        /// Gets or sets the player data component.
        /// </summary>
        /// <value>The player data component.</value>
		public PlayerDataComponent PlayerDataComponent { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestSDL2CSCOP.PlayerEntity"/> class.
		/// </summary>
		/// <param name="theWorld">The world.</param>
		/// <param name="theSprite">The sprite.</param>
		/// <param name="posx">X position.</param>
		/// <param name="posy">Y Position.</param>
		/// <param name="isAi">If set to <c>true</c> is ai.</param>
		public PlayerEntity (SDL2_CS_COP.World theWorld, SDL2_CS_COP.StandardItems.Components.Sprite theSprite, int posx = 0, int posy = 0, bool isAi=false): base (theWorld)
		{
			// Set the sprite and position it
			this.Sprite = theSprite;
			this.Sprite.MoveTo (posx, posy);

			// Create the other components and initialize them
			this.VelocityComponent = new SDL2_CS_COP.StandardItems.Components.Velocity (0,0);
			this.PlayerDataComponent = new PlayerDataComponent ();
			this.PlayerDataComponent.IsAi = isAi;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="TestSDL2CSCOP.PlayerEntity"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="TestSDL2CSCOP.PlayerEntity"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[PlayerEntity: ({0},{1})]", this.Sprite.Left, this.Sprite.Top);
		}
		
	}
}

