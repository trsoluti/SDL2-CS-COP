using System;

namespace SDL2_CS_Bridge
{
	/*
	 * class Renderer(object):
	 *     """SDL2-based renderer for windows and sprites."""
	 */
    /// <summary>
    /// A bridge class to all the SDL2 rendering functions.
    /// </summary>
	public class Renderer
	{
        /// <summary>
        /// Gets or sets the renderer.
        /// </summary>
        /// <value>The renderer.</value>
		private IntPtr _renderer { get; set; }
        /// <summary>
        /// Gets or sets the render target.
        /// </summary>
        /// <value>The render target.</value>
		private IntPtr _renderTarget { get; set; }
        /// <summary>
        /// Gets the SDL renderer.
        /// </summary>
        /// <value>The SDL renderer.</value>
		public IntPtr SDLRenderer { get { return this._renderer; } }
        /// <summary>
        /// Gets the render target.
        /// </summary>
        /// <value>The render target.</value>
        protected IntPtr RenderTarget { get { return this._renderTarget; } }
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        int Width {
            get {
                SDL2.SDL.SDL_RendererInfo renderInfo;
                SDL2.SDL.SDL_GetRendererInfo (this._renderer, out renderInfo);
                return renderInfo.max_texture_width;
            }
        }
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        int Height {
            get {
                SDL2.SDL.SDL_RendererInfo renderInfo;
                SDL2.SDL.SDL_GetRendererInfo (this._renderer, out renderInfo);
                return renderInfo.max_texture_height;
            }
        }



		/*
		 * def __init__(self, target, index=-1,
		 *              flags=render.SDL_RENDERER_ACCELERATED):
		 *     """Creates a new Renderer for the given target.
		 * 
		 *     If target is a Window or SDL_Window, index and flags are passed
		 *     to the relevant sdl.render.create_renderer() call. If target is
		 *     a SoftwareSprite or SDL_Surface, the index and flags arguments are
		 *     ignored.
		 *     """
		 */
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Renderer"/> class.
        /// </summary>
        /// <param name="window">Window.</param>
        /// <param name="index">Index.</param>
        /// <param name="flags">Flags.</param>
		public Renderer (SurfaceWindow window, int index = -1, SDL2.SDL.SDL_RendererFlags flags = SDL2.SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED)
			: this (window.SDLSurface, index, flags)
		{
			/*
			 *     if isinstance(target, Window):
			 *         self.renderer = render.SDL_CreateRenderer(target.window, index,
			 *                                                   flags)
			 *         self.rendertarget = target.window
			 */
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Renderer"/> class.
        /// </summary>
        /// <param name="videoWindow">Video window.</param>
        /// <param name="index">Index.</param>
        /// <param name="flags">Flags.</param>
		public Renderer (VideoWindow videoWindow, int index = -1, SDL2.SDL.SDL_RendererFlags flags = SDL2.SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED)
			: this (videoWindow.SDLWindow, index, flags)
		{
			/*
			 *     elif isinstance(target, video.SDL_Window):
			 *         self.renderer = render.SDL_CreateRenderer(target, index, flags)
			 *         self.rendertarget = target
			 */
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Renderer"/> class.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="index">Index.</param>
        /// <param name="flags">Flags.</param>
		private Renderer (IntPtr target, int index = -1, SDL2.SDL.SDL_RendererFlags flags = SDL2.SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED)
		{
            this.SetRendererAndTarget(target, index, flags);
		}
			/*
			 *     elif isinstance(target, SoftwareSprite):
			 *         self.renderer = render.SDL_CreateSoftwareRenderer(target.surface)
			 *         self.rendertarget = target.surface
			 */
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Renderer"/> class.
        /// </summary>
        /// <param name="surface">Surface.</param>
		public Renderer (Surface surface)
		{
			/*
			 *     elif isinstance(target, surface.SDL_Surface):
			 *         self.renderer = render.SDL_CreateSoftwareRenderer(target)
			 *         self.rendertarget = target
			 */
            this.SetRendererAndTarget (surface);
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Renderer"/> class.
        /// </summary>
        /// <remarks>
        /// This is used by the derived window class to instantiate itself before
        /// the window and renderer are created
        /// </remarks>
        public Renderer ()
        {
            this._renderTarget = IntPtr.Zero;
            this._renderer = IntPtr.Zero;
        }
        /// <summary>
        /// Sets the renderer and target.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="index">Index.</param>
        /// <param name="flags">Flags.</param>
        protected void SetRendererAndTarget(IntPtr target, int index = 1, SDL2.SDL.SDL_RendererFlags flags = SDL2.SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED)
        {
            this._renderTarget = target;
            this._renderer = SDL2.SDL.SDL_CreateRenderer (target, index, (uint)flags);
        }
        /// <summary>
        /// Sets the renderer and target.
        /// </summary>
        /// <param name="surface">Surface.</param>
        protected void SetRendererAndTarget(Surface surface)
        {
            this._renderTarget = surface.SDLSurface;
            //PySDL uses CreateSoftwareRenderer in this case, but SDL2-CS doesn't export this api call
            //this._renderer = SDL2.SDL.SDL_CreateSoftwareRenderer (surface.SDLSurface);
            this._renderer = SDL2.SDL.SDL_CreateRenderer (surface.SDLSurface);
        }
		/* 
		 * def __del__(self):
		 *     if self.renderer:
		 *         render.SDL_DestroyRenderer(self.renderer)
		 *     self.rendertarget = None
		 */
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SDL2_CS_Bridge.Renderer"/> is reclaimed by garbage collection.
        /// </summary>
		~Renderer()
		{
			if (this._renderer != IntPtr.Zero)
				SDL2.SDL.SDL_DestroyRenderer(this._renderer);
			this._renderTarget = IntPtr.Zero;
		}
		/*
		 * @property
		 * def color(self):
		 *     """The drawing color of the Renderer."""
		 *     r, g, b, a = Uint8(), Uint8(), Uint8(), Uint8()
		 *     ret = render.SDL_GetRenderDrawColor(self.renderer, byref(r), byref(g),
		 *                                         byref(b), byref(a))
		 *     if ret == -1:
		 *         raise SDLError()
		 *     return convert_to_color((r.value, g.value, b.value, a.value))
		 * 
		 * @color.setter
		 * def color(self, value):
		 *     """The drawing color of the Renderer."""
		 *     c = convert_to_color(value)
		 *     ret = render.SDL_SetRenderDrawColor(self.renderer, c.r, c.g, c.b, c.a)
		 *     if ret == -1:
		 *         raise SDLError()
		 */
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
		public Color Color {
			get {
				Byte Red;
				Byte Green;
				Byte Blue;
				Byte Alpha;
				int returnValue = SDL2.SDL.SDL_GetRenderDrawColor (this._renderer, out Red, out Green, out Blue, out Alpha);
				if (returnValue == -1)
					throw new ApplicationException ("SDL Error");
				return new Color (Red, Green, Blue, Alpha);
			}
			set {
				Color color = (Color)value;
				int returnValue = SDL2.SDL.SDL_SetRenderDrawColor (this._renderer, color.Red, color.Green, color.Blue, color.Alpha);
				if (returnValue == -1)
					throw new ApplicationException ("SDL Error");
			}
		}
		/* 
		 * @property
		 * def blendmode(self):
		 *     """The blend mode used for drawing operations (fill and line)."""
		 *     mode = blendmode.SDL_BlendMode()
		 *     ret = render.SDL_GetRenderDrawBlendMode(self.renderer, byref(mode))
		 *     if ret == -1:
		 *         raise SDLError()
		 *     return mode
		 * 
		 * @blendmode.setter
		 * def blendmode(self, value):
		 *     """The blend mode used for drawing operations (fill and line)."""
		 *     ret = render.SDL_SetRenderDrawBlendMode(self.renderer, value)
		 *     if ret == -1:
		 *         raise SDLError()
		 */
        /// <summary>
        /// Gets or sets the blend mode.
        /// </summary>
        /// <value>The blend mode.</value>
		public SDL2.SDL.SDL_BlendMode BlendMode {
			get {
				SDL2.SDL.SDL_BlendMode blendMode = new SDL2.SDL.SDL_BlendMode ();
				int returnValue = SDL2.SDL.SDL_GetRenderDrawBlendMode (this._renderer, out blendMode);
				if (returnValue == -1)
					throw new ApplicationException ("SDL Error");
				return blendMode;
			}
			set {
				int returnValue = SDL2.SDL.SDL_SetRenderDrawBlendMode (this._renderer, BlendMode);
				if (returnValue == -1)
					throw new ApplicationException ("SDL Error");
			}
		}
		/* 
		 * def clear(self, color=None):
		 *     """Clears the renderer with the currently set or passed color."""
		 *     if color is not None:
		 *         tmp = self.color
		 *         self.color = color
		 *     ret = render.SDL_RenderClear(self.renderer)
		 *     if color is not None:
		 *         self.color = tmp
		 *     if ret == -1:
		 *         raise SDLError()
		 */
        /// <summary>
        /// Clear the specified color.
        /// </summary>
        /// <param name="color">Color.</param>
		public void Clear(Color color = null)
		{
			Color savedColor = this.Color;
			if (color != null)
				this.Color = color;
			int returnValue = SDL2.SDL.SDL_RenderClear (this._renderer);
			if (color != null)
				this.Color = savedColor;
			if (returnValue == -1)
				throw new ApplicationException ("SDL Error");
		}
		/* 
		 * def copy(self, src, srcrect=None, dstrect=None):
		 *     """Copies (blits) the passed source to the target of the Renderer."""
		 *     SDL_Rect = rect.SDL_Rect
		 *     if isinstance(src, TextureSprite):
		 *         texture = src.texture
		 *     elif isinstance(src, render.SDL_Texture):
		 *         texture = src
		 *     else:
		 *         raise TypeError("src must be a TextureSprite or SDL_Texture")
		 *     if srcrect is not None:
		 *         x, y, w, h = srcrect
		 *         srcrect = SDL_Rect(x, y, w, h)
		 *     if dstrect is not None:
		 *         x, y, w, h = dstrect
		 *         dstrect = SDL_Rect(x, y, w, h)
		 *     ret = render.SDL_RenderCopy(self.renderer, texture, srcrect, dstrect)
		 *     if ret == -1:
		 *         raise SDLError()
		 */
        /// <summary>
        /// Copy the specified texture, sourceRectangle and destinationRectangle.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="destinationRectangle">Destination rectangle.</param>
		public void Copy(Texture texture, Rectangle sourceRectangle = null, Rectangle destinationRectangle = null)
		{
			this.Copy (texture.SDLTexture, sourceRectangle, destinationRectangle);
		}
        /// <summary>
        /// Copy the specified sdlTexture, sourceRectangle and destinationRectangle.
        /// </summary>
        /// <param name="sdlTexture">Sdl texture.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="destinationRectangle">Destination rectangle.</param>
		public void Copy(IntPtr sdlTexture, Rectangle sourceRectangle = null, Rectangle destinationRectangle = null)
		{
			SDL2.SDL.SDL_Rect sourceSDLRectangle = sourceRectangle.SDLRectangle;
			SDL2.SDL.SDL_Rect destinationSDLRectangle = destinationRectangle.SDLRectangle;
			int returnValue = SDL2.SDL.SDL_RenderCopy (this._renderer, sdlTexture, ref sourceSDLRectangle, ref destinationSDLRectangle);
			if (returnValue == -1)
				throw new ApplicationException ("SDL Error");
		}
		/* 
		 * def present(self):
		 *     """Refreshes the target of the Renderer."""
		 *     render.SDL_RenderPresent(self.renderer)
		 */
        /// <summary>
        /// Present this instance.
        /// </summary>
		public int Present()
		{
			SDL2.SDL.SDL_RenderPresent (this._renderer);
            return 0;
		}
		/* 
		 * def draw_line(self, points, color=None):
		 *     """Draws one or multiple lines on the renderer."""
		 *     # (x1, y1, x2, y2, ...)
		 *     pcount = len(points)
		 *     if (pcount % 4) != 0:
		 *         raise ValueError("points does not contain a valid set of points")
		 *     if pcount == 4:
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         x1, y1, x2, y2 = points
		 *         ret = render.SDL_RenderDrawLine(self.renderer, x1, y1, x2, y2)
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 *     else:
		 *         x = 0
		 *         off = 0
		 *         SDL_Point = rect.SDL_Point
		 *         ptlist = (SDL_Point * pcount / 2)()
		 *         while x < pcount:
		 *             ptlist[off] = SDL_Point(points[x], points[x + 1])
		 *             x += 2
		 *             off += 1
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         ptr = cast(ptlist, POINTER(SDL_Point))
		 *         ret = render.SDL_RenderDrawLines(self.renderer, ptr, pcount / 2)
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 */
        /// <summary>
        /// Draws the lines.
        /// </summary>
        /// <param name="points">Points.</param>
        /// <param name="color">Color.</param>
		public void DrawLines(System.Collections.Generic.List<Point> points, Color color=null)
		{
			SDL2.SDL.SDL_Point[] pointArray = new SDL2.SDL.SDL_Point[points.Count];
			for (int iPoint = 0; iPoint < points.Count; iPoint++) {
				pointArray [iPoint] = points [iPoint].SDLPoint;
			}

			Color savedColor = this.Color;
			if (color != null)
				this.Color = color;
			int returnValue = SDL2.SDL.SDL_RenderDrawLines (this._renderer, pointArray, points.Count);
			this.Color = savedColor;
			if (returnValue == -1)
				throw new ApplicationException ("SDL Error");
		}
		/* 
		 * def draw_point(self, points, color=None):
		 *     """Draws one or multiple points on the renderer."""
		 *     # (x1, y1, x2, y2, ...)
		 *     pcount = len(points)
		 *     if (pcount % 2) != 0:
		 *         raise ValueError("points does not contain a valid set of points")
		 *     if pcount == 2:
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         ret = render.SDL_RenderDrawPoint(self.renderer, points[0],
		 *                                          points[1])
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 *     else:
		 *         x = 0
		 *         off = 0
		 *         SDL_Point = rect.SDL_Point
		 *         ptlist = (SDL_Point * pcount / 2)()
		 *         while x < pcount:
		 *             ptlist[off] = SDL_Point(points[x], points[x + 1])
		 *             x += 2
		 *             off += 1
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         ptr = cast(ptlist, POINTER(SDL_Point))
		 *         ret = render.SDL_RenderDrawPoints(self.renderer, ptr)
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 */
        /// <summary>
        /// Draws the points.
        /// </summary>
        /// <param name="points">Points.</param>
        /// <param name="color">Color.</param>
		public void DrawPoints(System.Collections.Generic.List<Point> points, Color color=null)
		{
			SDL2.SDL.SDL_Point[] pointArray = new SDL2.SDL.SDL_Point[points.Count];
			for (int iPoint = 0; iPoint < points.Count; iPoint++) {
				pointArray [iPoint] = points [iPoint].SDLPoint;
			}

			Color savedColor = this.Color;
			if (color != null)
				this.Color = color;
			int returnValue = SDL2.SDL.SDL_RenderDrawPoints (this._renderer, pointArray, points.Count);
			this.Color = savedColor;
			if (returnValue == -1)
				throw new ApplicationException ("SDL Error");
		}
		/* 
		 * def draw_rect(self, rects, color=None):
		 *     """Draws one or multiple rectangles on the renderer."""
		 *     SDL_Rect = rect.SDL_Rect
		 *     # ((x, y, w, h), ...)
		 *     if type(rects[0]) == int:
		 *         # single rect
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         x, y, w, h = rects
		 *         ret = render.SDL_RenderDrawRect(self.renderer, SDL_Rect(x, y, w, h))
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 *     else:
		 *         x = 0
		 *         rlist = (SDL_Rect * len(rects))()
		 *         for idx, r in enumerate(rects):
		 *             rlist[idx] = SDL_Rect(r[0], r[1], r[2], r[3])
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         ptr = cast(rlist, SDL_Rect)
		 *         ret = render.SDL_RenderDrawRects(self.renderer, ptr)
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 */
        /// <summary>
        /// Draws the rectangles.
        /// </summary>
        /// <param name="rectangles">Rectangles.</param>
        /// <param name="color">Color.</param>
		public void DrawRectangles(System.Collections.Generic.List<Rectangle> rectangles, Color color = null)
		{
			SDL2.SDL.SDL_Rect[] rectangleArray = new SDL2.SDL.SDL_Rect[rectangles.Count];
			for (int iRectangle = 0; iRectangle < rectangles.Count; iRectangle++)
				rectangleArray [iRectangle] = rectangles [iRectangle].SDLRectangle;

			Color savedColor = this.Color;
			if (color != null)
				this.Color = color;
			int returnValue = SDL2.SDL.SDL_RenderDrawRects (this._renderer, rectangleArray, rectangles.Count);
			this.Color = savedColor;
			if (returnValue == -1)
				throw new ApplicationException ("SDL Error");
		}
		/* 
		 * def fill(self, rects, color=None):
		 *     """Fills one or multiple rectangular areas on the renderer."""
		 *     SDL_Rect = rect.SDL_Rect
		 *     # ((x, y, w, h), ...)
		 *     if type(rects[0]) == int:
		 *         # single rect
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         x, y, w, h = rects
		 *         ret = render.SDL_RenderFillRect(self.renderer, SDL_Rect(x, y, w, h))
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 *     else:
		 *         x = 0
		 *         rlist = (SDL_Rect * len(rects))()
		 *         for idx, r in enumerate(rects):
		 *             rlist[idx] = SDL_Rect(r[0], r[1], r[2], r[3])
		 *         if color is not None:
		 *             tmp = self.color
		 *             self.color = color
		 *         ptr = cast(rlist, SDL_Rect)
		 *         ret = render.SDL_RenderFillRects(self.renderer, ptr)
		 *         if color is not None:
		 *             self.color = tmp
		 *         if ret == -1:
		 *             raise SDLError()
		 */
        /// <summary>
        /// Fills the rectangles.
        /// </summary>
        /// <param name="rectangles">Rectangles.</param>
        /// <param name="color">Color.</param>
		public void FillRectangles(System.Collections.Generic.List<Rectangle> rectangles, Color color = null)
		{
			SDL2.SDL.SDL_Rect[] rectangleArray = new SDL2.SDL.SDL_Rect[rectangles.Count];
			for (int iRectangle = 0; iRectangle < rectangles.Count; iRectangle++)
				rectangleArray [iRectangle] = rectangles [iRectangle].SDLRectangle;

			Color savedColor = this.Color;
			if (color != null)
				this.Color = color;
			int returnValue = SDL2.SDL.SDL_RenderFillRects (this._renderer, rectangleArray, rectangles.Count);
			this.Color = savedColor;
			if (returnValue == -1)
				throw new ApplicationException ("SDL Error");
		}


        // Additional methods so renderered window and surface window share common interface
        /// <summary>
        /// Blit the specified surface and position.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="position">Position.</param>
        public void Blit(Surface surface, Point position)
        {
            this.Blit (new Texture (surface, this), position);
        }
        /// <summary>
        /// Blit the specified texture and position.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        public void Blit (Texture texture, Point position)
        {
            Rectangle source_rectangle = new Rectangle (
                0,
                0,
                texture.Width,
                texture.Height);

            Rectangle destination_rectangle = new Rectangle (
                position.x,
                position.y,
                texture.Width,
                texture.Height);

            this.Copy (texture, source_rectangle, destination_rectangle);
        }
        /// <summary>
        /// Blit the specified drawingItem and position.
        /// </summary>
        /// <param name="drawingItem">Drawing item.</param>
        /// <param name="position">Position.</param>
        public void Blit (DrawingItem drawingItem, Point position)
        {
            this.Blit (drawingItem.GetTexture (this), position);
        }
	}
}

