\page SDL2-CS-COP

This is SDL2-CS-COP, a Component-Oriented Programming library

Project Website: https://github.com/trsoluti/SDL2-CS-COP

License
=======

SDL2 and SDL2#, SDL2-CS-Bridge and SDL2-CS-COP are all released under the zlib license.
See the solution's LICENSE for details.

About SDL2-CS-COP
=================

SDL2-CS-COP is a port of the PySDL2 library to C#.
More information about the PySDL2 can be found at
http://pysdl2.readthedocs.org

This system is based on the same principles as the PySDL system,
namely the Component-Oriented Programming model
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

Differences from PySDL2
=======================

SDL2-CS-COP has a number of differences in API from PySDL2.
Most of these differences are due to C#'s typing requirements,
others are due to apparent weaknesses in the PySDL2's implementation of COP.

Keying Entities to Processes
----------------------------

In PySDL2, a Key is determined when the system is initialised.
A system identifies a list of component types it supports.
An Applicator requires *all* the component types to be present in the entity;
whereas a regular System requires *any* of the component types.

In SDL2-CS-COP, each system (called COP_System, as 'System' cannot properly be resolved due to namespace conflicts)
implements a method CanProcess(),
which looks at the given entity to determine whether or not it contains
the necessary component types.
This eliminates the need to differentiate Systems and Applicators
and is also open to more complex key mechanisms.

To assist in the keying process,
Entities provide the method Contains(<type>)
which will return True if the entity has at least one component
of the given type (or a component of a type derived from the given type).
This method relies on C# reflection and supports
the new SDL2-CS-COP concepts of Component Sets and Component Lists
(see below).

Accessing Components
--------------------

In PySDL2, the *name* of the component data element is used to determine
the actual component to process.
For example, if you have a component data type SpeedComponent,
your speed component field/property needs to be named "speedcomponent".
The World uses this relationship to determine which components to pass to the System.

In SDL2-CS-COP, World will pass the list of entities, not components.
(Again, this means there is no need to differentiate Systems and Applicators.)
The system uses supplied entity methods such as ComponentsOfType()
which rely on C# reflection methods to determine the actual component(s)
which match the requirements.

Alternatively, a system can call the Entity class method AllComponentsOfType(<entity list>)
to get a single list of all components in all entities,
which is what the PySDL2 World would have passed.

Finally, a system can use the square bracket method to get the first component of the given type.
(For example: PlayerDataComponent playerDataComponent = (PlayerDataComponent)(entity [typeof(PlayerDataComponent)];)))
This is most useful if the entity will never have more than one component of the type,
as it is easiest for casting.


Component Sets
--------------

A component set represents a group of components that are generally considered together,
but could be considered separately, if necessary.
An Entity is, in fact, a component set of all its public fields and properties.

A component set can be built out of:
 - components (as properties (recommended) or as fields)
 - other component sets (as properties (recommended) or as fields)
 - component lists of either components or component sets

Derived components can be accessed by their derived name or by any intermediate type name.
E.g. RectangleComponent, derived from PositionComponent will show up for
systems that ask for RectangleComponent and ones that ask for PositionComponent

Components in sub-component sets can also be accessed by type.
E.g. if you have a component set A which contains a component set B
and component set B contains the component VelocityComponent,
A.Contains(typeof(VelocityComponent)) will return True and
A.ComponentsOfType(typeof(VelocityComponent)) will return
the VelocityComponent field in component set B (assuming B has been initialized).

Some useful things you can do with component sets:
- have a list of enemy sprites in one Entity
- differentiate between an entity which has a related position and text and a different entity which has a position component and a separate text component

Component Lists
---------------

A component list represents a set of instances of the same type of component.
For example, a list of sprite components can be used by one entity to store all enemy sprites.

Note: If you are using a custom list, you need to either supply a property _Item_ of the correct type,
or a method _Get_ which returns the correct type, or your list will not be included.
System.Collections.Generic.List<Type>and Type[] are valid existing lists.

Standard Components and Systems
-------------------------------

SDL2-CS-COP provides a small set of standard components and systems to handle basic tasks.
The set includes:
- a \ref SDL2_CS_COP.StandardItems.Components.Position "Position" component, which provides movement and relational convenience methods
- an \ref SDL2_CS_COP.StandardItems.Components.Area "Area" component, derived from the Position component, which provides convenience methods for moving within bounds, and checking for collision
- a \ref SDL2_CS_COP.StandardItems.Components.Sprite "Sprite" component, which is derived from Area and is a wrapper for the SDL2-CS-Bridge.Sprite (along with some convenience methods)
- a \ref SDL2_CS_COP.StandardItems.Components.Velocity "Velocity" component, which can be used in a movement system
- a \ref SDL2_CS_COP.StandardItems.Systems.MovementSystem "MovementSystem," which handles simple (linear) movement using the Velocity component
- a \ref SDL2_CS_COP.StandardItems.Systems.SpriteRenderer "SpriteRenderer," which handles displaying all the sprites in order of depth


Hardware Abstraction
--------------------

The SDL2-CS-COP system makes a clear distinction between
the COP system itself and the underlying hardware/software
rendering. The World system encapsulates the bridge world,
and a Sprite component owns the (correct type of) bridge sprite.

Appropriate arguments can be passed to the SDL2-CS-Bridge layer
when creating a World, which can control or provide hints for
how the bridge layer inteprets the COP layer's requests.

(Future: probably will add an optional dictionary to World()
to contain all these hints, e.g. whether or not to initialise
the sound or the font subsystems)

Unimplemented Parts of PySDL2
-----------------------------

The current version only implements parts of PySDL2
needed to create the Pong game.
SDL2-CS-COP currently doesn't include font management,
sound management, resource management, or proper event management.

These will be added in the near future.

After that point, internals will change in order to
speed up the main cycle of operation, such as caching reflection information.

Organization of SDL2-CS-COP
===========================

The SDL2-CS-COP contains the following classes and protocols:
- \ref SDL2_CS_COP.World "World:" the world of systems and entities, derived from the Bridge. INSTANTIATE JUST ONE OF THESE.
- \ref SDL2_CS_COP.ICOP_System "ICOP_System:" any class that conforms to this interface can be used as a system in the world
- \ref SDL2_CS_COP.Entity "Entity:" the abstract class that is the basis of all user entities. Provides important internal methods used by COP Systems
- \ref SDL2_CS_COP.Component "Component:" an empty abstract class from which all components are derived (i.e. the type that represents *any* component)
- \ref SDL2_CS_COP.ComponentSet "ComponentSet:" the class the does the main reflective work for Entity. See above for more about Component Sets.
- \ref SDL2_CS_COP.ProcessCalls "ProcessCalls:" an internal class for managing the relationship between COP Systems and Entities
- \ref SDL2_CS_COP.Applicator "Applicator": just an abstract wrapper for the ICOP_System interface, as there is no operational difference

More information about each item can be found in the source code for the item.
