using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// A bridge class to SDL2_Event structure with convenience methods.
    /// </summary>
	public class Event
	{
        /// <summary>
        /// The SDL event.
        /// </summary>
		private SDL2.SDL.SDL_Event _event;
        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
		public SDL2.SDL.SDL_EventType EventType { get { return this._event.type; } }
        /// <summary>
        /// Gets the key code of the event.
        /// </summary>
        /// <value>The key code.</value>
		public SDL2.SDL.SDL_Keycode KeyCode { get { return this._event.key.keysym.sym; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.Event"/> class.
        /// </summary>
        /// <param name="theEvent">The event.</param>
		public Event (SDL2.SDL.SDL_Event theEvent)
		{
			// Note: this works because theEvent is a ValueType.
			// if it were a class, we would need to create a new event and copy over the data
			this._event = theEvent;
		}
	}
}

