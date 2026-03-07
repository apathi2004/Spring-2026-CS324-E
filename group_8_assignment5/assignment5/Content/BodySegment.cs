using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment5.Content;

public class BodySegment
{
    public Vector3 SegmentPosition;
    public Model Model;
    public float UndulateSpeed;
    public float Amplitude;
    
    // Will need to get each body segment to undulate up and down, can use sine to accomplish this but need
    // to incorporate lerp somewhere in the process
}