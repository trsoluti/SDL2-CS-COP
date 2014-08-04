using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// Bridge class for SDL Rectangle.
    /// 
    /// Provides some convenience methods for access.
    /// </summary>
	public class Rectangle
	{
        /// <summary>
        /// The underlying SDL rectangle.
        /// </summary>
		private SDL2.SDL.SDL_Rect _rectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Rectangle"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
		public Rectangle (int x, int y, int width, int height)
		{
			_rectangle = new SDL2.SDL.SDL_Rect ();
			_rectangle.x = x;
			_rectangle.y = y;
			_rectangle.w = width;
			_rectangle.h = height;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Rectangle"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="size">Size.</param>
		public Rectangle (Point position, Size size)
		{
			this._rectangle.x = position.x;
			this._rectangle.y = position.y;
			this._rectangle.w = size.Width;
			this._rectangle.h = size.Height;
		}

        /// <summary>
        /// Gets the x position of the instance.
        /// </summary>
        /// <value>The x.</value>
		public int x { get { return _rectangle.x; } }
        /// <summary>
        /// Gets the y position of the instance.
        /// </summary>
        /// <value>The y.</value>
		public int y { get { return _rectangle.y; } }
        /// <summary>
        /// Gets the width of the instance.
        /// </summary>
        /// <value>The w.</value>
		public int w { get { return _rectangle.w; } }
        /// <summary>
        /// Gets the height of the instance.
        /// </summary>
        /// <value>The h.</value>
		public int h { get { return _rectangle.h; } }
        /// <summary>
        /// Gets the SDL rectangle.
        /// </summary>
        /// <value>The SDL rectangle.</value>
		public SDL2.SDL.SDL_Rect SDLRectangle { get { return _rectangle; } }
        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <value>The top.</value>
		public int Top { get { return this._rectangle.y; } }
        /// <summary>
        /// Gets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
		public int Bottom { get { return this._rectangle.y + this._rectangle.h - 1; } }
        /// <summary>
        /// Gets the left.
        /// </summary>
        /// <value>The left.</value>
		public int Left { get { return this._rectangle.x; } }
        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <value>The right.</value>
		public int Right { get { return this._rectangle.x + this._rectangle.w - 1; } }
	}
}

