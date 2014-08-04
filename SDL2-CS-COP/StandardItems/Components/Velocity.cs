using System;

namespace SDL2_CS_COP.StandardItems.Components
{
    /// <summary>
    /// Standard component to handle velocity.
    /// </summary>
	public class Velocity: SDL2_CS_COP.Component
	{
        /// <summary>
        /// Gets or sets the x velocity as floating point.
        /// </summary>
        /// <value>The x velocity as floating point.</value>
		public double Vx { get; set; }
        /// <summary>
        /// Gets or sets the y velocity as floating point.
        /// </summary>
        /// <value>The y velocity as floating point.</value>
		public double Vy { get; set; }
        /// <summary>
        /// Gets or sets the x velocity as integer.
        /// </summary>
        /// <value>The x velocity as integer.</value>
		public int IntVx { get { return (int)this.Vx; } set { this.Vx = (double)value; } }
        /// <summary>
        /// Gets or sets the int y velocity as integer.
        /// </summary>
        /// <value>The y velocity as integer.</value>
		public int IntVy { get { return (int)this.Vy; } set { this.Vy = (double)value; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Components.Velocity"/> class.
        /// </summary>
        /// <param name="vx">X velocity (floating point).</param>
        /// <param name="vy">Y velocity (floating point).</param>
		public Velocity (double vx, double vy)
		{
			this.Vx = vx;
			this.Vy = vy;
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Components.Velocity"/> class.
        /// </summary>
        /// <param name="vx">X velocity (integer).</param>
        /// <param name="vy">Y velocity (integer).</param>
		public Velocity (int vx, int vy)
		{
			this.Vx = (double)vx;
			this.Vy = (double)vy;
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.StandardItems.Components.Velocity"/> class
        /// with no velocity.
        /// </summary>
		public Velocity () : this (0, 0)
		{
		}
	}
}

