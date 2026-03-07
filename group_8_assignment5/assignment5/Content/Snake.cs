using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using assignment5.Content;

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
    
    public Snake(int numSegments, Color color, Vector2 travelDirection, float travelSpeed, float undulateSpeed)
    {
        Segments = new BodySegment[numSegments];
        Head = new Head();
        Color = color;
        TravelSpeed = travelSpeed;
        TravelDirection = travelDirection;
        UndulateSpeed = undulateSpeed;
    }
}