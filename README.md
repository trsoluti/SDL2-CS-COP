\mainpage SDL2-CS-COP Main Page

This is SDL2-CS-COP, a Component-Oriented Programming model built on SDL2

Project Website: https://github.com/trsoluti/SDL2-CS-COP

License
-------

SDL2 and SDL2#, SDL2-CS-Bridge and SDL2-CS-COP are all released under the zlib license.
See LICENSE for details.

About SDL2-CS-COP
-----------------

SDL2-CS-COP is a port of the PySDL system to C#.
More information about the PySDL system can be found at
http://pysdl2.readthedocs.org

This system is based on the same principles as the PySDL system,
namely the Component Object Programming model
(also called Entity-Based System).
You can find out details about how COP works
and how to write programs with COP
with the PySDL documentation.

Essentially, A COP system has three major components:
- a set of Components, which contain data
- a set of Systems which operate on Entities
- a set of Keys which identify which systems operate on which entities

The COP system runs through the list of systems,
and each system runs through the list of entities it supports,
performing the same code on each.

For example, you can have one system that manages motion,
one system that manages collisions,
and one that manages display.

Differences from PySDL
----------------------

Differences between SDL2-CS-COP's API and PySDL's API
are discussed in detail in the README for the SDL2-CS-COP library.
They boil down to the following:
- SDL2-CS-COP's hardware dependencies are hidden in the Bridge library
- SDL2-CS-COP adds the concepts of "Component Set" and "Component List"
- In SDL2-CS-COP, the System is responsible for supplying the Key (through the CanProcess() method)

Organization of SDL2-CS-COP
---------------------------

The SDL2-CS-COP contains the following libraries and code samples:
- \ref SDL2-CS-Bridge : the interface to SDL2#, hiding hardware and rendering-method dependencies
- \ref SDL2-CS-COP : the actual COP library
- \ref Test-SDL2-CS : Some test code to ensure SDL2# is properly installed and connected
- \ref Test-SDL2-CS-Bridge : Some test code to check out features of the SDL2-CS-Bridge library
- \ref Test-SDL2-CS-COP : An implementation of the Pong Game from PySDL2

More information about each item can be found in the README.md for the item.

Installation
------------

First off, you need to download and install the SDK binaries for your platform:
- SDL2
- SDL2_image
- SDL2_mixer
- SDL2_ttf

Then you need to download and install SDL2# (a.k.a.SDL2-CS).
This can be found at https://github.com/flibitijibibo/SDL2-CS .

You need to configure the SDL2-CS.dll.config so it can load your SDL2 libraries.

You then need to update the References for the projects in the SDL2-CS-COP solution
to include your SDL2-CS library.

Note the SDL2-CS-COP solution was built using Xamarin Studio 5.
You may need to do some minor adjustment to get it to build in
your flavour of Visual Studio.

Now try out the various test programs.

Documentation
-------------

The PySDL documentation (http://pysdl2.readthedocs.org) is essential for understanding how to program using a COP system.

The source code is heavily documented with C# XML comments.
You can extract these comments and build your own documentation using the supplied doxygen file.
