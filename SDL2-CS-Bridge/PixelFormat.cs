using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// Bridge class to SDL2 Pixel Format.
    /// 
    /// Any convenience methods should be added here.
    /// </summary>
	public class PixelFormat
	{
        /// <summary>
        /// Gets a value indicating whether this pixel format has an alpha mask.
        /// </summary>
        /// <value><c>true</c> if this instance has an alpha mask; otherwise, <c>false</c>.</value>
		public bool HasAlphaMask { get; private set; }

        /// <summary>
        /// The SDL2 Pixel Format (stored as an IntPtr since no SDL2-CS method uses it otherwise)
        /// </summary>
		private IntPtr pixelFmtPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.PixelFormat"/> class.
        /// </summary>
        /// <param name="pixelFormatIntPtr">Int Ptr to Pixel format.</param>
		public PixelFormat (IntPtr pixelFormatIntPtr)
		{
			SDL2.SDL.SDL_PixelFormat pixelFormat = (SDL2.SDL.SDL_PixelFormat)System.Runtime.InteropServices.Marshal.PtrToStructure (pixelFormatIntPtr, typeof(SDL2.SDL.SDL_PixelFormat));
			this.HasAlphaMask = pixelFormat.Amask != 0;
			this.pixelFmtPtr = pixelFormatIntPtr;
		}

        /// <summary>
        /// Maps the color according the the instance's pixel format.
        /// </summary>
        /// <returns>The color.</returns>
        /// <param name="color">Color.</param>
		public uint MapColor(Color color)
		{
			if (this.HasAlphaMask)
				return SDL2.SDL.SDL_MapRGBA (this.pixelFmtPtr, color.Red, color.Green, color.Blue, color.Alpha);
			else
				return SDL2.SDL.SDL_MapRGB (this.pixelFmtPtr, color.Red, color.Green, color.Blue);
		}
	}
}

