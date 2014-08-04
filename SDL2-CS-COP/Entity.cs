using System;

namespace SDL2_CS_COP
{

	/// <summary>
	/// Entity -- a simple application object
	/// 
	/// An entity is a specific object living in the application world.
	/// It does not carry data or application logic.
    /// </summary>
	/// 
    /// <remarks>
	/// An Entity consists of a set of Components,
	/// which represent the actual data of the object.
	/// 
	/// The combination of data objects and data object sets provides the key
	/// which a system uses to determine whether to act on the entity or not.
	/// 
	/// For example, a SpriteRenderer acts on any Entity object
	/// that has a SpriteComponent. It ignores all other Entity objects.
	/// 
	/// You can also design Systems to look for particular combinations of components.
	/// 
	/// The SDL2_CS_COP system has a sophisticated hierarchy of components
	/// using lists and component sets.
	/// 
	/// A list is a list (System.Collections.Generic<typeparam> recommended, but type[] accepted)
	/// of components all of the same type. For example, you can use this to keep a list of space-time holes.
	/// 
	/// A component set is a named set of components that are processed together.
	/// For example, you can create a MovingSprite component set with velocity as well as the sprite to display.
	/// 
	/// A System interacts with an Entity using two primary methods, and two convenience routines.
	/// 
	/// When adding a new Entity to the world, the COP will ask each system if it wishes to process the entity,
	/// using the method <see cref="SDL2_CS_COP.ICOP_System.CanProcess"/>. Your CanProcess should then call
	/// <see cref="SDL2_CS_COP.Entity.Contains"/> with the given type to see if the given entity supports
	/// the components you will be processing.
	/// 
	/// When the World runs your system's <see cref="SDL2_CS_COP.ICOP_System.Process"/> method 
	/// with the list of entities to process, 
	/// you can use <see cref="SDL2_CS_COP.Entity.ComponentsOfType"/> 
	/// to get the list of components of a given type for a particular entity.
	/// 
	/// You can also call the convience static method <see cref="SDL2_CS_COP.Entity.AllComponentsOfType"/>
	/// to get you a single list of components spanning all the entities passed to you.
	/// 
	/// Finally, if you know there will be only one component of the given type in your entity,
	/// you can use <see cref="SDL2_CS_COP.Entity.this"/>  square bracket method to get the first component
	/// of the given type. This method is easier for casting.
	/// 
	/// From a design point of view, you will find it easier to design component sets for
	/// any components that need to be processed together. This will give you the same functionality as
	/// Python SDL2 COP's Applicator. However, if you put a component in two different component sets,
	/// the methods will return references to BOTH components, even if they are the same object.</remarks>
	/// 
	/// </summary>
	public abstract class Entity: ComponentSet
    {
		/// <summary>
		/// Create a single list of all the components of the given type in all the given entities.
		/// </summary>
		/// <returns>A list of all the components of the given type.</returns>
		/// <param name="entities">A list of Entities to process.</param>
		/// <param name="componentType">Component type to search for.</param>
		public static System.Collections.Generic.List<Component> AllComponentsOfType(System.Collections.Generic.List<Entity>entities, Type componentType)
		{
			System.Collections.Generic.List<Component> componentList = new System.Collections.Generic.List<Component> (entities.Count);
			foreach (Entity entity in entities) {
				componentList.AddRange (entity.ComponentsOfType(componentType));
			}
			return componentList;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SDL2_CS_COP.Entity"/> class.
		/// </summary>
		/// <param name="world">World.</param>
		public Entity (World world)
        {
			world.AddEntity (this);
        }

		/// <summary>
		/// Get a list of all the components of the given type
		/// </summary>
		/// <returns>The list of all components of the given type.</returns>
		/// <param name="componentType">Component type.</param>
		public System.Collections.Generic.List<Component> ComponentsOfType(Type componentType)
		{
			// The work is done by the ComponentSet class
			return this.AllComponentsOfType (componentType);
		}


		/// <summary>
		/// Return the first component of the given type
		/// 
		/// Convenience method. May not return the component you expect
		/// if someone else starts adding fields to your entity.
		/// </summary>
		/// <param name="componentType">Component type.</param>
		public Component this [Type componentType] {
			get {
				return this.FirstComponentOfType (componentType);
			}
		}

		/// <summary>
		/// Whether or not this type contains the given component type
		/// 
		/// Will return true even if component type buried in list,
		/// in component set or in derived type
		/// </summary>
		/// <param name="componentType">Component type.</param>
		public bool Contains (Type componentType) {
			return this.HasComponentsOfType (componentType);
		}
    }
}

