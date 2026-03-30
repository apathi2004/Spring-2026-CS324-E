using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment6
{


    public class WreckingBall
    {
        // ── pendulum state ────────────────────────────────────────────────────
        public float Angle;
        public float AngularVelocity;

        // ── derived linear state ──────────────────────────────────────────────
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        // ── properties ────────────────────────────────────────────────────────
        public float Mass         { get; }
        public float Radius       { get; }
        public float StringLength { get; }
        public float RestX        { get; }
        public float AnchorY      { get; }
        public bool  IsDragged    { get; set; }

       
        private const float G = 0.45f;

    
        private const float AirDragB = 0.0018f;

        /// <summary>Coefficient of restitution for ball–ball collisions (nearly elastic).</summary>
        public const float Restitution = 0.98f;

        // ── constructor ───────────────────────────────────────────────────────
        public WreckingBall(float restX, float anchorY, float stringLength,
                            float radius = 22f, float mass = 1f)
        {
            RestX        = restX;
            AnchorY      = anchorY;
            StringLength = stringLength;
            Radius       = radius;
            Mass         = mass;
            UpdatePosition();
        }

     
        public void Update()
        {
            if (IsDragged)
            {
                UpdatePosition();
                return;
            }

            // --- gravity term: α_g = -(g/L)·sin(θ) ---------------------------
            float alphGravity = -(G / StringLength) * MathF.Sin(Angle);

            // --- air-resistance term: α_drag = -(b/m)·ω ----------------------
            float alphaDrag = -(AirDragB / Mass) * AngularVelocity;

            // --- semi-implicit Euler: update ω first, then θ -----------------
            AngularVelocity += alphGravity + alphaDrag;
            Angle           += AngularVelocity;

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Vector2 prev = Position;
            Position = new Vector2(
                RestX   + MathF.Sin(Angle) * StringLength,
                AnchorY + MathF.Cos(Angle) * StringLength);
            Velocity = Position - prev;
        }

        /// <summary>Converts a dragged world-X into angle; zeroes angular velocity.</summary>
        public void SetAngleFromX(float worldX)
        {
            float dx = MathHelper.Clamp(worldX - RestX,
                                        -StringLength * 0.98f,
                                         StringLength * 0.98f);
            Angle           = MathF.Asin(dx / StringLength);
            AngularVelocity = 0f;
            UpdatePosition();
        }


        public void ResolveCollision(WreckingBall other)
        {
          
            float va = AngularVelocity       * StringLength       * MathF.Cos(Angle);
            float vb = other.AngularVelocity * other.StringLength * MathF.Cos(other.Angle);

            if (va <= vb) return;   // balls already separating — no impulse needed

            float ma = Mass, mb = other.Mass;

            // Impulse magnitude  J = m_a·m_b·(1+e)·Δv / (m_a+m_b)
            float deltaV = va - vb;
            float J      = ma * mb * (1f + Restitution) * deltaV / (ma + mb);

            // Post-collision velocities (Newton's 3rd: same J, opposite sign)
            float newVa = va - J / ma;   // ball a loses momentum
            float newVb = vb + J / mb;   // ball b gains momentum  (equal & opposite)

      
            float cosA = MathF.Cos(Angle);
            float cosB = MathF.Cos(other.Angle);
            if (MathF.Abs(cosA) > 0.01f)
                AngularVelocity       = newVa / (StringLength       * cosA);
            if (MathF.Abs(cosB) > 0.01f)
                other.AngularVelocity = newVb / (other.StringLength * cosB);

            // Positional correction — push balls apart (normal force resolution)
            float overlap = (Radius + other.Radius) - MathF.Abs(other.Position.X - Position.X);
            if (overlap > 0f)
            {
                float halfArc = MathF.Asin(MathHelper.Clamp(overlap * 0.5f / StringLength, -1f, 1f));
                Angle       -= halfArc;
                other.Angle += halfArc;
                UpdatePosition();
                other.UpdatePosition();
            }
        }

        // ── draw ─────────────────────────────────────────────────────────────

        public void Draw(SpriteBatch sb, Texture2D pixel)
        {
            int r = (int)Radius;
            DrawFilledCircle(sb, pixel, Position + new Vector2(4, 4), r,
                             new Color(0, 0, 0, 55));                           // shadow
            DrawFilledCircle(sb, pixel, Position, r,
                             new Color(80, 85, 90));                            // base sphere
            DrawFilledCircle(sb, pixel, Position + new Vector2(-r * 0.10f, -r * 0.05f),
                             (int)(r * 0.65f), new Color(110, 115, 122));       // mid highlight
            DrawFilledCircle(sb, pixel, Position + new Vector2(-r * 0.28f, -r * 0.30f),
                             (int)(r * 0.28f), new Color(210, 215, 220));       // specular
            DrawFilledCircle(sb, pixel, Position + new Vector2(-r * 0.38f, -r * 0.42f),
                             (int)(r * 0.10f), Color.White);                    // glint
        }

        private static void DrawFilledCircle(SpriteBatch sb, Texture2D pixel,
            Vector2 center, int radius, Color color)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int hw = (int)MathF.Sqrt(MathF.Max(0f, radius * radius - dy * dy));
                sb.Draw(pixel,
                    new Rectangle((int)center.X - hw, (int)center.Y + dy, hw * 2, 1),
                    color);
            }
        }
    }
}