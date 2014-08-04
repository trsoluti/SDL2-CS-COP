using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// Bridge class to SDL color object
    /// 
    /// Note: convenience methods (e.g. converting from a name or hex string)
    /// should be added here.
    /// </summary>
	public class Color
	{
        /// <summary>
        /// The underlying SDL color object.
        /// </summary>
		private SDL2.SDL.SDL_Color _color;
        /// <summary>
        /// Gets or sets the red component of the color.
        /// </summary>
        /// <value>The red value (0-255).</value>
		public byte Red { get { return this._color.r; } set {this._color.r = value;} }
        /// <summary>
        /// Gets or sets the green component of the color..
        /// </summary>
        /// <value>The green value (0-255).</value>
		public byte Green { get { return this._color.g; } set {this._color.g = value;} }
        /// <summary>
        /// Gets or sets the blue component of the color..
        /// </summary>
        /// <value>The blue value (0-255).</value>
		public byte Blue { get { return this._color.b; } set {this._color.b = value;} }
        /// <summary>
        /// Gets or sets the alpha (opacity) component of the color..
        /// </summary>
        /// <value>The alpha value (0-255).</value>
		public byte Alpha { get { return this._color.a; } set {this._color.a = value;} }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Color"/> class.
        /// </summary>
        /// <param name="red">Red.</param>
        /// <param name="green">Green.</param>
        /// <param name="blue">Blue.</param>
        /// <param name="alpha">Alpha.</param>
		public Color (byte red, byte green, byte blue, byte alpha=255)
		{
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
			this.Alpha = alpha;
		}
	}
}

