using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// Sprite Class.
    /// 
    /// This represents a factory (through class methods)
    /// to produce sprites of the appropriate type
    /// and (through methods) a simple wrapper of <see cref="SDL2_CS_Bridge.DrawingItem"/> 
    /// </summary>
	public class Sprite: DrawingItem
    {
        /// <summary>
        /// Gets or sets the renderer.
        /// 
        /// The renderer is the object responsible for blitting
        /// the sprite onto the window. 
        /// </summary>
        /// <value>The renderer.</value>
		public static Renderer Renderer { get; set; }

        /// <summary>
        /// Gets or sets the type of the sprite to be created
        /// </summary>
        /// <value>The type of the sprite.</value>
        public static SpriteType SpriteType { get; set; }

        /// <summary>
        /// Create a new sprite from the specified SDL surface.
        /// </summary>
        /// <returns>The sprite.</returns>
        /// <param name="sdlSurface">Sdl surface.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public static Sprite NewSprite (System.IntPtr sdlSurface, Boolean freeOnDestroy = false)
        {
            Sprite.CheckRendererConsistency (); // causes an exception if inconsistent
            return new Sprite (sdlSurface, renderer:Sprite.Renderer, freeOnDestroy: freeOnDestroy);
        }
        /// <summary>
        /// Create a new sprite from the given Surface.
        /// </summary>
        /// <returns>The sprite.</returns>
        /// <param name="surface">Surface.</param>
        public static Sprite NewSprite (Surface surface)
        {
            Sprite.CheckRendererConsistency (); // causes an exception if inconsistent
            Boolean freeOnDestroy = Sprite.SpriteType == SpriteType.SOFTWARE ? false : true;
            return new Sprite (surface, renderer:Sprite.Renderer, freeOnDestroy: freeOnDestroy);
        }
        /// <summary>
        /// Create a new sprite from the given color and size information.
        /// </summary>
        /// <returns>The sprite.</returns>
        /// <param name="spriteColor">Sprite color.</param>
        /// <param name="size">Size.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="maskColor">Mask color.</param>
        public static Sprite NewSprite (Color spriteColor, Size size, int depth=32, Color maskColor=null)
        {
            Sprite.CheckRendererConsistency (); // causes an exception if inconsistent
            return new Sprite (spriteColor, size, Sprite.Renderer, depth, maskColor, freeOnDestroy: true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Sprite"/> class.
        /// </summary>
        /// <param name="surface">Surface.</param>
        public Sprite (Surface surface): base(surface)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Sprite"/> class.
        /// </summary>
        /// <param name="sdlSurface">Sdl surface.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Sprite (IntPtr sdlSurface, Renderer renderer = null, Boolean freeOnDestroy = true): base(sdlSurface, renderer:renderer, freeOnDestroy:freeOnDestroy)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Sprite"/> class.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Sprite (Surface surface, Renderer renderer = null, Boolean freeOnDestroy = true) : this (surface.SDLSurface, renderer, freeOnDestroy)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Sprite"/> class.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="size">Size.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="maskColor">Mask color.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Sprite (Color color, Size size, Renderer renderer = null, int depth = 32, Color maskColor = null, Boolean freeOnDestroy = true): base(color, size, renderer:renderer, depth:depth, maskColor:maskColor, freeOnDestroy:freeOnDestroy)
        {
        }

        /// <summary>
        /// Render the sprite in the specified window at the specified position.
        /// </summary>
        /// <param name="window">Window.</param>
        /// <param name="position">Position.</param>
        public void Render (IWindow window, Point position)
        {
            window.Blit (this, position);
        }
        /// <summary>
        /// Checks the renderer consistency.
        /// </summary>
        private static void CheckRendererConsistency()
        {
            if (Sprite.SpriteType == SpriteType.SOFTWARE) {
                Sprite.Renderer = null;
            } else if (Sprite.Renderer == null) {
                throw new InvalidOperationException ("Renderer MUST be set for non-surface rendering");
            }
        }
    }
}

