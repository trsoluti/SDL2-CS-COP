using System;

namespace SDL2_CS_COP.StandardItems.Systems
{
	/*
	 * class SpriteRenderSystem(System):
	 *     """A rendering system for Sprite components.
	 * 
	 *     This is a base class for rendering systems capable of drawing and
	 *     displaying Sprite-based objects. Inheriting classes need to
	 *     implement the rendering capability by overriding the render()
	 *     method.
	 *     """
	 */

	/// <summary>
	/// Sprite renderer -- A rendering system for Sprite components
	/// 
	/// Unlike PySDL2, the hardware aspects of rendering are buried
	/// in the Bridge Sprite class.
	/// 
	/// This renderer can render any <see cref="SDL2_CS_COP.StandardItems.Components.Sprite"/>
	/// or a component derived from it.
	/// 
	/// </summary>
	public class SpriteRenderer: SDL2_CS_COP.ICOP_System
	{
        /// <summary>
        /// Gets or sets the window.
        /// </summary>
        /// <value>The window.</value>
		private SDL2_CS_Bridge.IWindow Window { get; set; }
        /// <summary>
        /// Gets or sets the sort function.
        /// </summary>
        /// <value>The sort function.</value>
		public System.Comparison<StandardItems.Components.Sprite> SortFunction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Systems.SpriteRenderer"/> class.
        /// </summary>
        /// <param name="window">Window.</param>
		public SpriteRenderer (SDL2_CS_Bridge.IWindow window)
		{
			this.SortFunction = delegate(SDL2_CS_COP.StandardItems.Components.Sprite x, SDL2_CS_COP.StandardItems.Components.Sprite y) {
				// we normalize the result to be -1, 0 or 1:
				return (int)((x.Depth - y.Depth)/(x.Depth-y.Depth));
			};
			this.Window = window;
		}
        /// <summary>
        /// Process the specified world and entities.
        /// </summary>
        /// <remarks>
        /// Extract all the sprites, then sort them (default: sort by depth),
        /// and finally render them in order.
        /// </remarks>
        /// <param name="world">World.</param>
        /// <param name="entities">Entities.</param>
		public virtual void Process(World world, System.Collections.Generic.List<Entity> entities)
		{
			System.Collections.Generic.List<SDL2_CS_COP.StandardItems.Components.Sprite> sprites = 
				new System.Collections.Generic.List<SDL2_CS_COP.StandardItems.Components.Sprite> (entities.Count);
			foreach (SDL2_CS_COP.Component component in Entity.AllComponentsOfType (entities, typeof(SDL2_CS_COP.StandardItems.Components.Sprite))) {
				sprites.Add ((SDL2_CS_COP.StandardItems.Components.Sprite)component);
			}
			sprites.Sort (this.SortFunction);
			foreach (SDL2_CS_COP.StandardItems.Components.Sprite sprite in sprites) {
                sprite.BridgeSprite.Render (this.Window, sprite.PositionPoint);
			}
		}
        /// <summary>
        /// Determines whether this instance can process the specified entity.
        /// </summary>
        /// <returns><c>true</c> if the specified entity has a sprite component; otherwise, <c>false</c>.</returns>
        /// <param name="entity">The entity.</param>
		public virtual bool CanProcess(Entity entity)
		{
			return entity.Contains (typeof(StandardItems.Components.Sprite));
		}
	}
}

