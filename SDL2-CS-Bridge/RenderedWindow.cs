using System;
using SDL2;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// RenderedWindow
    /// 
    /// A rendered window is the base drawing context. All updates
    /// are two this area using the method PostItem
    /// 
    /// The mechanism hides all hardware and software rendering functions
    /// from the remaining code, meaning there are no longer any
    /// hardware or software renderers or tools.
    /// 
    /// Note all rendering methods (shapes, lines etc) should be
    /// encorporated into this class.
    /// 
    /// </summary>
    public class RenderedWindow: Renderer, IWindow
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Window"/> class.
        /// </summary>
        /// <param name="bridge">Bridge.</param>
        /// <param name="title">Title.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="window_flags">Window flags.</param>
        /// <param name="render_flags">Render flags.</param> 
		public RenderedWindow (SDL2_CS_Bridge bridge,
            string title,
            int x,
            int y,
            int w,
            int h,
            SDL2.SDL.SDL_WindowFlags window_flags,
            SDL2.SDL.SDL_RendererFlags render_flags): base()
        {
            IntPtr window = SDL2.SDL.SDL_CreateWindow (title, x, y, w, h, window_flags);
            base.SetRendererAndTarget (window, flags: render_flags);

            bridge.RegisterWindow (this);
            // now the window is created, pass its surface to our parent
            //this.SDLSurface = SDL2.SDL.SDL_GetWindowSurface(this._sdl_window);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.RenderedWindow"/> class.
        /// </summary>
        /// <param name="baseWindow">Base window.</param>
        /// <param name="render_flags">Render flags.</param>
        public RenderedWindow (IntPtr baseWindow, SDL2.SDL.SDL_RendererFlags render_flags): base()
        {
            base.SetRendererAndTarget (baseWindow, flags: render_flags);
        }
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height {
            get {
                Surface window_surface = new Surface (SDL2.SDL.SDL_GetWindowSurface (this.RenderTarget));
                return window_surface.Height;
            }
        }
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width {
            get {
                Surface window_surface = new Surface (SDL2.SDL.SDL_GetWindowSurface (this.RenderTarget));
                return window_surface.Width;
            }
        }
        /// <summary>
        /// Show the window.
        /// </summary>
        public void Show()
        {
            SDL2.SDL.SDL_ShowWindow(this.RenderTarget);
        }

        /// <summary>
        /// Fill the specified areas with the specified color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="areas">Areas (fill the window if null).</param>
        public void Fill(Color color, System.Collections.Generic.List<Rectangle> areas=null)
        {
            if (areas == null) {
                areas = new System.Collections.Generic.List<Rectangle> (1);
                areas.Add (new Rectangle (new Point (0, 0), new Size (this.Width, this.Height)));
            }
            this.FillRectangles (areas, color);
        }

        /// <summary>
        /// Present the updated the window surface (i.e. display all rendered objects).
        /// </summary>
        /// <returns>The window surface.</returns>
        public int UpdateWindowSurface()
        {
            return base.Present();
        }

        /// <summary>
        /// Gets the SDL window.
        /// </summary>
        /// <value>The SDL window.</value>
        protected IntPtr SDLWindow { get { return System.IntPtr.Zero; } }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SDL2_CS_Bridge.Window"/> is reclaimed by garbage collection.
        /// </summary>
        ~RenderedWindow()
        {
        }

    }
}

