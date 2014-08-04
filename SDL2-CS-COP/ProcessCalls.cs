using System;

namespace SDL2_CS_COP
{
    /// <summary>
    /// Process call.
    /// </summary>
    /// <remarks>
    /// Manually derived from sealed class: System.Collections.DictionaryEntry
    /// </remarks>
	public struct ProcessCall
	{
        /// <summary>
        /// The key (a COP System).
        /// </summary>
		public ICOP_System Key;
        /// <summary>
        /// The value (a list of entities the system processes).
        /// </summary>
		public System.Collections.Generic.List<Entity> Value;
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2_CS_COP.ProcessCall"/> struct.
        /// </summary>
        /// <param name="theKey">A COP System.</param>
        /// <param name="theValue">A list of entities the system processes.</param>
		public ProcessCall (ICOP_System theKey, System.Collections.Generic.List<Entity> theValue)
		{
			this.Key = theKey;
			if (theValue == null)
				theValue = new System.Collections.Generic.List<Entity> ();
			this.Value = theValue;
		}
        /// <summary>
        /// Process the instance's key in the specified world.
        /// </summary>
        /// <param name="world">World.</param>
		public void Process (World world)
		{
			this.Key.Process (world, this.Value);
		}

		/// <summary>
		/// Adds the entity if can process.
		/// </summary>
		/// <returns><c>true</c>, if the entity was added, <c>false</c> otherwise.</returns>
		/// <param name="theEntity">The entity.</param>
		public bool AddEntityIfCanProcess (Entity theEntity)
		{
			// Adding twice is a no-op
			if (Value.Contains(theEntity))
				return true;

			bool canProcess = Key.CanProcess (theEntity);
			if (canProcess)
			{
				this.Value.Add (theEntity);
			}
			return canProcess;
		}
        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <param name="theEntity">The entity.</param>
		public void RemoveEntity(Entity theEntity)
		{
			this.Value.Remove (theEntity);
		}
        /// <summary>
        /// Determines whether the specified <see cref="SDL2_CS_COP.ICOP_System"/> is equal to the current <see cref="SDL2_CS_COP.ProcessCall"/>.
        /// </summary>
        /// <param name="obj">The <see cref="SDL2_CS_COP.ICOP_System"/> to compare with the current <see cref="SDL2_CS_COP.ProcessCall"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="SDL2_CS_COP.ICOP_System"/> is equal to the current
        /// <see cref="SDL2_CS_COP.ProcessCall"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (ICOP_System obj)
		{
			return base.Equals (obj);
		}
        /// <summary>
        /// Serves as a hash function for a <see cref="SDL2_CS_COP.ProcessCall"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
		public override int GetHashCode ()
		{
			return Key.GetHashCode ();
		}
        /// <summary>
        /// Converts the instance into a Dictionary Entry.
        /// </summary>
        /// <returns>The entry.</returns>
		public System.Collections.DictionaryEntry DictionaryEntry()
		{
			return new System.Collections.DictionaryEntry (this.Key, this.Value);
		}
	}

    /// <summary>
    /// Process calls.
    /// </summary>
	public class ProcessCalls: System.Collections.Specialized.IOrderedDictionary
	{
        /// <summary>
        /// The process calls.
        /// </summary>
		private System.Collections.Generic.List<ProcessCall> _processCalls;

		/// <summary>
		/// Initializes a new instance of the <see cref="SDL2_CS_COP.ProcessCalls"/> class.
		/// </summary>
		/// <param name="numItems">Number items.</param>
		public ProcessCalls (int numItems)
		{
			_processCalls = new System.Collections.Generic.List<ProcessCall> (numItems);
		}

		/// <summary>
		/// Process the entire list of process calls in insertion order
		/// </summary>
		/// <param name="world">World in which to process.</param>
		public void Process (World world)
		{
			foreach ( ProcessCall processCall in this._processCalls)
			{
				processCall.Process(world);
			}
		}

		/// <summary>
		/// Add a new entity to the process call list
		/// The method will automatically add the entity
		/// to the end of the list of entities
		/// for each system able to process the entity
		/// </summary>
		public void AddEntity(Entity theEntity)
		{
			foreach (ProcessCall processCall in this._processCalls) {
				processCall.AddEntityIfCanProcess (theEntity);
			}
		}
        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <param name="theEntity">The entity.</param>
		public void RemoveEntity(Entity theEntity)
		{
			foreach (ProcessCall processCall in this._processCalls) {
				processCall.RemoveEntity (theEntity);
			}
		}

		//----------------------------------------------------------------------------------------------------
		// METHODS REQUIRED TO IMPLEMENT IORDEREDDICTIONARY:

		/// <summary>
		/// Indexs the of key.
		/// </summary>
		/// <returns>The of key.</returns>
		/// <param name="key">Key.</param>
		public int IndexOfKey (ICOP_System key)
		{
			for (int i = 0; i < _processCalls.Count; i++) {
				if (((ProcessCall)_processCalls [i]).Key == key)
					return i;
			}

			// key not found, return -1. 
			return -1;
		}

		/// <summary>
		/// Gets or sets the <see cref="SDL2CSCOP.ProcessCalls"/> with the specified key.
		/// In this case, value will be a list of entities
		/// </summary>
		/// <param name="key">Key.</param>
		public System.Collections.Generic.List<Entity> this [ICOP_System key] {
			get {
				return ((ProcessCall)_processCalls [IndexOfKey (key)]).Value;
			}
			set {
				_processCalls [IndexOfKey (key)] = new ProcessCall (key, value);
			}
		}
		/// <summary>
		/// Gets or sets the <see cref="SDL2CSCOP.ProcessCalls"/> with the specified key.
		/// In this case, value will be a list of entities
		/// </summary>
		/// <param name="key">Key.</param>
		public object this [object key] {
			get {
				return (_processCalls [IndexOfKey ((ICOP_System)key)]).Value;
			}
			set {
				_processCalls [IndexOfKey ((ICOP_System)key)] = new ProcessCall ((ICOP_System)key, (System.Collections.Generic.List<Entity>)value);
			}
		}
		// IOrderedDictionary Members
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public System.Collections.IDictionaryEnumerator GetEnumerator ()
		{
			return new ProcessCallsEnum (_processCalls);
		}

		/// <summary>
		/// Insert the specified index, key and value.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="key">Key.</param>
        /// <param name="value">Value.</param> 
		public void Insert (int index, ICOP_System key, System.Collections.Generic.List<Entity> value)
		{
			if (IndexOfKey (key) != -1) {
				throw new ArgumentException ("An element with the same key already exists in the collection.");
			}
			_processCalls.Insert (index, new ProcessCall (key, value));
		}
        /// <summary>
        /// Insert the specified index, key and value.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
		public void Insert (int index, object key, object value)
		{
			this.Insert(index, (ICOP_System)key, (System.Collections.Generic.List<Entity>)value);
		}

		/// <summary>
		/// Removes at index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveAt (int index)
		{
			_processCalls.RemoveAt (index);
		}

		/// <summary>
		/// Gets or sets the <see cref="SDL2CSCOP.ProcessCalls"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public object this [int index] {
			get {
				return ((ProcessCall)_processCalls [index]).Value;
			}
			set {
				ICOP_System key = ((ProcessCall)_processCalls [index]).Key;

				_processCalls [index] = new ProcessCall (key, (System.Collections.Generic.List<Entity>)value);
			}
		}
		// IDictionary Members
		/// <summary>
		/// Add the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void Add (ICOP_System key, System.Collections.Generic.List<Entity> value)
		{
			if (IndexOfKey (key) != -1) {
				throw new ArgumentException ("An element with the same key already exists in the collection.");
			}
			_processCalls.Add (new ProcessCall (key, value));
		}
        /// <summary>
        /// Add the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
		public void Add (object key, object value)
		{
			if (IndexOfKey ((ICOP_System)key) != -1) {
				throw new ArgumentException ("An element with the same key already exists in the collection.");
			}
			_processCalls.Add (new ProcessCall ((ICOP_System)key, (System.Collections.Generic.List<Entity>)value));
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear ()
		{
			_processCalls.Clear ();
		}

		/// <summary>
		/// Contains the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public bool Contains (object key)
		{
			if (key.GetType () != typeof(ICOP_System)) {
				return false;
			} else if (IndexOfKey ((ICOP_System)key) == -1) {
				return false;
			} else {
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is fixed size.
		/// </summary>
		/// <value><c>true</c> if this instance is fixed size; otherwise, <c>false</c>.</value>
		public bool IsFixedSize {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets the keys.
		/// </summary>
		/// <value>The keys.</value>
		public System.Collections.ICollection Keys {
			get {
				System.Collections.Generic.List<ICOP_System> KeyCollection = new System.Collections.Generic.List<ICOP_System> (_processCalls.Count);
				for (int i = 0; i < _processCalls.Count; i++) {
					KeyCollection.Add (((ProcessCall)_processCalls [i]).Key);
				}
				return KeyCollection;
			}
		}

		/// <summary>
		/// Remove the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public void Remove (object key)
		{
			_processCalls.RemoveAt (IndexOfKey ((ICOP_System)key));
		}

		/// <summary>
		/// Gets the values.
		/// </summary>
		/// <value>The values.</value>
		public System.Collections.ICollection Values {
			get {
				System.Collections.Generic.List<System.Collections.Generic.List<Entity>> ValueCollection = new System.Collections.Generic.List<System.Collections.Generic.List<Entity>> (_processCalls.Count);
				for (int i = 0; i < _processCalls.Count; i++) {
					ValueCollection.Add (((ProcessCall)_processCalls [i]).Value);
				}
				return ValueCollection;
			}
		}
		// ICollection Members
		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="index">Index.</param>
		public void CopyTo (Array array, int index)
		{
			_processCalls.CopyTo ((ProcessCall[])array, index);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return _processCalls.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is synchronized.
		/// Note: according to MSDN, generic lists are not synchronized
		/// </summary>
		/// <value><c>true</c> if this instance is synchronized; otherwise, <c>false</c>.</value>
		public bool IsSynchronized {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets the sync root.
		/// Note: according to MSDN, the default implemenation always returns the current instance
		/// </summary>
		/// <value>The sync root.</value>
		public object SyncRoot {
			get {
				return _processCalls;
			}
		}
		// IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return new ProcessCallsEnum (_processCalls);
		}
	}

    /// <summary>
    /// Process calls enumerator.
    /// </summary>
	public class ProcessCallsEnum : System.Collections.IDictionaryEnumerator
	{
		/// <summary>
		/// The _people.
		/// </summary>
		public System.Collections.Generic.List<ProcessCall> _processCalls;
		// Enumerators are positioned before the first element
		// until the first MoveNext() call.
		int position = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="SDL2CSCOP.PeopleEnum"/> class.
		/// </summary>
		/// <param name="list">List.</param>
		public ProcessCallsEnum (System.Collections.Generic.List<ProcessCall> list)
		{
			_processCalls = list;
		}

		/// <summary>
		/// Moves the next.
		/// </summary>
		/// <returns><c>true</c>, if next was moved, <c>false</c> otherwise.</returns>
		public bool MoveNext ()
		{
			position++;
			return (position < _processCalls.Count);
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		public void Reset ()
		{
			position = -1;
		}

		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>The current.</value>
		public object Current {
			get {
				try {
					return _processCalls [position];
				} catch (IndexOutOfRangeException) {
					throw new InvalidOperationException ();
				}
			}
		}

		/// <summary>
		/// Gets the entry.
		/// </summary>
		/// <value>The entry.</value>
		public System.Collections.DictionaryEntry Entry {
			get {
				return ((ProcessCall)Current).DictionaryEntry();
			}
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		public object Key {
			get {
				try {
					return ((ProcessCall)_processCalls [position]).Key;
				} catch (IndexOutOfRangeException) {
					throw new InvalidOperationException ();
				}
			}
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public object Value {
			get {
				try {
					return ((ProcessCall)_processCalls [position]).Value;
				} catch (IndexOutOfRangeException) {
					throw new InvalidOperationException ();
				}
			}
		}
	}
}

