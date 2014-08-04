using System;

namespace SDL2_CS_COP.StandardItems.Components
{
    /// <summary>
    /// Standard component to handle sprites.
    /// 
    /// Wrapper class for the Bridge sprite which does the actual rendering
    /// but derived from Area and Position which means systems that work
    /// on those will also manage sprites.
    /// </summary>
	public class Sprite: Area
	{
        /// <summary>
        /// Gets the bridge sprite.
        /// </summary>
        /// <value>The bridge sprite.</value>
		public SDL2_CS_Bridge.Sprite BridgeSprite { get; private set; }
        /// <summary>
        /// Gets the width (setting not allowed).
        /// </summary>
        /// <value>The width.</value>
		public override int Width { get { return this.BridgeSprite.Width; } /*no set allowed*/ }
        /// <summary>
        /// Gets the height (setting not allowed).
        /// </summary>
        /// <value>The height.</value>
		public override int Height { get { return this.BridgeSprite.Height; } /*no set allowed*/ }
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
		public double Depth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Components.Sprite"/> class.
        /// </summary>
        /// <param name="sprite">Bridge Sprite.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
		public Sprite (SDL2_CS_Bridge.Sprite sprite, int x=0, int y=0)
		{
			this.BridgeSprite = sprite;
			this.MoveTo (x, y);
		}
        /// <summary>
        /// Determines whether this instance is overlapping sprite the specified otherSprite.
        /// </summary>
        /// <returns><c>true</c> if this instance is overlapping the specified otherSprite; otherwise, <c>false</c>.</returns>
        /// <param name="otherSprite">Other sprite.</param>
		public Boolean IsOverlappingSprite(Sprite otherSprite)
		{
			return this.IsOverlappingArea ((Area)otherSprite);
		}
	}
}

