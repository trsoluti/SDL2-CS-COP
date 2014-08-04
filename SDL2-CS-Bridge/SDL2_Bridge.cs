using System;
using SDL2;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// a hardware abstraction layer between SDL2-CS and SDL2-CS-COP.
    /// </summary>
    /// <remarks>
    /// SDL2-CS-Bridge provides the interface between the SDL2-CS-COP system
    /// and the actual SDL2 implementation of that system.
    /// 
    /// The intent of the bridge is twofold:
    /// 1) to keep hardware and SDL dependencies in SDL2-CS-COP to a minimum
    /// 2) to hide C-dependencies in the SDL2-CS code (such as freeing allocated structures)
    ///
    /// The basic components of the bridge include:
    /// - Windows, which are based on renderers,
    /// - Drawing Items, which are based on either textures or surfaces,
    /// - factories for sprites and windows
    /// - assorted small classes and structures to wrap around SDL2 structures
    /// 
    /// This class is the root class of the bridge -- Only instantiate one of these!
    /// </remarks>
	public class SDL2_CS_Bridge
    {
        /// <summary>
        /// Gets or sets the renderer.
        /// </summary>
        /// <value>The renderer.</value>
        public Renderer Renderer { get; set; }
        /// <summary>
        /// Whether or not the system is using the SDL2 renderer functions
        /// </summary>
        private Boolean _isUsingSDL2Renderer; // if false, will be using own built-in emulator based on SDL2 surfaces
        /// <summary>
        /// The list of windows.
        /// </summary>
		private System.Collections.Generic.List<IWindow> _windows;
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.SDL2_CS_Bridge"/> class.
        /// </summary>
        /// <param name="isUsingSDL2Renderer">If set to <c>true</c> use the SDL2 renderer.</param>
		public SDL2_CS_Bridge (Boolean isUsingSDL2Renderer=false)
        {
            SDL2.SDL.SDL_Init(SDL2.SDL.SDL_INIT_VIDEO);
			this._windows = new System.Collections.Generic.List<IWindow> ();
            this._isUsingSDL2Renderer = isUsingSDL2Renderer;
            if (isUsingSDL2Renderer) {
                Sprite.SpriteType = SpriteType.TEXTURE;
            } else {
                Sprite.SpriteType = SpriteType.SOFTWARE;
            }
        }
        /// <summary>
        /// Adds a window to the system.
        /// </summary>
        /// <returns>The window.</returns>
        /// <param name="title">Title.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="windowFlags">Window flags.</param>
        /// <param name="rendererFlags">Renderer flags.</param>
        public IWindow AddWindow(string title,
            int x,
            int y,
            int w,
            int h,
            SDL2.SDL.SDL_WindowFlags windowFlags=0,
            SDL2.SDL.SDL_RendererFlags rendererFlags=0)
        {
            IWindow newWindow = null;
            if (this._isUsingSDL2Renderer) {
                RenderedWindow renderedWindow = new RenderedWindow (this, title, x, y, w, h, windowFlags, rendererFlags);
                this.Renderer = renderedWindow;
                Sprite.Renderer = renderedWindow;
                newWindow = renderedWindow;
            } else {
                newWindow = new SurfaceWindow (this, title, x, y, w, h, windowFlags);
                this.Renderer = null;
                Sprite.Renderer = null;
            }
            return newWindow;
        }
        /// <summary>
        /// Registers the window.
        /// </summary>
        /// <param name="window">Window.</param>
        public void RegisterWindow(IWindow window)
        {
			this._windows.Add (window);
            // in case the user used new Window directly
            // instead of coming through AddWindow,
            // Ensure renderer and type are properly set
            if (window.GetType () == typeof(RenderedWindow)) {
                this._isUsingSDL2Renderer = true;
                Sprite.SpriteType = SpriteType.TEXTURE;
                this.Renderer = (RenderedWindow)window;
                Sprite.Renderer = this.Renderer;
            } else {
                this._isUsingSDL2Renderer = false;
                Sprite.SpriteType = SpriteType.SOFTWARE;
                this.Renderer = null;
                Sprite.Renderer = null;
            }
        }
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SDL2_CS_Bridge.SDL2_CS_Bridge"/> is reclaimed by garbage collection.
        /// </summary>
		~SDL2_CS_Bridge()
        {
            SDL2.SDL.SDL_Quit ();
        }
    }
    /// <summary>
    /// a bridge to the SDL2_Point structure with convenience methods.
    /// NOTE THIS IS A STRUCT (VALUE-PASSED), NOT A CLASS (REFERENCE-PASSED).
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// The sdl point.
        /// </summary>
		private SDL2.SDL.SDL_Point _sdl_point;
        /// <summary>
        /// Gets the SDL point.
        /// </summary>
        /// <value>The SDL point.</value>
		public SDL2.SDL.SDL_Point SDLPoint { get { return this._sdl_point; } }
        /// <summary>
        /// Gets or sets the x coordinate of the point.
        /// </summary>
        /// <value>The x.</value>
		public int x { get { return this._sdl_point.x; } set { this._sdl_point.x = value; } }
        /// <summary>
        /// Gets or sets the y coordinate of the point.
        /// </summary>
        /// <value>The y.</value>
		public int y { get { return this._sdl_point.y; } set { this._sdl_point.y = value; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Point"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Point(int x, int y)
        {
			this._sdl_point = new SDL2.SDL.SDL_Point ();
			this._sdl_point.x = x;
			this._sdl_point.y = y;
        }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SDL2_CS_Bridge.Point"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="SDL2_CS_Bridge.Point"/>.</returns>
        public override string ToString ()
        {
            return string.Format ("({1}, {2})", this.x, this.y);
        }
    }
    /// <summary>
    /// a simple size structure to simplify argument lists. NOTE THIS IS A STRUCT (VALUE-PASSED), NOT A CLASS (REFERENCE-PASSED).
    /// </summary>
	public struct Size
	{
        /// <summary>
        /// The width.
        /// </summary>
		public int Width;
        /// <summary>
        /// The height.
        /// </summary>
		public int Height;
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Size"/> struct.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
		public Size(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}
	}
}

