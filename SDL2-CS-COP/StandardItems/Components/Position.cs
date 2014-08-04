using System;

namespace SDL2_CS_COP.StandardItems.Components
{
    /// <summary>
    /// Standard component to handle position information.
    /// 
    /// Contains convenience methods to support collision, etc.
    /// </summary>
	public class Position: SDL2_CS_COP.Component
	{
        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        /// <value>The x coordinate.</value>
		public int X { get; private set; }
        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        /// <value>The y coordinate.</value>
		public int Y { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Components.Position"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
		public Position (int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
        /// <summary>
        /// Gets the position as a <see cref="SDL2_CS_Bridge.Point"/> .
        /// </summary>
        /// <value>The position point.</value>
        public SDL2_CS_Bridge.Point PositionPoint { get { return new SDL2_CS_Bridge.Point(this.X, this.Y); } }
        /// <summary>
        /// Determines whether this instance is above the specified otherPosition.
        /// </summary>
        /// <returns><c>true</c> if this instance is above the specified otherPosition; otherwise, <c>false</c>.</returns>
        /// <param name="otherPosition">Other position.</param>
		public virtual Boolean IsAbove(Position otherPosition)
		{
			return this.Y < otherPosition.Y;
		}
        /// <summary>
        /// Determines whether this instance is below the specified otherPosition.
        /// </summary>
        /// <returns><c>true</c> if this instance is below the specified otherPosition; otherwise, <c>false</c>.</returns>
        /// <param name="otherPosition">Other position.</param>
		public virtual Boolean IsBelow(Position otherPosition)
		{
			return this.Y > otherPosition.Y;
		}
        /// <summary>
        /// Determines whether this instance is left of the specified otherPosition.
        /// </summary>
        /// <returns><c>true</c> if this instance is left of the specified otherPosition; otherwise, <c>false</c>.</returns>
        /// <param name="otherPosition">Other position.</param>
		public virtual Boolean IsLeftOf(Position otherPosition)
		{
			return this.X < otherPosition.X;
		}
        /// <summary>
        /// Determines whether this instance is right of the specified otherPosition.
        /// </summary>
        /// <returns><c>true</c> if this instance is right of the specified otherPosition; otherwise, <c>false</c>.</returns>
        /// <param name="otherPosition">Other position.</param>
		public virtual Boolean IsRightOf(Position otherPosition)
		{
			return this.X > otherPosition.X;
		}
        /// <summary>
        /// Move at the specified velocity.
        /// </summary>
        /// <param name="velocity">Velocity.</param>
		public void Move(Velocity velocity)
		{
			this.Move (velocity.IntVx, velocity.IntVy);
		}
        /// <summary>
        /// Move the specified x and y.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
		public void Move(int x, int y)
		{
			this.X += x;
			this.Y += y;
		}
        /// <summary>
        /// Moves to the specified position.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
		public void MoveTo(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

	}
}

