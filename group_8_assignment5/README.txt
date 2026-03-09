README – Assignment 5: 3D Animation

This project implements a simple animated 3D scene using MonoGame. The scene contains a snake composed of multiple spherical segments that move together to create a continuous animation. The animation loops indefinitely and demonstrates hierarchical object transformations and interpolated motion.

To run the program:

Open the solution file in Visual Studio.

1. Ensure MonoGame is installed and the project dependencies restore correctly.

2. Build the project.

3. Run the project using the "Start Debugging" or "Run" option in Visual Studio.

4. Once the program starts, a 3D scene will appear showing a snake moving across the ground plane. The animation runs automatically and loops indefinitely. No user input is required.

Project Structure
The project is implemented using several classes that manage different components of the animation:

Game1.cs
Handles the main MonoGame loop, rendering pipeline, camera setup, and scene updates.

Snake.cs
Manages the overall snake object and coordinates the positions of the head and body segments.

Head.cs
Represents the snake’s head and controls its movement and orientation within the scene.

BodySegment.cs
Represents each individual body segment of the snake. These segments follow the head with a slight delay to create a smooth motion effect.

Tongue.cs
Handles the animated tongue attached to the snake’s head.

Assets
The project uses sphere mesh models to represent the head and body segments. These models are loaded from the content pipeline.

The animation is implemented using transformations and interpolation to produce smooth motion throughout the scene.