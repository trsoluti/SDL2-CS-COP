using System;

namespace TestSDL2CSCOP
{
	/// <summary>
	/// Tracking AI controller.
    /// 
    /// Implements the tracking AI for the Pong game
	/// </summary>
	public class TrackingAIController: SDL2_CS_COP.Applicator
	{
        /// <summary>
        /// Gets or sets the minimum y position of the paddle.
        /// </summary>
        /// <value>Y position in pixels.</value>
		public Double MinY { get; set; }
        /// <summary>
        /// Gets or sets the max y position of the paddle.
        /// </summary>
        /// <value>Y position in pixels.</value>
		public Double MaxY { get; set; }
        /// <summary>
        /// Gets or sets the ball entity being tracked.
        /// </summary>
        /// <value>The ball object.</value>
		public BallEntity Ball { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestSDL2CSCOP.TrackingAIController"/> class.
		/// </summary>
		/// <param name="miny">Minimum y value for paddle.</param>
		/// <param name="maxy">Maximum y value for paddle.</param>
		public TrackingAIController (Double miny, Double maxy): base()
		{
			this.MinY = miny;
			this.MaxY = maxy;
			this.Ball = null;
		}

		/// <summary>
		/// Determines whether this applicator can process the specified entity.
		/// 
		/// </summary>
		/// <returns>
        /// <c>true</c> if the entity has sprite, velocity and player data<br>
        /// <c>false</c> otherwise
        /// </returns>
		/// <param name="theEntity">The entity.</param>
		public override bool CanProcess (SDL2_CS_COP.Entity theEntity)
		{
			/*
			 *  self.componenttypes = (PlayerData, Velocity, sdl2ext.Sprite)
			 */
			return theEntity.Contains(typeof(SDL2_CS_COP.StandardItems.Components.Sprite)) 
				&& theEntity.Contains(typeof(PlayerDataComponent)) 
				&& theEntity.Contains(typeof(SDL2_CS_COP.StandardItems.Components.Velocity));
		}

		/// <summary>
		/// Process the specified world and entities.
        /// </summary>
		/// <remarks>
        /// If the ball is moving away,
        /// move the paddle towards the center of the screen.
        /// 
        /// Otherwise, move the paddle towards the ball's y position.
		/// </remarks>
		/// <param name="world">World.</param>
		/// <param name="entities">Entities.</param>
		public override void Process(SDL2_CS_COP.World world, System.Collections.Generic.List<SDL2_CS_COP.Entity> entities)
		{
			foreach (SDL2_CS_COP.Entity entity in entities) {
				/*
				 * Pull the components out of the entity for processing
				 */
				PlayerDataComponent playerDataComponent = (PlayerDataComponent)entity [typeof(PlayerDataComponent)];
				SDL2_CS_COP.StandardItems.Components.Velocity velocityComponent = (SDL2_CS_COP.StandardItems.Components.Velocity)entity [typeof(SDL2_CS_COP.StandardItems.Components.Velocity)];
				SDL2_CS_COP.StandardItems.Components.Sprite spriteComponent = (SDL2_CS_COP.StandardItems.Components.Sprite)entity [typeof(SDL2_CS_COP.StandardItems.Components.Sprite)];

				/*
	             * Ignore all non-ai entities
	             */
				if (!playerDataComponent.IsAi)
					continue;
				/*
	             * Pull some useful data out of the sprite component
	             */
				double sheight = spriteComponent.Height;
				double centery = spriteComponent.VerticalCenter;

				/*
	             * If the ball is moving away from the AI,
	             */
				if (this.Ball.VelocityComponent.Vx < 0.0) {
					/*
		             * Move the paddle towards the center of the screen
		             */
					if (centery < this.MaxY / 2.0 - PlayerEntity.PADDLE_SPEED) {
						velocityComponent.Vy = PlayerEntity.PADDLE_SPEED;
					} else if (centery > this.MaxY / 2.0 + PlayerEntity.PADDLE_SPEED) {
						velocityComponent.Vy = -PlayerEntity.PADDLE_SPEED;
					} else {
						velocityComponent.Vy = 0.0;
					}
				} else {
					/*
		             * Move the paddle towards the ball's Y position
					 */
					double bcentry = this.Ball.SpriteComponent.VerticalCenter;
					if (bcentry < centery) {
						velocityComponent.Vy = -PlayerEntity.PADDLE_SPEED;
					} else if (bcentry > centery) {
						velocityComponent.Vy = PlayerEntity.PADDLE_SPEED;
					} else {
						velocityComponent.Vy = 0;
					}
				}
			}
		}
	}
}

