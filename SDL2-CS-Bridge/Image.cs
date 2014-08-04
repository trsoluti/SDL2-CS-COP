using System;
using SDL2;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// A drawing item that contains an image.
    /// </summary>
    public class Image: DrawingItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Image"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="renderer">Renderer.</param>
        public Image (String name, Renderer renderer=null): base(SDL2.SDL_image.IMG_Load (name), renderer:renderer, freeOnDestroy:true)
        {
        }
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SDL2_CS_Bridge.Image"/> is reclaimed by garbage collection.
        /// </summary>
        ~Image()
        {
        }
    }
}

