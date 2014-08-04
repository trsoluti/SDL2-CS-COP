\page Test-SDL2-CS-COP

This is Test-SDL2-CS-COP, a test program for the SDL2-CS-COP library

License
-------

SDL2 and SDL2#, SDL2-CS-Bridge and SDL2-CS-COP are all released under the zlib license.
See the solution's LICENSE for details.

About Test-SDL2-CS-COP
----------------------

This program implements the PySDL Pong Game, which is described at http://pysdl2.readthedocs.org.

The original source code for the Python version can be found at https://bitbucket.org/marcusva/py-sdl2/.


Differences from PySDL's Version
--------------------------------

All SDL2 initialization is handled by SDL2-CS-Bridge constructor,
and the COP World is derived from the Bridge,
so we need to create the world before we do anything.

In SDL2-CS-Bridge, the window IS its renderer,
so the type of rendering has to be passed to the window,
rather than adding a renderer to a surface window.

(Why make the window the renderer?
So we can simply ask the window to draw us,
and not have to worry about the acceleration method used.)

SDL2-CS-COP offers several mechanisms for creating windows and sprites
that allow flexibility but hide implementation details:
- Create separate windows and sprite factories, as per PySDL2 (except no separate SpriteRenderer needed)
- Use the Sprite class methods and class variables as our sprite factory
- Pass the rendering type to World and let it manage everything

The third method is the easiest (but least flexible).
The Test-SDL2-CS-COP code shows all three methods (the second two in comments),
but uses the first as it is closest to the original Python code.

Events are passed through the world for preprocessing.
Currently this method is a no-op, but in the future,
events will be daisy-chained through all systems
who have a ProcessEvent method
(and, possibly, who satisify a simple key on event type).
Systems will be able to swallow events before they reach the main line or other processes.

Components:
- \ref TestSDL2CSCOP.MainClass : Main Class