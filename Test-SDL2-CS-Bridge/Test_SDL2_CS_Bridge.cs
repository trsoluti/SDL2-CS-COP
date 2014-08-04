using System;

/// \page Test-SDL2-CS-Bridge
/// 
/// <summary>
/// Test-SDL2-CS-Bridge is a test program to check
/// some of the features of <see cref="SDL2-CS-Bridge"/> .
/// 
/// It is not (yet) a comprehensive test suite.
/// </summary>
/// 
/// <remarks>
/// NOTE: this program depends on the file "sax.png"
/// being in the same folder as the running program
/// (bin/Debug/ for debug). Copy the file from the
/// source folder to the running folder before starting the program.
/// 
/// Components:
/// <see cref="TestSDL2CSBridge.MainClass"/>: main class 
/// </remarks>

namespace TestSDL2CSBridge
{
    /// <summary>
    /// Main class.
    /// </summary>
	class MainClass
	{
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args)
		{
			Boolean running;
			//SDL2.SDL.SDL_Event sdl_event;

			SDL2_CS_Bridge.SDL2_CS_Bridge SDLBridge = new SDL2_CS_Bridge.SDL2_CS_Bridge ();

			SDL2_CS_Bridge.SurfaceWindow bWindow = new SDL2_CS_Bridge.SurfaceWindow(SDLBridge, "Hello World",
				SDL2.SDL.SDL_WINDOWPOS_CENTERED, 
				SDL2.SDL.SDL_WINDOWPOS_CENTERED,
				780, 800, 0);

			bWindow.Show ();

			SDL2_CS_Bridge.Image bImage = new SDL2_CS_Bridge.Image ("Sax.png");

			SDL2_CS_Bridge.Color WhiteColor = new SDL2_CS_Bridge.Color (255, 255, 255);
            SDL2_CS_Bridge.Sprite.SpriteType = SDL2_CS_Bridge.SpriteType.SOFTWARE;
			SDL2_CS_Bridge.Sprite sprite = SDL2_CS_Bridge.Sprite.NewSprite (WhiteColor, new SDL2_CS_Bridge.Size (20, 20));

			bWindow.Blit (bImage, new SDL2_CS_Bridge.Point (20, 25));
			bWindow.Blit (bImage, new SDL2_CS_Bridge.Point (400, 25));
            sprite.Render(bWindow, new SDL2_CS_Bridge.Point (380, 380));
			bWindow.UpdateWindowSurface ();
			bImage = null; // free up the image when garbage collect comes around

			running = true;
            //SDL2.SDL.SDL_Event sdlEvent;
			while (running) {

                foreach (SDL2_CS_Bridge.Event sdlEvent in SDL2_CS_Bridge.EventQueue.GetEvents()) {
                    /*
			         * if event.type == sdlevents.SDL_QUIT:
			         *     running = False
			         *     break
			         */
                    System.Console.WriteLine ("Processing Event type {0:x}, expecting {1}", sdlEvent.EventType, SDL2.SDL.SDL_EventType.SDL_QUIT);
                    if (sdlEvent.EventType == SDL2.SDL.SDL_EventType.SDL_QUIT) {
                        running = false;
                        break;
                    }
                }
			}
		}
	}
}
