using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace assignment5.Content;

public class Snake
{
    public Head Head;
    public BodySegment[] Segments;
    public Vector3 HeadPosition;
    public float TravelSpeed;
    public Vector2 TravelDirection;
    public float UndulateSpeed;
    public Color Color;

    private float _time = 0f;
    private float _amplitude = 1.5f;
    private float _frequency = 1.5f;

    public Snake(Model model, int numSegments, Color color)
    {
        Color = color;
        TravelSpeed = 2.0f;
        TravelDirection = new Vector2(0, -1); // moving forward along Z
        UndulateSpeed = 2.0f;

        Head = new Head(.02f, model, color);
        HeadPosition = Head.HeadPosition;

        float segmentScale = 0.03f * 0.85f; 
        float segmentSpacing = segmentScale * 24.24f * 0.1f;

        Segments = new BodySegment[numSegments];
        for (int i = 0; i < numSegments; i++)
        {
            Vector3 startPos = HeadPosition + new Vector3(0, 0, segmentSpacing * (i + 1));
            float phaseOffset = i * 0.6f;
            Segments[i] = new BodySegment(model, segmentScale, color, startPos, phaseOffset);
        }
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _time += dt;

        float pathLength = 25f;   
        float startZ = 10f;       

        // looping motion
        float z = startZ - (_time * TravelSpeed % pathLength);
        float x = _amplitude * (float)Math.Sin(_frequency * _time);

        HeadPosition = new Vector3(x, Head.ScaledRadius, z);

        
        for (int i = 0; i < Segments.Length; i++)
        {
            Vector3 target = i == 0 ? HeadPosition : Segments[i - 1].SegmentPosition;
            Segments[i].Update(gameTime, target);
        }
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
    {
        // Draw head
        Matrix savedWorld = basicEffect.World;
        basicEffect.World = Matrix.CreateTranslation(HeadPosition) * savedWorld;
        Head.Draw(graphicsDevice, basicEffect);
        basicEffect.World = savedWorld;

        
        // Draw each body segment
        foreach (var segment in Segments)
        {
            segment.Draw(graphicsDevice, basicEffect);
        }
    }
}