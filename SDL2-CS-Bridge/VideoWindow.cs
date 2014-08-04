using System;

namespace SDL2_CS_Bridge
{
	/// <summary>
	/// Video window.
	/// 
	/// Not sure how this works yet.
	/// For now it's just a marker to differentiate
	/// creating renderers from surfaces or from the SDL_Window.
	/// </summary>
	public class VideoWindow: SurfaceWindow
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.VideoWindow"/> class.
        /// </summary>
        /// <param name="bridge">Bridge.</param>
        /// <param name="title">Title.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="flags">Flags.</param>
		public VideoWindow (SDL2_CS_Bridge bridge,
			string title,
			int x,
			int y,
			int w,
			int h,
			SDL2.SDL.SDL_WindowFlags flags): base(bridge, title, x, y, w, h, flags)
		{
		}
        /// <summary>
        /// Gets the SDL window.
        /// </summary>
        /// <value>The SDL window.</value>
		public new IntPtr SDLWindow { get { return base.SDLWindow; } }
	}
}

