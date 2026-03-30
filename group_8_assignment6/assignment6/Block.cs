using Microsoft.Xna.Framework;

namespace assignment6
{
    /// <summary>
    /// Harris's class.
    /// In the Newton's Cradle, a Block represents the contact/collision zone
    /// anchored at each ball's rest position.
    ///
    /// It tracks the normal force magnitude that was last applied at this
    /// contact point during a ball–ball collision, so Pendulum can read it
    /// for debug display or future gameplay logic.
    ///
    /// The normal force at contact satisfies Newton's 3rd Law:
    ///   F_on_A = -F_on_B  (stored as LastNormalForce, always >= 0).
    /// </summary>
    public class Block
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public float Width  { get; }
        public float Height { get; }
        public float Mass   { get; }
        public bool  IsActive = true;

        /// <summary>
        /// Magnitude of the normal (contact) force applied at the last
        /// collision event at this ball's rest position.
        /// J / deltaTime  where deltaTime = 1 frame.
        /// </summary>
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