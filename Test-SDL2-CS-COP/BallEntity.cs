using System;

namespace TestSDL2CSCOP
{
	/// <summary>
	/// Ball entity.
	/// 
	/// This represents the ball in our paddle game.
	/// 
	/// The entity has the following components:
    ///   - a sprite (including position and area)
	///   - a velocity
	/// </summary>
	public class BallEntity: SDL2_CS_COP.Entity
	{
        /// <summary>
        /// The speed (pixels/frame) at which the ball moves.
        /// </summary>
		public const int BALL_SPEED = 3;

        /// <summary>
        /// Gets or sets the sprite component.
        /// </summary>
        /// <value>The sprite component.</value>
		public SDL2_CS_COP.StandardItems.Components.Sprite SpriteComponent { get; set; }

        /// <summary>
        /// Gets or sets the velocity component.
        /// </summary>
        /// <value>The velocity component.</value>
		public SDL2_CS_COP.StandardItems.Components.Velocity VelocityComponent { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestSDL2CSCOP.BallEntity"/> class.
		/// </summary>
		/// <param name="world">World.</param>
		/// <param name="spriteComponent">Sprite component.</param>
		/// <param name="posx">initial X position.</param>
		/// <param name="posy">initial Y position.</param>
		public BallEntity (SDL2_CS_COP.World world, SDL2_CS_COP.StandardItems.Components.Sprite spriteComponent, int posx=0, int posy=0): base(world)
		{
			this.SpriteComponent = spriteComponent;
			this.SpriteComponent.MoveTo (posx, posy);
			this.VelocityComponent = new SDL2_CS_COP.StandardItems.Components.Velocity();
		}
	}
}

