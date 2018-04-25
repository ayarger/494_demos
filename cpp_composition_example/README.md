# C++ Composition Example

## Why?

The Composition approach to code reuse and structuring is not limited to Unity and C# contexts. One may implement a basic Composition framework into existing C++ applications with fairly little effort, as demonstrated in this example. The primary steps are...

1. Establish a base "Component" class.
2. Establish a base "Entity" class that "holds" components somehow.
3. Derive from the Component class to create new custom components.

If you would like to create your own composition-based game engine and become independent of 3rd-parties like Unity and Epic Games, this example could serve as a decent starting point.

## Usage

Navigate to the cpp_composition_example directory and type "make". The example program may then be run via "./cpp_composition_example".
