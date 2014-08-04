using System;
using SDL2;

/// \page Test-SDL2-CS
/// 
/// <summary>
/// Test-SDL2-CS is a test program to check that SDL2# (a.k.a. SDL2-CS)
/// has been installed and configured enough to be usable.
/// 
/// It is not a comprehensive test suite.
/// </summary>
/// 
/// <remarks>
/// NOTE: this program depends on the file "sax.png"
/// being in the same folder as the running program
/// (bin/Debug/ for debug). Copy the file from the
/// source folder to the running folder before starting the program.
/// 
/// Components:
/// <see cref="TestSDL2CS.Test_SDL2_CS"/>: main class
/// </remarks>

namespace TestSDL2CS
{
    /// <summary>
    /// Class to test SDL2-CS directly
    /// </summary>
	class Test_SDL2_CS
	{
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args)
		{
			SDL2.SDL.SDL_Event sdl_event;

			SDL2.SDL.SDL_Init(SDL2.SDL.SDL_INIT_VIDEO);

			/*****
			SDL2_Bridge.Window bWindow = new SDL2_Bridge.Window(SDLBridge, "Hello World",
				SDL2.SDL.SDL_WINDOWPOS_CENTERED, 
				SDL2.SDL.SDL_WINDOWPOS_CENTERED,
				400, 800, SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
			*****/
			IntPtr newWindow;
			newWindow = SDL2.SDL.SDL_CreateWindow ("Hello World",
				SDL2.SDL.SDL_WINDOWPOS_CENTERED, 
				SDL2.SDL.SDL_WINDOWPOS_CENTERED,
				400, 800, SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
			IntPtr windowSurface;
			windowSurface = SDL2.SDL.SDL_GetWindowSurface(newWindow);

			/*****
			bWindow.Show ();
			*****/
			SDL2.SDL.SDL_ShowWindow(newWindow);

			/*****
			SDL2_Bridge.Image bImage = new SDL2_Bridge.Image ("Sax.png");
			*****/
			IntPtr imageSurface = SDL2.SDL_image.IMG_Load ("Sax.png");

			/*****
			bWindow.Blit (bImage, new SDL2_Bridge.Point (20, 25));
			*****/
			SDL2.SDL.SDL_Surface managedSurface = (SDL2.SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure (imageSurface, typeof(SDL2.SDL.SDL_Surface));
			SDL2.SDL.SDL_Rect first_rect = new SDL2.SDL.SDL_Rect ();
			first_rect.x = 0;
			first_rect.y = 0;
			first_rect.w = managedSurface.w;
			first_rect.h = managedSurface.h;
			SDL2.SDL.SDL_Rect second_rect = new SDL2.SDL.SDL_Rect ();
			second_rect.x = 20;
			second_rect.y = 25;
			second_rect.w = managedSurface.w;
			second_rect.h = managedSurface.h;
			SDL2.SDL.SDL_BlitSurface(imageSurface, ref first_rect, windowSurface, ref second_rect);
			/*****
			bWindow.UpdateWindowSurface ();
			bImage = null; // free up the image when garbage collect comes around
			*****/
			SDL2.SDL.SDL_UpdateWindowSurface(newWindow);
			SDL2.SDL.SDL_FreeSurface (imageSurface);

			Boolean running = true;
			while (running) {
				while (SDL2.SDL.SDL_PollEvent(out sdl_event) != 0) {
					if (sdl_event.type == SDL2.SDL.SDL_EventType.SDL_QUIT) {
						running = false;
						break;
					}
				}
			}
		}
	}
}
