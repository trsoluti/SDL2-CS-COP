using System;

namespace SDL2_CS_Bridge
{
	/*
	 * class SpriteFactory(object):
	 *     """A factory class for creating Sprite objects."""
	 */
    /// <summary>
    /// Sprite type
    /// </summary>
	public enum SpriteType {
        /// <summary>
        /// Textures rendered by SDL2 renderer
        /// </summary>
		TEXTURE = 0,
        /// <summary>
        /// Surfaces rendered by SDL_Blit
        /// </summary>
		SOFTWARE = 1
	}
    /// <summary>
    /// A set of methods to store sprite creation parameters and to make sprites from them.
    /// </summary>
	public class SpriteFactory
	{
		/* @property
		 * def sprite_type(self):
		 *     """The sprite type created by the factory."""
		 *     return self._spritetype
		 */
        /// <summary>
        /// Gets the type of the sprite.
        /// </summary>
        /// <value>The type of the sprite.</value>
		public SpriteType SpriteType { get; private set; }
        /// <summary>
        /// The default arguments.
        /// </summary>
		private System.Collections.Generic.Dictionary<String,Object> _defaultArguments;

		/* def __init__(self, sprite_type=TEXTURE, **kwargs):
		 *     """Creates a new SpriteFactory.
		 * 
		 *     The SpriteFactory can create TextureSprite or SoftwareSprite
		 *     instances, depending on the sprite_type being passed to it,
		 *     which can be SOFTWARE or TEXTURE. The additional kwargs are used
		 *     as default arguments for creating sprites within the factory
		 *     methods.
		 *     """
		 */
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.SpriteFactory"/> class.
        /// </summary>
        /// <param name="spriteType">Sprite type.</param>
        /// <param name="keywordArguments">Keyword arguments.</param>
		public SpriteFactory (SpriteType spriteType=SpriteType.TEXTURE, System.Collections.Generic.Dictionary<String,Object> keywordArguments=null )
		{
            // ensure keywordArugments is always a dictionary, even if not passed
            if (keywordArguments == null) {
                keywordArguments = new System.Collections.Generic.Dictionary<String,Object> (0);
            }
			/* if sprite_type == TEXTURE:
			 *     if "renderer" not in kwargs:
			 *         raise ValueError("you have to provide a renderer=<arg> argument")
			 */
			if (spriteType == SpriteType.TEXTURE) {
				if (!keywordArguments.ContainsKey ("renderer")) {
					throw new ApplicationException ("Texture sprites REQUIRE a renderer");
				} else {
					// Pass the renderer to the Sprite singleton so it can be used in the same way
					Sprite.Renderer = (Renderer)keywordArguments ["renderer"];
				}
			} else {
				Sprite.Renderer = null;
			}
            Sprite.SpriteType = spriteType;
			/* 
			 * elif sprite_type != SOFTWARE:
			 *     raise ValueError("stype must be TEXTURE or SOFTWARE")
			 * self._spritetype = sprite_type
			 * self.default_args = kwargs
			 */
			this.SpriteType = spriteType;
            this._defaultArguments = new System.Collections.Generic.Dictionary<string, object> (keywordArguments.Count);
            foreach (String key in keywordArguments.Keys) {
                this._defaultArguments [key] = keywordArguments [key];
            }
            if (spriteType == SpriteType.SOFTWARE) {
                this._defaultArguments ["renderer"] = null;
            }
		}
		/* def __repr__(self):
		 *     stype = "TEXTURE"
		 *     if self.sprite_type == SOFTWARE:
		 *         stype = "SOFTWARE"
		 *     return "SpriteFactory(sprite_type=%s, default_args=%s)" % 
		 *         (stype, self.default_args)
		 */
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SDL2_CS_Bridge.SpriteFactory"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="SDL2_CS_Bridge.SpriteFactory"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[SpriteFactory: SpriteType={0}, default args = {1}]", this.SpriteType, _defaultArguments);
		}
		/* def create_sprite_render_system(self, *args, **kwargs):
		 *     """Creates a new SpriteRenderSystem.
		 * 
		 *     For TEXTURE mode, the passed args and kwargs are ignored and the
		 *     Renderer or SDL_Renderer passed to the SpriteFactory is used.
		 *     """
		 *     if self.sprite_type == TEXTURE:
		 *         return TextureSpriteRenderSystem(self.default_args["renderer"])
		 *     else:
		 *         return SoftwareSpriteRenderSystem(*args, **kwargs)
		 */
		/* def from_image(self, fname):
		 *     """Creates a Sprite from the passed image file."""
		 *     return self.from_surface(load_image(fname), True)
		 */
        /// <summary>
        /// Creates a new sprite from a surface.
        /// </summary>
        /// <returns>The sprite from surface.</returns>
        /// <param name="surface">Surface.</param>
		public Sprite NewSpriteFromSurface(Surface surface)
		{
            Renderer oldRenderer = Sprite.Renderer;
            SpriteType oldSpriteType = Sprite.SpriteType;
            Sprite.Renderer = (Renderer)this._defaultArguments ["renderer"];
            Sprite.SpriteType = this.SpriteType;
            Sprite newSprite = Sprite.NewSprite (surface);
            Sprite.Renderer = oldRenderer;
            Sprite.SpriteType = oldSpriteType;
            return newSprite;
		}
		/* def from_object(self, obj):
		 *     """Creates a Sprite from an arbitrary object."""
		 *     if self.sprite_type == TEXTURE:
		 *         rw = rwops.rw_from_object(obj)
		 *         # TODO: support arbitrary objects.
		 *         imgsurface = surface.SDL_LoadBMP_RW(rw, True)
		 *         if not imgsurface:
		 *             raise SDLError()
		 *         s = self.from_surface(imgsurface.contents, True)
		 *     elif self.sprite_type == SOFTWARE:
		 *         rw = rwops.rw_from_object(obj)
		 *         imgsurface = surface.SDL_LoadBMP_RW(rw, True)
		 *         if not imgsurface:
		 *             raise SDLError()
		 *         s = SoftwareSprite(imgsurface.contents, True)
		 *     return s
		 */
		/* def from_color(self, color, size, bpp=32, masks=None):
		 *     """Creates a sprite with a certain color.
		 *     """
		 */
        /// <summary>
        /// Creates a new sprite from the given color and size information.
        /// </summary>
        /// <returns>The sprite from color.</returns>
        /// <param name="spriteColor">Sprite color.</param>
        /// <param name="size">Size.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="maskColor">Mask color.</param>
		public Sprite NewSpriteFromColor(Color spriteColor, Size size, int depth=32, Color maskColor=null)
		{
            Renderer oldRenderer = Sprite.Renderer;
            SpriteType oldSpriteType = Sprite.SpriteType;
            Sprite.Renderer = (Renderer)this._defaultArguments ["renderer"];
            Sprite.SpriteType = this.SpriteType;
            Sprite newSprite = Sprite.NewSprite (spriteColor, size, depth, maskColor);
            Sprite.Renderer = oldRenderer;
            Sprite.SpriteType = oldSpriteType;
            return newSprite;
		}
		/* def from_text(self, text, **kwargs):
		 *     """Creates a Sprite from a string of text."""
		 *     args = self.default_args.copy()
		 *     args.update(kwargs)
		 *     fontmanager = args['fontmanager']
		 *     surface = fontmanager.render(text, **args)
		 *     return self.from_surface(surface, free=True)
		 * 
		 * def create_sprite(self, **kwargs):
		 *     """Creates an empty Sprite.
		 * 
		 *     This will invoke create_software_sprite() or
		 *     create_texture_sprite() with the passed arguments and the set
		 *     default arguments.
		 *     """
		 *     args = self.default_args.copy()
		 *     args.update(kwargs)
		 *     if self.sprite_type == TEXTURE:
		 *         return self.create_texture_sprite(**args)
		 *     else:
		 *         return self.create_software_sprite(**args)
		 * 
		 * def create_software_sprite(self, size, bpp=32, masks=None):
		 *     """Creates a software sprite.
		 * 
		 *     A size tuple containing the width and height of the sprite and a
		 *     bpp value, indicating the bits per pixel to be used, need to be
		 *     provided.
		 *     """
		 *     if masks:
		 *         rmask, gmask, bmask, amask = masks
		 *     else:
		 *         rmask = gmask = bmask = amask = 0
		 *     imgsurface = surface.SDL_CreateRGBSurface(0, size[0], size[1], bpp,
		 *                                               rmask, gmask, bmask, amask)
		 *     if not imgsurface:
		 *         raise SDLError()
		 *     return SoftwareSprite(imgsurface.contents, True)
		 * 
		 * def create_texture_sprite(self, renderer, size,
		 *                           pformat=pixels.SDL_PIXELFORMAT_RGBA8888,
		 *                           access=render.SDL_TEXTUREACCESS_STATIC):
		 *     """Creates a texture sprite.
		 * 
		 *     A size tuple containing the width and height of the sprite needs
		 *     to be provided.
		 * 
		 *     TextureSprite objects are assumed to be static by default,
		 *     making it impossible to access their pixel buffer in favour for
		 *     faster copy operations. If you need to update the pixel data
		 *     frequently or want to use the texture as target for rendering
		 *     operations, access can be set to the relevant
		 *     SDL_TEXTUREACCESS_* flag.
		 *     """
		 *     if isinstance(renderer, render.SDL_Renderer):
		 *         sdlrenderer = renderer
		 *     elif isinstance(renderer, Renderer):
		 *         sdlrenderer = renderer.renderer
		 *     else:
		 *         raise TypeError("renderer must be a Renderer or SDL_Renderer")
		 *     texture = render.SDL_CreateTexture(sdlrenderer, pformat, access,
		 *                                        size[0], size[1])
		 *     if not texture:
		 *         raise SDLError()
		 *     return TextureSprite(texture.contents)
		 */

	}
}

