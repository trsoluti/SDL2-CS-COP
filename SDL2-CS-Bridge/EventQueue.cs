using System;

namespace SDL2_CS_Bridge
{
    /// <summary>
    /// A mechanism to get a list of events from SDL2.
    /// </summary>
	public class EventQueue
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_Bridge.EventQueue"/> class.
        /// </summary>
        /// <remarks>
        /// Not used, as the only methods are class methods
        /// </remarks>
		public EventQueue ()
		{
		}
		/*
		 * def get_events():
		 *     """Gets all SDL events that are currently on the event queue."""
		 */
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns>The events.</returns>
		public static System.Collections.Generic.List<Event> GetEvents()
		{
			/*
			 * events.SDL_PumpEvents()
			 */
			SDL2.SDL.SDL_PumpEvents ();

			/*
			 * evlist = []
			 * SDL_PeepEvents = events.SDL_PeepEvents
			 */
			System.Collections.Generic.List<Event> eventList = new System.Collections.Generic.List<Event> ();

            //TODO: PeepEvents doesn't seem to work--generates weird event types
            // for events in array. (probably event struct size is invalid)
            // For now, just use poll for events
			/* 
			 * op = events.SDL_GETEVENT
			 * first = events.SDL_FIRSTEVENT
			 * last = events.SDL_LASTEVENT
			 */
			//SDL2.SDL.SDL_eventaction op = SDL2.SDL.SDL_eventaction.SDL_GETEVENT;
			//SDL2.SDL.SDL_EventType first = SDL2.SDL.SDL_EventType.SDL_FIRSTEVENT;
			//SDL2.SDL.SDL_EventType last = SDL2.SDL.SDL_EventType.SDL_LASTEVENT;

			/*
			 * while True:
			 *     evarray = (events.SDL_Event * 10)()
			 *     ptr = ctypes.cast(evarray, ctypes.POINTER(events.SDL_Event))
			 *     ret = SDL_PeepEvents(ptr, 10, op, first, last)
			 *     if ret <= 0:
			 *         break
			 *     evlist += list(evarray)[:ret]
			 *     if ret < 10:
			 *         break
			 * return evlist
			 */

			//Boolean moreEventsToGet = true;
			//while (moreEventsToGet) {
			//	SDL2.SDL.SDL_Event[] eventArray = new SDL2.SDL.SDL_Event[10];
			//	int nEvents = SDL2.SDL.SDL_PeepEvents (eventArray, 10, op, first, last);
			//	if (nEvents <= 0) {
			//		moreEventsToGet = false;
			//	} else {
			//		for (int iEvent = 0; iEvent < nEvents; iEvent++)
			//			eventList.Add (new Event (eventArray [iEvent]));
			//		moreEventsToGet = nEvents == 10;
			//	}
			//}

            SDL2.SDL.SDL_Event sdlEvent;
            while (SDL2.SDL.SDL_PollEvent(out sdlEvent) != 0) {
                eventList.Add (new Event(sdlEvent));
            }

			return eventList;
		}
	}
}

