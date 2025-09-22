# Unity Radial Layout Group

My simple custom implementation of a RadialLayoutGroup component in Unity.

## Features

- **Flexible Layout Control**: Supports both full circle (360°) and partial arc layouts
- **Directional Support**: Elements can be arranged clockwise or counterclockwise
- **Interactive Pointer**: Visual feedback with smooth pointer animation that follows mouse movement
- **Customizable Parameters**: 
  - Adjustable radius and start angle
  - Configurable rotation speed
  - Element spacing control
- **Easy Integration**: Drop-in replacement for Unity's built-in Layout Groups
- **Runtime Reinitialization**: Debug-friendly with context menu support for live updates

## Components

### RadialLayoutGroup
Core layout component that handles the positioning of child UI elements in a circular pattern.

### RadialMenuPointer
Visual pointer component that provides user feedback and smooth animation between menu sections.

### RadialMenu
Main controller that manages user input, pointer animation, and element interactions.

## Usage

1. Add the `RadialLayoutGroup` component to your UI parent object
2. Configure the layout parameters (radius, start angle, direction, etc.)
3. Add UI buttons as child elements - they will automatically arrange in a radial pattern
