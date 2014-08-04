using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// Interface to windows.
    /// 
    /// Any class that conforms to this protocol can be considered a window.
    /// </summary>
    public interface IWindow
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
        int Width  { get; }
        /// <summary>
        /// Show this instance.
        /// </summary>
        void Show();
        /// <summary>
        /// Fill the specified areas with the specified color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="areas">Areas (fill the window if null).</param>
        void Fill( Color color, System.Collections.Generic.List<Rectangle> areas=null);
        /// <summary>
        /// Blit the specified surface to the window at the specified position.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="position">Position.</param>
        void Blit(Surface surface, Point position);
        /// <summary>
        /// Blit the specified texture to the window at the specified position.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        void Blit(Texture texture, Point position);
        /// <summary>
        /// Blit the specified drawingItem to the window at the specified position.
        /// </summary>
        /// <param name="drawingItem">Drawing item.</param>
        /// <param name="position">Position.</param>
        void Blit(DrawingItem drawingItem, Point position);
        /// <summary>
        /// Updates the window surface (i.e. displays the updated surface/texture/renderer).
        /// </summary>
        /// <returns>The window surface.</returns>
        int UpdateWindowSurface();
    }
}

