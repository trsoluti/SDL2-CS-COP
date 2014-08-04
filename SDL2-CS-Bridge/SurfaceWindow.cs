using System;
using SDL2;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// Window, which has a title, size and underlying surface.
    /// </summary>
    public class SurfaceWindow: Surface, IWindow
    {
        /// <summary>
        /// The underlying SDL window object.
        /// </summary>
        private IntPtr _sdl_window;

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Window"/> class.
        /// </summary>
        /// <param name="bridge">Bridge.</param>
        /// <param name="title">Title.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="flags">Flags.</param>
		public SurfaceWindow (SDL2_CS_Bridge bridge,
                       string title,
                       int x,
                       int y,
                       int w,
                       int h,
                       SDL2.SDL.SDL_WindowFlags flags): base(IntPtr.Zero)
        {

            this._sdl_window = SDL2.SDL.SDL_CreateWindow (title, x, y, w, h, flags);
            bridge.RegisterWindow (this);
            this.SDLSurface = SDL2.SDL.SDL_GetWindowSurface(this._sdl_window);
        }

        /// <summary>
        /// Show the window.
        /// </summary>
        public void Show()
        {
            SDL2.SDL.SDL_ShowWindow(this._sdl_window);
        }

        /// <summary>
        /// Update the window surface (i.e. display all blitted objects).
        /// </summary>
        /// <returns>The window surface.</returns>
        public int UpdateWindowSurface()
        {
            return SDL2.SDL.SDL_UpdateWindowSurface(this._sdl_window);
        }

        /// <summary>
        /// Gets the SDL window.
        /// </summary>
        /// <value>The SDL window.</value>
		protected IntPtr SDLWindow { get { return this._sdl_window; } }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SDL2_CS_Bridge.Window"/> is reclaimed by garbage collection.
        /// </summary>
        ~SurfaceWindow()
        {
            SDL2.SDL.SDL_DestroyWindow(this._sdl_window);
        }
    }
}

