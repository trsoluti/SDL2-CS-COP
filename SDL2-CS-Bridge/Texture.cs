using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// A bridge to the SDL2_Texture structure with convenience methods needed by DrawingItem.
    /// </summary>
    public class Texture: IDrawingBase
    {
        /// <summary>
        /// The sdl texture.
        /// </summary>
        private System.IntPtr _sdlTexture;
        /// <summary>
        /// Whether to free on destroy.
        /// </summary>
        private Boolean _freeOnDestroy;
        /// <summary>
        /// Gets the SDL texture.
        /// </summary>
        /// <value>The SDL texture.</value>
        public System.IntPtr SDLTexture { get { return this._sdlTexture; } }
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width {
            get {
                TextureInfo textureInfo = new TextureInfo (this._sdlTexture);
                return textureInfo.width;
            }
        }
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height {
            get {
                TextureInfo textureInfo = new TextureInfo (this._sdlTexture);
                return textureInfo.height;
            }
        }
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public IntPtr Target { get { return this._sdlTexture; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Texture"/> class.
        /// </summary>
        /// <param name="sdlSurface">Sdl surface.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Texture (IntPtr sdlSurface, Renderer renderer, Boolean freeOnDestroy=true )
        {
            if (!sdlSurface.Equals (IntPtr.Zero)) {
                this._sdlTexture = IntPtr.Zero;
            } else {
                this._sdlTexture = SDL2.SDL.SDL_CreateTextureFromSurface (renderer.SDLRenderer, sdlSurface);
            }
            this._freeOnDestroy = freeOnDestroy;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Texture"/> class.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Texture (Surface surface, Renderer renderer, Boolean freeOnDestroy=true): this (surface.SDLSurface, renderer, freeOnDestroy)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Texture"/> class.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="size">Size.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="maskColor">Mask color.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public Texture (Color color, Size size, Renderer renderer, int depth=32, Color maskColor=null, Boolean freeOnDestroy=true)
        {
            Surface surface = new Surface (color, size, depth, maskColor, freeOnDestroy: true);
            this._sdlTexture = SDL2.SDL.SDL_CreateTextureFromSurface (renderer.SDLRenderer, surface.SDLSurface);
            this._freeOnDestroy = freeOnDestroy;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SDL2_CS_Bridge.Texture"/> is reclaimed by garbage collection.
        /// </summary>
        ~Texture ()
        {
            if (this._sdlTexture != IntPtr.Zero && this._freeOnDestroy) {
                SDL2.SDL.SDL_DestroyTexture(this._sdlTexture);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SDL2_CS_Bridge.Texture"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="SDL2_CS_Bridge.Texture"/>.</returns>
        public override string ToString ()
        {
            TextureInfo textureInfo = new TextureInfo (this._sdlTexture);
            return string.Format ("[Texture: format={0}, access={1}, size=({3}{4})]", 
                textureInfo.flags, textureInfo.access, textureInfo.width, textureInfo.height);
        }

        // mechanism to extract info:
        private struct TextureInfo {
            public UInt32 flags;
            public int access;
            public int width;
            public int height;
            public TextureInfo(IntPtr texture) {
                int returnValue = SDL2.SDL.SDL_QueryTexture(texture, out this.flags, out this.access, out this.width, out this.height);
                if (returnValue == -1)
                    throw new ApplicationException ("SDL Error");
            }
        }
    }
}

