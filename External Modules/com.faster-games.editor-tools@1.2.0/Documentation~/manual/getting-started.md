# Getting Started

Some tips and tricks on how to begin using editor-tools.

## PropertyAttributes

- `EmbedObject` - supports embedding objects in the inspector, allowing children with multiple properties to appear inline directly.
- `ReadOnly` - makes a field read only in the inspector.

## Classes

- `TypeRef` - supports serializing a type reference into a unity object. Useful for selecting a pointer to a type in the editor, and then resolving that pointer at runtime.
- `TypeRef<T>` - same as above, but limits types that can be selected to those that derive from `T`.
