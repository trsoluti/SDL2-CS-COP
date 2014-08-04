using System;

namespace TestSDL2CSCOP
{
    /// <summary>
    /// Simple collision system for our paddle game.
    /// </summary>
    /// <remarks>
    /// This system checks to see whether the ball
    /// has hit a paddle or the wall,
    /// and adjusts the direction accordingly.
    /// </remarks>
	public class CollisionSystem: SDL2_CS_COP.Applicator
	{
        /// <summary>
        /// Gets or sets the minimum x position for the ball.
        /// </summary>
        /// <value>The minimum x.</value>
		public int MinX { get; set; }
        /// <summary>
        /// Gets or sets the minimum y position for the ball.
        /// </summary>
        /// <value>The minimum y.</value>
		public int MinY { get; set; }
        /// <summary>
        /// Gets or sets the maximum x position for the ball.
        /// </summary>
        /// <value>The maximum x.</value>
		public int MaxX { get; set; }
        /// <summary>
        /// Gets or sets the maximum y position for the ball.
        /// </summary>
        /// <value>The maximum y.</value>
		public int MaxY { get; set; }
        /// <summary>
        /// Gets or sets the ball entity.
        /// </summary>
        /// <value>The ball entity.</value>
		public BallEntity BallEntity { get; set; }
        /// <summary>
        /// Gets the ball's sprite component.
        /// </summary>
        /// <value>The ball sprite component.</value>
		private SDL2_CS_COP.StandardItems.Components.Sprite BallSpriteComponent { get { return (SDL2_CS_COP.StandardItems.Components.Sprite)BallEntity [typeof(SDL2_CS_COP.StandardItems.Components.Sprite)]; } }
		/*
	     * def __init__(self, minx, miny, maxx, maxy):
	     *     super(CollisionSystem, self).__init__()
		 */
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSDL2CSCOP.CollisionSystem"/> class.
        /// </summary>
        /// <param name="minx">Minimum x position for the ball.</param>
        /// <param name="miny">Minimum y position for the ball.</param>
        /// <param name="maxx">Maximum x position for the ball.</param>
        /// <param name="maxy">Maximum y position for the ball.</param>
		public CollisionSystem (int minx, int miny, int maxx, int maxy)
		{
			/*
		     * self.ball = None
		     * self.minx = minx
		     * self.miny = miny
		     * self.maxx = maxx
		     * self.maxy = maxy
		     */
			this.BallEntity = null;
			this.MinX = minx;
			this.MinY = miny;
			this.MaxX = maxx;
			this.MaxY = maxy;
		}
        /// <summary>
        /// Determines whether this instance can process the specified entity.
        /// </summary>
        /// <returns><c>true</c> if the entity has velocity and sprite components; otherwise
        /// <c>false</c></returns>
        /// <param name="entity">The entity to be checked.</param>
		public override bool CanProcess(SDL2_CS_COP.Entity entity)
		{
			/* self.componenttypes = Velocity, sdl2.ext.Sprite
		     */
			return entity.Contains (typeof(SDL2_CS_COP.StandardItems.Components.Velocity)) && entity.Contains (typeof(SDL2_CS_COP.StandardItems.Components.Sprite));
		}
		/* def _overlap(self, item):
	     */ 
        /// <summary>
        /// Determines whether the specified sprite is overlapping the ball.
        /// </summary>
        /// <returns><c>true</c> if the specified sprite is overlapping the ball; otherwise, <c>false</c>.</returns>
        /// <param name="sprite">Sprite.</param>
		private bool IsOverlappingBall(SDL2_CS_COP.StandardItems.Components.Sprite sprite)
		{
			/* sprite = item[1]
		     * if sprite == self.ball.sprite:
		     *     return False
		     * 
		     * left, top, right, bottom = sprite.area
		     * bleft, btop, bright, bbottom = self.ball.sprite.area
		     * 
		     * return (bleft < right and bright > left and
		     *         btop < bottom and bbottom > top)
		     */
			return this.BallSpriteComponent.IsOverlappingSprite (sprite);
		}
		/* def process(self, world, componentsets):
		 */
        /// <summary>
        /// Process the specified world and entities.
        /// </summary>
        /// <remarks>
        /// This method checks to see if any of the given sprites
        /// have collided with the ball.
        /// If so, it bounces the ball back towards the center of the court
        /// with the appropriate vertical movement, depending on which
        /// half of the paddle was hit.
        /// 
        /// If there was no collision with a paddle,
        /// this process checks to see if the ball has hit any of the four walls.
        /// If so, it bounces the ball back without touching the other part of the velocity.
        /// </remarks>
        /// <param name="world">World.</param>
        /// <param name="entities">Entities.</param>
		public override void Process (SDL2_CS_COP.World world, System.Collections.Generic.List<SDL2_CS_COP.Entity> entities)
		{
			/* collitems = [comp for comp in componentsets if self._overlap(comp)]
		     * if len(collitems) != 0:
		     *     self.ball.velocity.vx = -self.ball.velocity.vx
		     * 
		     *     sprite = collitems[0][1]
		     *     ballcentery = self.ball.sprite.y + self.ball.sprite.size[1] // 2
		     *     halfheight = sprite.size[1] // 2
		     *     stepsize = halfheight // 10
		     *     degrees = 0.7
		     *     paddlecentery = sprite.y + halfheight
		     *     if ballcentery < paddlecentery:
		     *         factor = (paddlecentery - ballcentery) // stepsize
		     *         self.ball.velocity.vy = -int(round(factor * degrees))
		     *     elif ballcentery > paddlecentery:
		     *         factor = (ballcentery - paddlecentery) // stepsize
		     *         self.ball.velocity.vy = int(round(factor * degrees))
		     *     else:
		     *         self.ball.velocity.vy = -self.ball.velocity.vy
		     * 
		     * if (self.ball.sprite.y <= self.miny or
		     *     self.ball.sprite.y + self.ball.sprite.size[1] >= self.maxy):
		     *     self.ball.velocity.vy = -self.ball.velocity.vy
		     * 
		     * if (self.ball.sprite.x <= self.minx or
		     *     self.ball.sprite.x + self.ball.sprite.size[0] >= self.maxx):
		     *     self.ball.velocity.vx = -self.ball.velocity.vx
			 */
			SDL2_CS_COP.StandardItems.Components.Velocity ballVelocity = (SDL2_CS_COP.StandardItems.Components.Velocity)this.BallEntity [typeof(SDL2_CS_COP.StandardItems.Components.Velocity)];
			System.Collections.Generic.List<SDL2_CS_COP.StandardItems.Components.Sprite> overlappingSprites = new System.Collections.Generic.List<SDL2_CS_COP.StandardItems.Components.Sprite> (entities.Count);
			foreach (SDL2_CS_COP.Component component in SDL2_CS_COP.Entity.AllComponentsOfType(entities,typeof(SDL2_CS_COP.StandardItems.Components.Sprite))) {
				SDL2_CS_COP.StandardItems.Components.Sprite spriteComponent = (SDL2_CS_COP.StandardItems.Components.Sprite)component;
				if (spriteComponent != this.BallSpriteComponent && IsOverlappingBall (spriteComponent))
					overlappingSprites.Add (spriteComponent);
			}
			if (overlappingSprites.Count > 0) { 
				// no matter what, ball should move towards the centre of the court
				int courtHorizontalCentre = (this.MaxX - this.MinX) / 2;
				double absoluteVelocity = ballVelocity.Vx < 0 ? 0 - ballVelocity.Vx : ballVelocity.Vx;
				ballVelocity.Vx = this.BallSpriteComponent.HorizontalCenter > courtHorizontalCentre ? 0 - absoluteVelocity : absoluteVelocity;

				SDL2_CS_COP.StandardItems.Components.Sprite paddle = overlappingSprites [0]; // can't have more than 1 paddle overlap at a time

				int ballCenterY = this.BallSpriteComponent.VerticalCenter;
				int halfHeight = paddle.Height / 2;
				int stepSize = halfHeight / 10;
				double degrees = 0.7;
				int paddleCenterY = paddle.VerticalCenter;
				if (ballCenterY < paddleCenterY) {
					int factor = (paddleCenterY - ballCenterY) / stepSize;
					ballVelocity.Vy = 0 - (int)(Math.Round (factor * degrees));
				} else if (ballCenterY < paddleCenterY) {
					int factor = (ballCenterY - paddleCenterY) / stepSize;
					ballVelocity.Vy = (int)(Math.Round (factor * degrees));
				} else {
					ballVelocity.Vy = 0 - ballVelocity.Vy;
				}
			}

			// check if hitting top or bottom, left or right:
			if (BallSpriteComponent.Top <= this.MinY || BallSpriteComponent.Bottom + 1 >= this.MaxY) {
				ballVelocity.Vy = 0 - ballVelocity.Vy;
			} if (BallSpriteComponent.Left <= this.MinX || BallSpriteComponent.Right + 1 >= this.MaxX) {
				ballVelocity.Vx = 0 - ballVelocity.Vx;
			}
		}
	}
}

