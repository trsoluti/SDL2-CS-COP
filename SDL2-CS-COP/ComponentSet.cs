using System;

namespace SDL2_CS_COP
{
	/// <summary>
	/// Component set
	/// 
	/// A component set is a group of components that need to be considered together
	/// For example, SpriteComponent and VelocityComponent could be requested
	/// as a component set SpriteAndVelocityComponentSet
    /// </summary>
    /// 
    /// <remarks>
	/// A component set can be built out of:
	///  - components (as properties (recommended) or as fields)
	///  - other component sets (as properties (recommended) or as fields)
	///  - Lists (i.e. System.Collections.IEnumerable) of either components or component sets
	/// 
	/// Derived components can be accessed by their derived name or by any intermediate type name.
	/// E.g. RectangleComponent, derived from PositionComponent will show up for
	/// systems that ask for RectangleComponent and ones that ask for PositionComponent
	/// 
	/// Components in sub-component sets can also be accessed by type.
	/// E.g. if you have a component set A which contains a component set B
	/// and component set B contains the component VelocityComponent,
	/// A.FirstComponentOfType(typeof(VelocityComponent)) will return
	/// the VelocityComponent in component set B (assuming B has been initialized).
	/// 
	/// Some useful things you can do with component sets:
	/// 
	///  - have a list of enemy sprites in one Entity
	///  - differentiate between an entity which has a related position and text
	///    and a different entity which has a position component and a separate text component
	/// 
	/// Note: If you are using a custom list, you need to either supply a property Item of the correct type,
	/// or a method Get which returns the correct type, or your list will not be included.
	/// System.Collections.Generic.List<Type>and Type[] are valid existing lists.
	/// 
	/// Note: the methods given here should not be called directly on components.
	/// To maintain proper COP design, always call the methods on objects of the <see cref="SDL2_CS_COP.Entity"/> class.
	/// 
	/// </remarks>
	public abstract class ComponentSet: Component
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SDL2_CS_COP.ComponentSet"/> class.
		/// </summary>
		public ComponentSet ()
		{
			// No-op at this point
		}

		/// <summary>
		/// Find all the components of a given type
		/// 
		/// Note this also returns components of the given type embedded in
		/// lists and component subsets.
		/// 
		/// </summary>
		/// <returns>The list of all components of the given type or of types derived from it.</returns>
		/// <param name="componentType">Component type.</param>
		protected System.Collections.Generic.List<Component> AllComponentsOfType(Type componentType)
		{
			return this.SomeComponentsOfType (componentType, Int32.MaxValue);
		}

		/// <summary>
		/// Find the first component of the given type
		/// 
		/// This is a convenience routine if you know by design there will be only
		/// one component of the given type in your entity.
		/// 
		/// </summary>
		/// <returns>The first component of the given type or of some type derived from it</returns>
		/// <param name="componentType">Component type.</param>
		protected Component FirstComponentOfType (Type componentType)
		{
			System.Collections.Generic.List<Component> componentList = this.SomeComponentsOfType (componentType, 1);
			return componentList.Count > 0 ? componentList[0] : null;
		}

		/// <summary>
		/// Determines whether this instance has components of the specified component type.
		/// </summary>
		/// <returns><c>true</c> if this instance has components of type the specified componentType; otherwise, <c>false</c>.</returns>
		/// <param name="componentType">Component type.</param>
		protected Boolean HasComponentsOfType (Type componentType)
		{
			return ComponentSet.TypeHasComponentsOfType (this.GetType (), componentType);
		}

		/// <summary>
		/// Determines if the given type has components of the specified component type
		/// 
        /// Note: this can be called even if the members have not yet been initialized.
		/// </summary>
		/// <returns><c>true</c>, if has components of type, <c>false</c> otherwise.</returns>
		/// <param name="componentSetType">Component set type.</param>
		/// <param name="componentType">Component type.</param>
		private static Boolean TypeHasComponentsOfType(Type componentSetType, Type componentType)
		{
			// Get a list of all the members for this type
			System.Reflection.MemberInfo[] members = componentSetType.GetMembers ();

			// For each member,
			for (int iMember = 0; iMember < members.Length; iMember++) {
				System.Reflection.MemberInfo memberInfo = members [iMember];

				// Look for matching properties
				if (memberInfo.MemberType == System.Reflection.MemberTypes.Property) {
					System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)memberInfo;
					// if the property type is the same as our property or is derived from it,
                    if (propertyInfo.PropertyType == componentType || propertyInfo.PropertyType.IsSubclassOf (componentType)) 
						return true;
					else if (propertyInfo.PropertyType.IsSubclassOf (typeof(ComponentSet))) {
						if (ComponentSet.TypeHasComponentsOfType (propertyInfo.PropertyType, componentType))
							return true;
					}
				}

				// Look for matching fields and or lists
				else if (memberInfo.MemberType == System.Reflection.MemberTypes.Field) {
					System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)memberInfo;
					Type fieldType = fieldInfo.FieldType;
                    if (fieldType == componentType || fieldType.IsSubclassOf(componentType)) {
						return true;
					} 
					// handle component sets recursively
					else if (fieldType.IsSubclassOf(typeof(ComponentSet))) {
						if (ComponentSet.TypeHasComponentsOfType (fieldType, componentType))
							return true;
					} 
					// if not component or component set, check to see if it's a valid list
					else {
						// find if the field supports "System.Collections.IEnumberable" interface
						Type[] interfaces = fieldType.FindInterfaces (delegate(Type m, object filterCriteria) {
							return m.ToString () == filterCriteria.ToString ();
						}, "System.Collections.IEnumerable");
						if (interfaces.Length > 0) {
							// Get the Item property, whose type will tell us the internal type
							Type listItemType = ComponentSet.GetListItemType (fieldType);
                            if (listItemType == componentType
                                || listItemType.IsSubclassOf (componentType))
								return true;
							if (listItemType.IsSubclassOf (typeof(ComponentSet))) {
								if (ComponentSet.TypeHasComponentsOfType (listItemType, componentType))
									return true;
							}
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the requested number of the components of specified type in this instance.
		/// 
		/// Normally called with either 1 (first) or Int32.MaxInt (all)
		/// </summary>
		/// <returns>The components of given type.</returns>
		/// <param name="componentType">Component type.</param>
		/// <param name="maxComponents">Max components.</param>
		private System.Collections.Generic.List<Component> SomeComponentsOfType(Type componentType, int maxComponents)
		{
			System.Collections.Generic.List<Component> componentList = new System.Collections.Generic.List<Component> ();

			// Get a list of all the members for this type
			System.Reflection.MemberInfo[] members = this.GetType ().GetMembers ();

			// For each member,
			for (int iMember = 0; iMember < members.Length && componentList.Count < maxComponents; iMember++) {
				System.Reflection.MemberInfo memberInfo = members [iMember];

				// Look for matching properties
				if (memberInfo.MemberType == System.Reflection.MemberTypes.Property) {
					System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)memberInfo;
					// if the property type is the same as our property or is derived from it,
                    if (propertyInfo.PropertyType == componentType || propertyInfo.PropertyType.IsSubclassOf (componentType)) {
						// Add the component if it exists
						System.Reflection.MethodInfo getMethod = propertyInfo.GetGetMethod ();
                        // if you support the [] notation, you get a property method that takes
                        // parameters. Don't want that to end up as a component in our list
                        System.Reflection.ParameterInfo[] parameters = getMethod.GetParameters ();
                        if (parameters.Length == 0) {
                            Component component = (Component)getMethod.Invoke (this, null);
                            if (component != null) {
                                componentList.Add (component);
                            }
                        }
					} else if (propertyInfo.PropertyType.IsSubclassOf (typeof(ComponentSet))) {
						System.Reflection.MethodInfo getMethod = propertyInfo.GetGetMethod ();
						ComponentSet componentSet = (ComponentSet)getMethod.Invoke (this, null);
						if (componentSet != null)
							componentList.AddRange (componentSet.SomeComponentsOfType (componentType, maxComponents));
					}
				}

				// Look for matching fields and or lists
				else if (memberInfo.MemberType == System.Reflection.MemberTypes.Field) {
					System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)memberInfo;
					Type fieldType = fieldInfo.FieldType;
                    if (fieldType == componentType || fieldType.IsSubclassOf (componentType)) {
						Component component = (Component)fieldInfo.GetValue (this);
						if (component != null)
							componentList.Add (component);
					} 
					// handle component sets recursively
					else if (fieldType.IsSubclassOf (typeof(ComponentSet))) {
						ComponentSet componentSet = (ComponentSet)fieldInfo.GetValue (this);
						if (componentSet != null)
							componentList.AddRange (componentSet.SomeComponentsOfType (componentType, maxComponents));
					} 
					// if not component or component set, check to see if it's a valid list
					else {
						// find if the field supports "System.Collections.IEnumberable" interface
						Type[] interfaces = fieldType.FindInterfaces (delegate(Type m, object filterCriteria) {
							return m.ToString () == filterCriteria.ToString ();
						}, "System.Collections.IEnumerable");
						if (interfaces.Length > 0) {
							// Get the Item property, whose type will tell us the internal type
							Type listItemType = ComponentSet.GetListItemType (fieldType);
                            if (listItemType == componentType
                                || listItemType.IsSubclassOf(componentType)
								|| listItemType.IsSubclassOf(typeof(ComponentSet))) {
								// Get the list field object itself
								Object listField = fieldInfo.GetValue (this);
								if (listField != null) {
									// Get the enumerator
									System.Reflection.MethodInfo enumeratorMethodInfo = fieldType.GetMethod ("GetEnumerator");
									System.Collections.IEnumerator enumerator = (System.Collections.IEnumerator)enumeratorMethodInfo.Invoke (listField, null);

									// Transfer each component
									while (enumerator.MoveNext ()) {
										if (listItemType.IsSubclassOf (typeof(ComponentSet)))
											componentList.AddRange (((ComponentSet)enumerator.Current).SomeComponentsOfType (componentType, maxComponents));
										else
											componentList.Add ((Component)enumerator.Current);
									}
								}
							}
						}
					}
				}
			}

			return componentList;
		}
		private static Type GetListItemType(Type iEnumerableType)
		{
			// try the "Item" property
			System.Reflection.PropertyInfo itemProperty = iEnumerableType.GetProperty ("Item");
			if (itemProperty != null) {
				return itemProperty.PropertyType;
			}

			// try the "Get" method
			System.Reflection.MethodInfo getMethod = iEnumerableType.GetMethod ("Get");
			if (getMethod != null) {
				return getMethod.ReturnType;
			}
			return null;
		}
	}
}

