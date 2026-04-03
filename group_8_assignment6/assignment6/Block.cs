using Microsoft.Xna.Framework;

namespace assignment6
{
  
    public class Block
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public float Width  { get; }
        public float Height { get; }
        public float Mass   { get; }
        public bool  IsActive = true;

       
        public float LastNormalForce { get; set; }

        public Block(Vector2 position, float width, float height, float mass = 1f)
        {
            Position = position;
            Width    = width;
            Height   = height;
            Mass     = mass;
        }
    }
}