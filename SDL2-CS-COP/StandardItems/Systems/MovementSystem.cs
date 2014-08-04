using System;

namespace SDL2_CS_COP.StandardItems.Systems
{
    /// <summary>
    /// Simple movement system (no gravity or drag) for entities with position.
    /// </summary>
    public class MovementSystem: SDL2_CS_COP.ICOP_System
	{
        /// <summary>
        /// The bounds of movement.
        /// </summary>
		private SDL2_CS_Bridge.Rectangle _bounds;
        /// <summary>
        /// Gets the left boundary.
        /// </summary>
        /// <value>The left boundary.</value>
		public int LeftBoundary { get { return this._bounds.Left; } }
        /// <summary>
        /// Gets the top boundary.
        /// </summary>
        /// <value>The top boundary.</value>
		public int TopBoundary { get { return this._bounds.Top; } }
        /// <summary>
        /// Gets the right boundary.
        /// </summary>
        /// <value>The right boundary.</value>
		public int RightBoundary { get { return this._bounds.Right; } }
        /// <summary>
        /// Gets the bottom boundary.
        /// </summary>
        /// <value>The bottom boundary.</value>
		public int BottomBoundary { get { return this._bounds.Bottom; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Systems.MovementSystem"/> class.
        /// </summary>
        /// <param name="minx">Minx for movement.</param>
        /// <param name="miny">Miny for movement.</param>
        /// <param name="maxx">Maxx for movement.</param>
        /// <param name="maxy">Maxy for movement.</param>
		public MovementSystem (int minx, int miny, int maxx, int maxy)
		{
			this._bounds = new SDL2_CS_Bridge.Rectangle(minx, miny, (maxx+1-minx), maxy+1-miny);
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Systems.MovementSystem"/> class.
        /// </summary>
        /// <param name="bounds">Bounds of movement.</param>
		public MovementSystem (SDL2_CS_Bridge.Rectangle bounds)
		{
			this._bounds = bounds;
		}

        /// <summary>
        /// Determines whether this instance can process the specified entity.
        /// </summary>
        /// <returns><c>true</c> if the specified entity has a sprite component; otherwise, <c>false</c>.</returns>
        /// <param name="theEntity">The entity.</param>
		public bool CanProcess (SDL2_CS_COP.Entity theEntity)
		{
			/*
		     * self.componenttypes = Velocity, Area
			 */
			return theEntity.Contains (typeof(SDL2_CS_COP.StandardItems.Components.Area))
				&& theEntity.Contains(typeof(SDL2_CS_COP.StandardItems.Components.Velocity));
		}	

		/* def process(self, world, componentsets):
		 */
        /// <summary>
        /// Process the specified world and entities.
        /// </summary>
        /// <remarks>
        /// For each entity,
        /// apply <see cref="SDL2_CS_COP.StandardItems.Components.Area"/>'s MoveWithinBounds method
        /// on the entity's area component
        /// using the entity's velocity component. 
        /// </remarks>
        /// <param name="world">World.</param>
        /// <param name="entities">Entities.</param>
		public void Process (SDL2_CS_COP.World world, System.Collections.Generic.List<SDL2_CS_COP.Entity> entities)
		{
			/*
		     * for velocity, sprite in componentsets:
			 */
			foreach (SDL2_CS_COP.Entity entity in entities) {
				/*
			     * swidth, sheight = sprite.size
			     * sprite.x += velocity.vx
			     * sprite.y += velocity.vy
			     */
				SDL2_CS_COP.StandardItems.Components.Area areaComponent = (SDL2_CS_COP.StandardItems.Components.Area)entity[typeof(SDL2_CS_COP.StandardItems.Components.Area)];
				SDL2_CS_COP.StandardItems.Components.Velocity velocityComponent = (SDL2_CS_COP.StandardItems.Components.Velocity)entity [typeof(SDL2_CS_COP.StandardItems.Components.Velocity)];
				int componentWidth = areaComponent.Width;
				int componentHeight =areaComponent.Height;
				areaComponent.MoveWithinBounds ((int)velocityComponent.Vx, (int)velocityComponent.Vy, this._bounds);
			}
		}
	}
}

