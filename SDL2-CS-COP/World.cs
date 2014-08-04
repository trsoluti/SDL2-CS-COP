using System;
using SDL2_CS_Bridge;

namespace SDL2_CS_COP
{
	/// <summary>
	/// World -- A simple application world.
	/// 
	/// An application world defines the combination of application data and
	/// processing logic and how the data will be processed. As such, it is
	/// a container object in which the application is defined.
	/// 
	/// The application world maintains a set of entities and their related
	/// components as well as a set of systems that process the data of the
	/// entities. Each processing system within the application world only
	/// operates on a certain set of components, but not all components of
	/// an entity at once.
	/// 
	/// The order in which data is processed depends on the order of the
	/// added systems.
	/// </summary>
	public class World:SDL2_CS_Bridge.SDL2_CS_Bridge
    {
		/// <summary>
		/// the linkage between processes and entities.
		/// </summary>
		private class ProcessCall
		{
			public ICOP_System System { get; set; }
			private System.Collections.Generic.List<Entity> Entities;
			public ProcessCall (ICOP_System theSystem)
			{
				this.System = theSystem;
				this.Entities = new global::System.Collections.Generic.List<Entity>();
			}
			public void AddEntity(Entity theEntity)
			{
				if (this.System.CanProcess(theEntity))
					this.Entities.Add (theEntity);
			}
			public void RemoveEntity(Entity theEntity)
			{
				this.Entities.Remove (theEntity);
			}
			public void Process(World world)
			{
				this.System.Process (world, this.Entities);
			}
			// Note this override allows us to compare based completely on value of System.
			public override bool Equals (object obj)
			{
				return this.System.Equals (obj);
			}
			public override int GetHashCode ()
			{
				return this.System.GetHashCode ();
			}
		}


		/// <summary>
		/// the linkage between processes and entities.
		/// </summary>
		private System.Collections.Generic.List<ProcessCall> _processCalls;

		/// <summary>
		/// A list of entities, in case we add a system after
		/// </summary>
		private System.Collections.Generic.List<Entity> _entities;

		/// <summary>
		/// Initializes a new instance of the <see cref="SDL2_CS_COP.World"/> class.
		/// </summary>
        public World (Boolean isUsingSDL2Renderer=false): base(isUsingSDL2Renderer)
        {
			this._processCalls = new System.Collections.Generic.List<ProcessCall>();
			this._entities = new System.Collections.Generic.List<Entity> ();
        }

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="SDL2_CS_COP.World"/> is
		/// reclaimed by garbage collection.
		/// </summary>
        ~World()
        {
        }

		/// <summary>
		/// Adds a system to the world at the end of the list of systems.
		/// </summary>
		/// <param name="theSystem">The system.</param>
		public void AddSystem(ICOP_System theSystem)
		{
			// can't add a system more than once
			// (note: since our Equals is based on System,
			// can do a Contains on a newly-created ProcessCall object)
			if (!this._processCalls.Contains (new ProcessCall(theSystem))) {
				this._processCalls.Add (new ProcessCall(theSystem));

				// add in any entities that were defined before the system
				this.AddExistingEntities (this._processCalls [this._processCalls.Count - 1]);
			}
		}

		/// <summary>
		/// Inserts the system at the specified location (0=first system).
		/// </summary>
		/// <param name="theSystem">The system.</param>
		/// <param name="index">Index.</param>
		public void InsertSystem(ICOP_System theSystem, int index)
		{
			if (!this._processCalls.Contains (new ProcessCall(theSystem))) {
				this._processCalls.Insert (index, new ProcessCall (theSystem));

				// add in any entities that were defined before the system
				this.AddExistingEntities (this._processCalls [index]);
			}
		}

		/// <summary>
		/// Deletes the system.
		/// </summary>
		/// <param name="theSystem">The system.</param>
		public void DeleteSystem(ICOP_System theSystem)
		{
			this._processCalls.Remove (new ProcessCall(theSystem));
		}

		/// <summary>
		/// Adds the entity.
		/// </summary>
		/// <param name="theEntity">The entity.</param>
		public void AddEntity(Entity theEntity)
		{
			this._entities.Add (theEntity);
			foreach (ProcessCall processCall in this._processCalls) 
				processCall.AddEntity (theEntity);
		}

		/// <summary>
		/// Deletes the entity.
		/// </summary>
		/// <param name="theEntity">The entity.</param>
		public void DeleteEntity(Entity theEntity)
		{
			foreach (ProcessCall processCall in this._processCalls) 
				processCall.RemoveEntity (theEntity);
			this._entities.Remove (theEntity);
		}

		/// <summary>
		/// Run the process cycle of each system, in order
		/// </summary>
		public void Process()
		{
			foreach (ProcessCall processCall in this._processCalls) 
				processCall.Process (this);
		}

        /// <summary>
        /// Gets events from the SDL2 system and processes them.
        /// </summary>
        /// <returns>The list of unswallowed events.</returns>
        public System.Collections.Generic.List<SDL2_CS_Bridge.Event> GetAndProcessEvents()
        {
            // in the future, we want a mechanism to ask each
            // process that has a ProcessEvent method
            // to process the event.
            // If the process 'swallows' the event,
            // it will return null and stop the daisy-chain.

            // for now, we just return the event list unprocessed
            return SDL2_CS_Bridge.EventQueue.GetEvents();
        }

		/// <summary>
		/// Adds the existing entities.
		/// 
		/// This is called any time we add a system,
		/// just in case we defined entities before we
		/// defined systems.
		/// </summary>
		/// <param name="theProcessCall">The process call.</param>
		private void AddExistingEntities(ProcessCall theProcessCall)
		{
			foreach (Entity entity in this._entities)
				theProcessCall.AddEntity (entity);
		}
    }
}

