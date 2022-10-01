using System;
using FasterGames.EditorTools.PropertyAttributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FasterGames.EditorTools
{
    /// <summary>
    /// A serializable reference to a given type
    /// </summary>
    /// <remarks>
    /// <c>null</c> if the type cannot be found at runtime  
    /// </remarks>
    [Serializable]
    public class TypeRef
    {
        public static class FieldNames
        {
            public const string TypeAssemblyQualifiedName = nameof(TypeRef.typeAssemblyQualifiedName);
        }
        
        /// <summary>
        /// The encapsulated type
        /// </summary>
        /// <remarks>
        /// <c>null</c> if the type cannot be found at runtime 
        /// </remarks>
        public Type Type => string.IsNullOrWhiteSpace(typeAssemblyQualifiedName) ? null : Type.GetType(typeAssemblyQualifiedName);

        /// <summary>
        /// The assembly qualified type name
        /// </summary>
        /// <remarks>
        /// This is populated by the property-drawer logic
        /// </remarks>
        [ReadOnly]
        [SerializeField]
        protected string typeAssemblyQualifiedName;
    }

    /// <summary>
    /// A serializable reference to a given type, that must be derived from <see cref="TBaseType"/>
    /// </summary>
    /// <remarks>
    /// <c>null</c> if the type cannot be found at runtime  
    /// </remarks>
    /// <typeparam name="TBaseType">the base type to filter by</typeparam>
    [Serializable]
    public class TypeRef<TBaseType> : TypeRef
    {
        public new static class FieldNames
        {
            public const string TypeAssemblyQualifiedName = nameof(TypeRef<Object>.typeAssemblyQualifiedName);
            public const string BaseTypeAssemblyQualifiedName = nameof(TypeRef<Object>.baseTypeAssemblyQualifiedName);
        }
        
        /// <summary>
        /// The base types assembly qualified name
        /// </summary>
        [ReadOnly]
        [SerializeField]
        protected string baseTypeAssemblyQualifiedName = typeof(TBaseType).AssemblyQualifiedName;
    }
}