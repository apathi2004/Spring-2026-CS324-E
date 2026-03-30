using Microsoft.Xna.Framework;

namespace assignment6
{
    /// <summary>
    /// Advaiith's class.
    /// One string segment between the frame anchor and the ball.
    ///
    /// Forces modelled here:
    ///   - String tension via Hooke's Law:  F = -k * (|delta| - restLength)
    ///     applied along the string direction so the link always pulls back
    ///     toward its rest length.
    ///   - The constraint solver (SatisfyConstraint) enforces the inextensible
    ///     limit — the string cannot stretch beyond RestLength.
    /// </summary>
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

        /// <summary>
        /// Hooke's Law tension force this link exerts on its parent.
        /// F = k * (extension) * direction_toward_child
        /// Positive when string is stretched (pulls parent toward child).
        /// </summary>
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

        /// <summary>
        /// Hard inextensibility constraint — nudges the link so it is never
        /// further than RestLength from <paramref name="parentPosition"/>.
        /// This enforces the normal (reaction) force of the string on the ball:
        /// the string can only pull, never push.
        /// </summary>
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