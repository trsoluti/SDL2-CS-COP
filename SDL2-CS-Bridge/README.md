\page SDL2-CS-Bridge

This is SDL2-CS-Bridge, a hardware abstraction layer between SDL2-CS and SDL2-CS-COP

Project Website: https://github.com/x/SDL2-CS-COP

License
-------

SDL2 and SDL2#, SDL2-CS-Bridge and SDL2-CS-COP are all released under the zlib license.
See LICENSE for details.

About SDL2-CS-Bridge
====================

SDL2-CS-Bridge provides the interface between the SDL2-CS-COP system
and the actual SDL2 implementation of that system.

The intent of the bridge is twofold:
1) to keep hardware and SDL dependencies in SDL2-CS-COP to a minimum
2) to hide C-dependencies in the SDL2-CS code (such as freeing allocated structures)

The basic components of the bridge include:
- Windows, which are based on renderers,
- Drawing Items, which are based on either textures or surfaces,
- factories for sprites and windows
- assorted small classes and structures to wrap around SDL2 structures

Abstraction Concepts
--------------------

A window is based on a renderer.
There are two renderers implemented:
- \ref SDL2_CS_Bridge.Renderer "Renderer:" which provides an interface to the various SDL2 renderers
- \ref SDL2_CS_Bridge.Surface  "Surface:"  which provides simple blit support

Note that the Surface class provides support for both
windows and drawing items.
As a window, it supports *blit* and *fill*,
and as a drawing item, it supports *fill* (to make sprites from colors).

Any class that implements the IWindow interface
can be used as a window in the system.

A Drawing Item is the base class of all sprites and images.
It is a class, rather than an interface,
and manages either a Surface or a Texture
to represent its internal representation.

When creating a new Drawing Item,
if you provide a <c>null</c> renderer,
the system will create a surface-based drawing item
Otherwise, it will use the provided renderer
to create a texture-based drawing item.

When blitting the object, the window (which is a renderer)
uses the appropriate SDL2 method to get the object onto the screen.
Note that the SDL2 Renderer can support blitting surfaces,
but the Surface renderer cannot support textures.

The intent of this rather convoluted system is to make it
easy for the COP system to configure itself either one way (using surfaces)
or the other (using one of the SDL2 renderers)
and to hide that configuration from the COP system itself.

Simplest Method for Managing Rendering Type
===========================================

The Bridge system provides several methods for managing
the rendering type, depending on how flexible the system needs to be.

The simplest method is the preferred method, but it is the least flexible.

In the simplest method, the user's program passes a boolean value
*isUsingSDL2Renderer* to the World constructor
(which simply passes it to the Bridge constructor).
The world/bridge instance will retain this value.

The user's program then calls world.AddWindow() with all the appropriate arguments.
(If <c>isUsingSDL2Renderer</v> was set to False, the rendering flags will be ignored.)
The Bridge, based on the remembered boolean value,
will create the appropriate window, either a renderered or a surface window,
and initialize the Sprite class to create the appropriate sprites.

The user's program can then call the Sprite.NewSprite() factory method,
which will use the appropriate class variables (set by Bridge)
to create either a sprite based on a Surface
or a sprite based on a Texture using the provided renderer.

    SDL2_CS_COP.World world = new SDL2_CS_COP.World (renderingTypeArgument);
    SDL2_CS_Bridge.IWindow window = world.AddWindow(
        "The Pong Game", 
        SDL2.SDL.SDL_WINDOWPOS_CENTERED, 
        SDL2.SDL.SDL_WINDOWPOS_CENTERED,
        800, 600, SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN,
        SDL2.SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED); // render flags ignore if not using SDL2 Renderer
    SDL2_CS_Bridge.Sprite sp_paddle_sprite = Sprite.NewSprite (WHITE, new SDL2_CS_Bridge.Size(20, 200));
    SDL2_CS_Bridge.Sprite sp_ball_sprite = Sprite.NewSprite (WHITE, new SDL2_CS_Bridge.Size (20, 20));

Using SpriteFactory
===================

The SpriteFactory class was provided in PySDL2,
and is also available in SDL2-CS-Bridge.
The class allows the user to create an instance with certain configuration hints
and to generate the appropriate sprites from those hints.
It is more flexible and object-oriented than the Sprite classmethods,
as it doesn't manipulate "global variables" that affect future sprite generation.

In SDL2-CS-Bridge, SpriteFactory has the same interface
(note: not all PySDL2 methods are implemented yet),
and uses a C# Dictionary<String,Object> to store the configuration arguments.
Currently, only ["renderer"] is supported,
but ["fontmanager"] will be added in a later version.

Note that SpriteFactory calls the Sprite class methods to do the actual work,
setting the Sprite constants before and restoring them after.

Creating Windows and Sprites of Different Rendering Types
=========================================================

If it is necessary to create renderers and sprites of different types,
you can always create a copy of an existing window using the
RenderedWindow(IntPtr baseWindow, render_flags) constructor.
This has not been tested, and may run into memory problems
when the window is released.

Note that the standard COP Sprite Renderer processes all the sprites
in a particular order, from the furthest back to the furthest forward.
Applying different renderers during this operation may mess up
the way sprites are displayed.

Organization of SDL2-CS-Bridge
------------------------------

The SDL2-CS-Bridge contains the following classes, structures and protocols:
- \ref SDL2_CS_Bridge.SDL2_CS_Bridge "SDL2_CS_Bridge:" the bridge to SDL2. MAKE ONLY ONE OF THESE.
- \ref SDL2_CS_Bridge.IWindow        "IWindow:" any class that conforms to this protocol can be considered a window
- \ref SDL2_CS_Bridge.RenderedWindow "RenderedWindow:" an IWindow based on the SDL2 rendering system
- \ref SDL2_CS_Bridge.Renderer       "Renderer:" a bridge class to all the SDL2 rendering functions
- \ref SDL2_CS_Bridge.SurfaceWindow  "SurfaceWindow:" a window based on SDL2_Surface (i.e. old-style rendering)
- \ref SDL2_CS_Bridge.Surface        "Surface:" a bridge to the SDL2_Surface structure and its companion functions, plus some convenience methods
- \ref SDL2_CS_Bridge.VideoWindow    "VideoWindow:" (not sure what this is for, but PySDL2 has it) 
- \ref SDL2_CS_Bridge.DrawingItem    "DrawingItem:" base class of all drawn objects (sprites, images, etc)
- \ref SDL2_CS_Bridge.Sprite         "Sprite:" a set of class methods to create sprites, and a wrapper for the appropriate Drawing Item
- \ref SDL2_CS_Bridge.SpriteFactory  "SpriteFactory:" a set of methods to store sprite creation parameters and to make sprites from them
- \ref SDL2_CS_Bridge.Image          "Image:" a drawing item that contains an image
- \ref SDL2_CS_Bridge.Texture        "Texture:" a bridge to the SDL2_Texture structure with convenience methods needed by DrawingItem
- \ref SDL2_CS_Bridge.Event          "Event:" a bridge class to SDL2_Event structure with convenience methods
- \ref SDL2_CS_Bridge.EventQueue     "EventQueue:" a mechanism to get a list of events from SDL2
- \ref SDL2_CS_Bridge.PixelFormat    "PixelFormat:" a bridge class to extract info from an SDL2_PixelFormat structure (probably should be private to Surface)
- \ref SDL2_CS_Bridge.Rectangle      "Rectangle:" a bridge class to the SDL2_Rect structure with some convenience methods
- \ref SDL2_CS_Bridge.Color          "Color:" a bridge class to the SDL2_Color structure, with some convenience methods
- \ref SDL2_CS_Bridge.Point          "Point:" a bridge to the SDL2_Point structure with convenience methods. NOTE THIS IS A STRUCT (VALUE-PASSED), NOT A CLASS (REFERENCE-PASSED).
- \ref SDL2_CS_Bridge.Size           "Size:" a simple size structure to simplify argument lists. NOTE THIS IS A STRUCT (VALUE-PASSED), NOT A CLASS (REFERENCE-PASSED).

More information about each item can be found in the source code comments for the item.

