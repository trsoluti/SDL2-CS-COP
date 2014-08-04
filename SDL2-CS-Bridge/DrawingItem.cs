using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// Common Interface to either a Surface or a Texture
    /// </summary>
    public interface IDrawingBase
    {
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        int Height { get; }
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        int Width { get; }
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        IntPtr Target { get; }
    }

    /// <summary>
    /// Drawing item -- an item that can be drawn on a window.
    /// 
    /// This class forms the basis of all sprites and other drawing objects.
    /// </summary>
    public class DrawingItem
    {
        /// <summary>
        /// The drawing base.
        /// </summary>
        private IDrawingBase _drawingBase;
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get { return this._drawingBase.Height; } }
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get { return this._drawingBase.Width; } }

        /// <summary>
        /// Gets the surface if any.
        /// </summary>
        /// <returns>The surface.</returns>
        public Surface GetSurface() {
            if (this._drawingBase.GetType () == typeof(Surface)) {
                return (Surface)this._drawingBase;
            } else {
                return null;
            }
        }
        /// <summary>
        /// Gets or renders the texture.
        /// </summary>
        /// <returns>The texture.</returns>
        /// <param name="renderer">Renderer.</param>
        public Texture GetTexture(Renderer renderer) {
            if (this._drawingBase.GetType () == typeof(Texture)) {
                return (Texture)this._drawingBase;
            } else {
                Texture texture = new Texture ((Surface)this._drawingBase, renderer, freeOnDestroy: true);
                return texture;
            }
        }
        /// <summary>
        /// Gets the SDL surface if any.
        /// </summary>
        /// <value>The SDL surface.</value>
        public IntPtr SDLSurface {
            get {
                if (this._drawingBase.GetType() == typeof(Surface)) {
                    return this._drawingBase.Target;
                } else {
                    return IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// Gets the SDL texture if any.
        /// </summary>
        /// <value>The SDL texture.</value>
        public IntPtr SDLTexture {
            get {
                if (this._drawingBase.GetType() == typeof(Texture)) {
                    return this._drawingBase.Target;
                } else {
                    return IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.DrawingItem"/> class.
        /// </summary>
        /// <param name="surface">Surface.</param>
        public DrawingItem (Surface surface)
        {
            this._drawingBase = new Surface (surface);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.DrawingItem"/> class.
        /// </summary>
        /// <param name="sdlSurface">Sdl surface.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public DrawingItem (IntPtr sdlSurface, Renderer renderer = null, Boolean freeOnDestroy = true)
        {
            if (renderer == null) {
                this._drawingBase = new Surface (sdlSurface, freeOnDestroy);
            } else {
                this._drawingBase = new Texture (sdlSurface, renderer, freeOnDestroy);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.DrawingItem"/> class.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public DrawingItem (Surface surface, Renderer renderer = null, Boolean freeOnDestroy = true) : this (surface.SDLSurface, renderer, freeOnDestroy)
        {
            if (renderer == null) {
                this._drawingBase = new Surface (surface.SDLSurface, freeOnDestroy);
            } else {
                this._drawingBase = new Texture (surface, renderer, freeOnDestroy);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.DrawingItem"/> class.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="size">Size.</param>
        /// <param name="renderer">Renderer.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="maskColor">Mask color.</param>
        /// <param name="freeOnDestroy">If set to <c>true</c> free on destroy.</param>
        public DrawingItem (Color color, Size size, Renderer renderer = null, int depth = 32, Color maskColor = null, Boolean freeOnDestroy = true)
        {
            if (renderer == null) {
                this._drawingBase = new Surface (color, size, depth, maskColor, freeOnDestroy);
            } else {
                this._drawingBase = new Texture (color, size, renderer, depth, maskColor, freeOnDestroy);
            }
        }
    }
}

