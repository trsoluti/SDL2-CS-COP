using System;

namespace SDL2_CS_COP.StandardItems.Components
{
    /// <summary>
    /// Standard component to handle items that have area, such as sprites.
    /// 
    /// Provides convenience methods such as WithinBounds and IsOverlappingArea
    /// That can be used by collision and rendering systems.
    /// </summary>
	public class Area: Position
	{
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
		public virtual int Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
		public virtual int Height { get; set; }
        /// <summary>
        /// Gets the top position.
        /// </summary>
        /// <value>The top.</value>
		public int Top { get { return this.Y; } }
        /// <summary>
        /// Gets the left position.
        /// </summary>
        /// <value>The left.</value>
		public int Left { get { return this.X; } }
        /// <summary>
        /// Gets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
		public int Bottom { get { return this.Y + this.Height - 1; } }
        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <value>The right.</value>
		public int Right { get { return this.X + this.Width - 1; } }
        /// <summary>
        /// Gets the vertical center.
        /// </summary>
        /// <value>The vertical center.</value>
		public int VerticalCenter { get { return this.Y + (this.Height / 2); } }
        /// <summary>
        /// Gets the horizontal center.
        /// </summary>
        /// <value>The horizontal center.</value>
		public int HorizontalCenter { get { return this.X + (this.Width / 2); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Components.Area"/> class
        /// without any width or height.
        /// </summary>
        /// <remarks>
        /// for some derived classes, width can't be set so need constructor that doesn't set
        /// </remarks>
		public Area (): base(0,0)
		{
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Components.Area"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="height">Height.</param>
        /// <param name="width">Width.</param>
		public Area ( int x, int y, int height, int width): base(x,y)
		{
			this.Width = width;
			this.Height = height;
		}
        /// <summary>
        /// Moves the instance within the given bounds.
        /// </summary>
        /// <param name="velocity">Velocity.</param>
        /// <param name="bounds">Bounds.</param>
		public void MoveWithinBounds( Velocity velocity, SDL2_CS_Bridge.Rectangle bounds)
		{
			this.MoveWithinBounds (velocity.IntVx, velocity.IntVy, bounds);
		}

		/// <summary>
		/// Moves the instance within specified bounds.
		/// </summary>
		/// <param name="deltax">delta x to move.</param>
		/// <param name="deltay">The y coordinate.</param>
		/// <param name="bounds">Bounds.</param>
		public void MoveWithinBounds( int deltax, int deltay, SDL2_CS_Bridge.Rectangle bounds)
		{
			int newx = Math.Min( Math.Max (this.X + deltax, bounds.x), bounds.Right-this.Width);
			int newy = Math.Min( Math.Max (this.Y + deltay, bounds.y), bounds.Bottom-this.Height);
			this.MoveTo (newx, newy);
		}
        /// <summary>
        /// Determines whether this instance is overlapping area the specified otherArea.
        /// </summary>
        /// <returns><c>true</c> if this instance is overlapping area the specified otherArea; otherwise, <c>false</c>.</returns>
        /// <param name="otherArea">Other area.</param>
		public bool IsOverlappingArea(Area otherArea)
		{
			return ((this.Top <= otherArea.Bottom && this.Top >= otherArea.Top)
				|| (this.Bottom <= otherArea.Bottom && this.Bottom >= otherArea.Top))
				&& ((this.Left >= otherArea.Left && this.Left <= otherArea.Right)
					|| (this.Right >= otherArea.Left && this.Right <= otherArea.Right));
		}
        /// <summary>
        /// Determines whether this instance is above the specified position.
        /// </summary>
        /// <returns><c>true</c> if this instance is above the specified otherPosition; otherwise, <c>false</c>.</returns>
        /// <param name="otherPosition">Other position.</param>
		public override bool IsAbove (Position otherPosition)
		{
			return this.Bottom < otherPosition.Y;
		}
        /// <summary>
        /// Determines whether this instance is left of the specified position.
        /// </summary>
        /// <returns><c>true</c> if this instance is left of the specified otherPosition; otherwise, <c>false</c>.</returns>
        /// <param name="otherPosition">Other position.</param>
		public override bool IsLeftOf (Position otherPosition)
		{
			return this.Right < otherPosition.X;
		}
        /// <summary>
        /// Determines whether this instance is above the specified area.
        /// </summary>
        /// <returns><c>true</c> if this instance is above the specified area; otherwise, <c>false</c>.</returns>
        /// <param name="otherArea">Other area.</param>
		public bool IsAbove (Area otherArea)
		{
			return this.IsAbove ((Position)otherArea);
		}
        /// <summary>
        /// Determines whether this instance is below the specified area.
        /// </summary>
        /// <returns><c>true</c> if this instance is below the specified area; otherwise, <c>false</c>.</returns>
        /// <param name="otherArea">Other area.</param>
		public bool IsBelow (Area otherArea)
		{
			return this.Top > otherArea.Bottom;
		}
        /// <summary>
        /// Determines whether this instance is left of the specified area.
        /// </summary>
        /// <returns><c>true</c> if this instance is left of the specified area; otherwise, <c>false</c>.</returns>
        /// <param name="otherArea">Other area.</param>
		public bool IsLeftOf (Area otherArea)
		{
			return this.IsLeftOf ((Position)otherArea);
		}
        /// <summary>
        /// Determines whether this instance is right of the specified area.
        /// </summary>
        /// <returns><c>true</c> if this instance is right of the specified area; otherwise, <c>false</c>.</returns>
        /// <param name="otherArea">Other area.</param>
		public bool IsRightOf (Area otherArea)
		{
			return this.Left > otherArea.Right;
		}
	}
}

