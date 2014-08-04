using System;

namespace SDL2_CS_COP
{
    /// <summary>
    /// Interface to COP_System
    /// 
    /// All classes that adhere to this interface
    /// can be used as Systems in the COP framework
    /// </summary>
    /// 
    /// <remarks>
    /// A processing system within an application world
    /// operates on every entity object that meets its criteria
    /// (represented by a true result from CanProcess()).
    /// 
    /// The system operates on an abstract Entity object,
    /// but knows nothing about the contents of the entity
    /// other than those components for which it is configured.
    /// 
    /// A system has the following mechanisms to access components
    /// within an entity or within the world:
    ///
    ///   Entity.AllComponentsOfType(componentType) -- all such components in the world
    ///   entityInstance.ComponentsOfType(componentType) -- all such components in a specific entity
    ///   entityInstance[componentType] -- the first such component in a specific entity (useful if you know there is only one)
    /// 
    /// See <see cref="SDL2_CS_COP.Entity"/> for more information. 
    /// 
    /// In fact, in C#, this class is not needed, as there are no common methods.
    /// It is recommended you use the ICOP_System Interface instead.
    /// 
    /// </remarks>
    public interface ICOP_System
    {
        /// <summary>
        /// Process the specified world and entities.
        /// 
        /// This is called by the world on the list of appropriate entities.
        /// The world has already ensured the entities contain the required components.
        /// 
        /// Each component can be accessed from the entity using the [Type] construct,
        /// e.g. player[typeof(Position)] returns a reference to the Position component of the object player.
        /// </summary>
        /// <param name="world">World.</param>
        /// <param name="entities">Entities.</param>
        void Process (SDL2_CS_COP.World world, System.Collections.Generic.List<Entity> entities);
        /// <summary>
        /// Determines whether this instance can process the specified entity.
        /// </summary>
        /// <returns><c>true</c> if this instance can process the specified entity; otherwise, <c>false</c>.</returns>
        /// <param name="entity">The entity.</param>
        bool CanProcess (Entity entity);
    }
}

