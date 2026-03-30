using Microsoft.Xna.Framework;

namespace assignment6
{
   
    public class ChainLink
    {
        public Vector2 Position;
        public float   RestLength { get; }

        // Hooke's Law spring stiffness (used by Chain to compute tension force)
        public const float SpringK = 800f;   // N/m  (pixel-unit scale)

        public ChainLink(Vector2 startPosition, float restLength)
        {
            Position  = startPosition;
            RestLength = restLength;
        }

       
        public Vector2 TensionForce(Vector2 parentPosition)
        {
            Vector2 delta     = Position - parentPosition;
            float   dist      = delta.Length();
            if (dist < 0.001f) return Vector2.Zero;

            float extension = dist - RestLength;          // zero when slack
            if (extension <= 0f) return Vector2.Zero;     // strings don't push

            Vector2 direction = delta / dist;             // unit vector parent→child
            return direction * SpringK * extension;       // F = k·x  (Hooke's Law)
        }

      
        public void SatisfyConstraint(Vector2 parentPosition)
        {
            Vector2 delta = Position - parentPosition;
            float   dist  = delta.Length();
            if (dist < 0.001f || dist <= RestLength) return;

            float correction = (dist - RestLength) / dist;
            Position -= delta * correction;               // normal force correction
        }
    }
}