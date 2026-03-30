using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment6
{
    /// <summary>
    /// Steven's class.
    /// One ball in the Newton's Cradle.
    ///
    /// Forces applied each frame (Newton's 2nd Law  F = ma):
    ///   1. Gravity:          Fg = m * g  (downward)
    ///   2. String tension:   Ft = Chain.TensionForce  (along string toward anchor)
    ///   3. Air resistance:   Fd = -b * v  (opposes velocity, linear drag)
    ///
    /// Collision (Newton's 3rd Law + conservation of momentum + energy):
    ///   When two balls touch, a 1-D impulse J is computed from the
    ///   coefficient of restitution (e = 0.98) and applied equal-and-opposite
    ///   to both balls, conserving momentum exactly and energy to within (1-e²).
    ///
    /// Angular approximation:
    ///   The pendulum ODE  α = -(g/L) sin(θ)  is integrated with a semi-
    ///   implicit Euler step, which preserves energy better than explicit Euler.
    ///   String tension provides the centripetal component automatically through
    ///   the constraint solver in Chain.
    /// </summary>
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

        // ── physics constants ─────────────────────────────────────────────────

        /// <summary>Gravitational acceleration in px/frame² (g ≈ 9.8 m/s², scaled).</summary>
        private const float G = 0.45f;

        /// <summary>
        /// Linear drag coefficient b in  Fd = -b·v.
        /// Models air resistance; small value so energy decays slowly.
        /// </summary>
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

        // ── update ────────────────────────────────────────────────────────────

        /// <summary>
        /// Integrates one physics tick using semi-implicit Euler.
        ///
        /// Net torque τ = -m·g·L·sin(θ)  →  α = τ/(m·L²) = -(g/L)·sin(θ)
        ///
        /// Air-resistance torque: τ_drag = -b·ω·L² (drag force × moment arm)
        /// reduces to  α_drag = -(b/m)·ω
        /// </summary>
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

        // ── collision ─────────────────────────────────────────────────────────

        /// <summary>
        /// Newton's 3rd Law + conservation of momentum + energy.
        ///
        /// 1-D impulse along the line of centres (X axis for cradle):
        ///
        ///   J = m_a·m_b·(1+e)·(v_a - v_b) / (m_a + m_b)
        ///
        /// Applied  +J/m_b  to ball b  and  -J/m_a  to ball a  (equal-and-opposite).
        ///
        /// Momentum check:  m_a·v_a + m_b·v_b  is identical before and after.
        /// Energy check:    KE loss = J²·(m_a+m_b)/(2·m_a·m_b) · (1-e²)  ≥ 0.
        /// </summary>
        public void ResolveCollision(WreckingBall other)
        {
            // Tangential (X) velocity at ball centre (v = ω · L · cos θ)
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

            // Convert linear Δv back to angular velocity  ω = v / (L·cos θ)
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