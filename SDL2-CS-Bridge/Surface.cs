using System;
using SDL2;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// This class encapsulates all the surface information of SDL
    /// to remove hardware and implementation dependencies
    /// </summary>
    public class Surface: IDrawingBase
    {
		// TODO: revise class to support different types of surfaces, e.g. SDL_PixelFormat

        /// <summary>
        /// The underlying SDL surface.
        /// </summary>
        private IntPtr _sdl_surface;

        /// <summary>
        /// Whether the surface can be freed on destroy.
        /// </summary>
		private Boolean _isFreeOnDestroy;

        /// <summary>
        /// The child surface instances attached to this SDL surface.
        /// </summary>
		private System.Collections.Generic.List<Surface> _children;

        /// <summary>
        /// The Surface instance which "owns" the SDL surface.
        /// If it is NULL, this instance owns the SDL surface.
        /// </summary>
		private Surface _parent;

        /// <summary>
        /// Gets the height of the object.
        /// </summary>
        /// <value>The height in pixels.</value>
		public int Height { 
			get { 
				SDL2.SDL.SDL_Surface managedSurface = (SDL2.SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure (this._sdl_surface, typeof(SDL2.SDL.SDL_Surface));
				return managedSurface.h; 
			} 
		}

        /// <summary>
        /// Gets the width of the object.
        /// </summary>
        /// <value>The width in pixels.</value>
		public int Width { 
			get { 
				SDL2.SDL.SDL_Surface managedSurface = (SDL2.SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure (this._sdl_surface, typeof(SDL2.SDL.SDL_Surface));
				return managedSurface.w; 
			} 
		}

        /// <summary>
        /// Gets the pixel format used by the SDL surface.
        /// </summary>
        /// <value>The pixel format.</value>
		public PixelFormat PixelFormat {
			get {
				SDL2.SDL.SDL_Surface managedSurface = (SDL2.SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure (this._sdl_surface, typeof(SDL2.SDL.SDL_Surface));
				return new PixelFormat (managedSurface.format);
			}
		}
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public IntPtr Target { get { return this._sdl_surface; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Surface"/> class.
        /// </summary>
        /// <param name="sdl_surface">SDL surface.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Surface (IntPtr sdl_surface, Boolean freeOnDestroy=false )
        {
            this._sdl_surface = sdl_surface;
            this._isFreeOnDestroy = freeOnDestroy;

			if (sdl_surface != IntPtr.Zero)

			this._parent = null;
			this._children = new System.Collections.Generic.List<Surface> ();
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Surface"/> class.
        /// 
        /// This version of the instance method is called when you need to create
        /// a Surface instance but you don't yet have an SDL Surface.
        /// (e.g. when creating a Window.)
        /// </summary>
        /// <param name="surface">Surface.</param>
		public Surface (Surface surface): this(surface == null ? IntPtr.Zero : surface._sdl_surface, false)
		{
			if (surface != null) {
				this._sdl_surface = surface._sdl_surface;
				// add this as a sub-surface so that the parent won't be destroyed until we are destroyed
				// In case we passed a child as our seed surface, find the real parent
				while (surface._parent != null)
					surface = surface._parent;
				this._parent = surface;
				surface._children.Add (this);
			}
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Surface"/> class.
        /// </summary>
        /// <param name="spriteColor">Sprite color.</param>
        /// <param name="size">Size.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="maskColor">Mask color.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Surface(Color spriteColor, Size size, int depth=32, Color maskColor=null, Boolean freeOnDestroy=false): this(IntPtr.Zero, freeOnDestroy)
        {
            uint rmask = 0;
            uint gmask = 0;
            uint bmask = 0;
            uint amask = 0;
            if (maskColor != null) {
                rmask = maskColor.Red;
                gmask = maskColor.Green;
                bmask = maskColor.Blue;
                amask = maskColor.Alpha;
            }
            this._sdl_surface = SDL2.SDL.SDL_CreateRGBSurface (0, size.Width, size.Height, depth, rmask, gmask, bmask, amask);
            this.Fill (spriteColor);
        }

        /// <summary>
        /// Gets or sets the SDL surface.
        /// </summary>
        /// <value>The SDL surface.</value>
		public IntPtr SDLSurface { 
			get { return this._sdl_surface; } 
			protected set { 
				this._sdl_surface = value;
			} 
		}

        /// <summary>
        /// Blit the specified surface and position to our surface.
        /// </summary>
        /// <param name="surface">Surface (e.g. sprite) to be blitted.</param>
        /// <param name="position">top left corner for positioning.</param>
        public void Blit(Surface surface, Point position)
        {
            this.Blit (surface.SDLSurface, position);
        }
        /// <summary>
        /// Blit the specified texture and position.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        public void Blit(Texture texture, Point position)
        {
            // not supporting blitting textures to surfaces.
            // Should be using Window based on texture instead
            throw new NotImplementedException ();
        }
        /// <summary>
        /// Blit the specified drawingItem and position.
        /// </summary>
        /// <param name="drawingItem">Drawing item.</param>
        /// <param name="position">Position.</param>
        public void Blit(DrawingItem drawingItem, Point position)
        {
            if (drawingItem.SDLSurface == IntPtr.Zero) {
                throw new NotImplementedException ();
            }
            this.Blit (drawingItem.SDLSurface, position);
        }
        /// <summary>
        /// Blit the specified sdlSurface and position.
        /// </summary>
        /// <param name="sdlSurface">Sdl surface.</param>
        /// <param name="position">Position.</param>
        private void Blit(IntPtr sdlSurface, Point position)
        {
            SDL2.SDL.SDL_Surface managedSurface = (SDL2.SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure (sdlSurface, typeof(SDL2.SDL.SDL_Surface));
            SDL2.SDL.SDL_Rect first_rect = new SDL2.SDL.SDL_Rect ();
            first_rect.x = 0;
            first_rect.y = 0;
            first_rect.w = managedSurface.w;
            first_rect.h = managedSurface.h;
            SDL2.SDL.SDL_Rect second_rect = new SDL2.SDL.SDL_Rect ();
            second_rect.x = position.x;
            second_rect.y = position.y;
            second_rect.w = managedSurface.w;
            second_rect.h = managedSurface.h;
            SDL2.SDL.SDL_UpperBlit(sdlSurface, ref first_rect, this._sdl_surface, ref second_rect);
        }

        /// <summary>
        /// Fill the specified areas with the specified color.
        /// 
        /// If no areas, fill the entire surface.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="areas">Areas.</param>
		public void Fill( Color color, System.Collections.Generic.List<Rectangle> areas=null)
		{
			uint pixel = this.PixelFormat.MapColor (color);

            // if no areas, fill the surface
			if (areas == null) {
				SDL2.SDL.SDL_Rect fullSurface = new SDL.SDL_Rect ();
				fullSurface.x = 0;
				fullSurface.y = 0;
				fullSurface.w = this.Width;
				fullSurface.h = this.Height;
				SDL2.SDL.SDL_FillRect (this._sdl_surface, ref fullSurface, pixel);
			} else {
                // otherwise, fill each rectangle
				SDL2.SDL.SDL_Rect[] rectangles = new SDL.SDL_Rect[areas.Count];
				for (int iRect = 0; iRect < areas.Count; iRect++) {
					rectangles [iRect] = areas [iRect].SDLRectangle;
				}
				SDL2.SDL.SDL_FillRects (this._sdl_surface, rectangles, areas.Count, pixel);
			}
		}

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SDL2_CS_Bridge.Surface"/> is reclaimed by garbage collection.
        /// </summary>
        ~Surface()
        {
            if (this._isFreeOnDestroy) {
				// only free if no children around
				if (this._children.Count == 0)
                	SDL2.SDL.SDL_FreeSurface (this._sdl_surface);
				// TODO: I think what we need to do here is make the first child the parent,
				// and link the remaining children to it
				// for now we have a memory leak
                // (but it's likely GC won't happen on this object
                // as long as it has anything referring to it,
                // so this method won't be called
                // unless _children.Count is 0)
            }
			// remove us from the parent
			if (this._parent != null)
				this._parent._children.Remove (this);
        }
    }
}

