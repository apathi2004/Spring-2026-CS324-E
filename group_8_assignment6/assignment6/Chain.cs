using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment6
{
  
    ///
    /// Forces / laws modelled:
    ///   - Hooke's Law string tension (via ChainLink.TensionForce).
    ///   - Normal force: the inextensibility constraint stops the string
    ///     exceeding RestLength, which is the reaction force keeping the
    ///     ball on its circular arc.
    ///   - The net tension vector is exposed so WreckingBall can include it
    ///     in its force sum each frame.
    /// </summary>
    public class Chain
    {
        private readonly Vector2   _anchorLeft;
        private readonly Vector2   _anchorRight;
        private readonly ChainLink _linkLeft;
        private readonly ChainLink _linkRight;

        private const int SolverIterations = 12;

        public Vector2 TailPosition  { get; private set; }

        /// <summary>
        /// Combined Hooke's-Law tension force from both strings on the ball.
        /// Points from ball toward the anchor midpoint when string is taut.
        /// </summary>
        public Vector2 TensionForce  { get; private set; }

        public Chain(Vector2 anchorLeft, Vector2 anchorRight, float stringLength)
        {
            _anchorLeft  = anchorLeft;
            _anchorRight = anchorRight;

            float midX = (anchorLeft.X + anchorRight.X) * 0.5f;
            float midY =  anchorLeft.Y + stringLength;

            _linkLeft  = new ChainLink(new Vector2(midX, midY), stringLength);
            _linkRight = new ChainLink(new Vector2(midX, midY), stringLength);
            TailPosition = new Vector2(midX, midY);
        }

        /// <summary>
        /// Pins the tail to <paramref name="ballPosition"/>, computes Hooke's
        /// tension, then enforces inextensibility (normal force).
        /// </summary>
        public void Update(Vector2 ballPosition)
        {
            _linkLeft.Position  = ballPosition;
            _linkRight.Position = ballPosition;

            // Hooke's Law: tension from each string toward its anchor
            Vector2 tLeft  = -_linkLeft.TensionForce(_anchorLeft);    // force on ball
            Vector2 tRight = -_linkRight.TensionForce(_anchorRight);
            TensionForce   = tLeft + tRight;

            // Inextensibility (normal force) — iterative constraint
            for (int i = 0; i < SolverIterations; i++)
            {
                _linkLeft.SatisfyConstraint(_anchorLeft);
                _linkRight.SatisfyConstraint(_anchorRight);
                _linkLeft.Position  = ballPosition;
                _linkRight.Position = ballPosition;
            }

            TailPosition = (_linkLeft.Position + _linkRight.Position) * 0.5f;
        }

        public void Draw(SpriteBatch sb, Texture2D pixel)
        {
            DrawLine(sb, pixel, _anchorLeft,  TailPosition, new Color(180, 170, 140), 1);
            DrawLine(sb, pixel, _anchorRight, TailPosition, new Color(180, 170, 140), 1);
        }

        private static void DrawLine(SpriteBatch sb, Texture2D pixel,
            Vector2 from, Vector2 to, Color color, int thickness)
        {
            Vector2 edge  = to - from;
            float   angle = System.MathF.Atan2(edge.Y, edge.X);
            sb.Draw(pixel,
                new Rectangle((int)from.X, (int)from.Y, (int)edge.Length(), thickness),
                null, color, angle, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}