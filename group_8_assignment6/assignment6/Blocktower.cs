using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment6
{
    /// <summary>
    /// Harris's class.
    /// Manages the row of WreckingBall objects and their Chain strings.
    ///
    /// Each frame it:
    ///   1. Calls WreckingBall.Update() on every ball (gravity + air drag).
    ///   2. Calls Chain.Update() to compute string tension (Hooke's Law)
    ///      and enforce inextensibility (normal force).
    ///   3. Calls WreckingBall.ResolveCollision() on adjacent pairs that overlap
    ///      (momentum conservation + Newton's 3rd Law impulse).
    ///   4. Records the normal force magnitude in the corresponding Block.
    /// </summary>
    public class BlockTower
    {
        public  List<WreckingBall> Balls   { get; } = new();
        private List<Chain>        _chains           = new();
        public  List<Block>        Blocks  { get; }  = new();

        private readonly int _screenW;
        private readonly int _screenH;

        private const int   BallCount    = 5;
        private const float BallRadius   = 22f;
        private const float BallSpacing  = BallRadius * 2f + 2f;
        private const float StringLength = 180f;

        public Vector2 FrameTopLeft  { get; private set; }
        public Vector2 FrameTopRight { get; private set; }
        public float   FrameHeight   { get; private set; }

        public BlockTower(int screenW, int screenH)
        {
            _screenW = screenW;
            _screenH = screenH;
            Build();
        }

        private void Build()
        {
            Balls.Clear();
            _chains.Clear();
            Blocks.Clear();

            float totalWidth = BallSpacing * (BallCount - 1);
            float centreX    = _screenW * 0.5f;
            float anchorY    = _screenH * 0.22f;
            float leftX      = centreX - totalWidth * 0.5f - BallRadius * 2f;
            float rightX     = centreX + totalWidth * 0.5f + BallRadius * 2f;

            FrameHeight   = StringLength + BallRadius * 2f + 20f;
            FrameTopLeft  = new Vector2(leftX,  anchorY - 10f);
            FrameTopRight = new Vector2(rightX, anchorY - 10f);

            for (int i = 0; i < BallCount; i++)
            {
                float restX = centreX - totalWidth * 0.5f + i * BallSpacing;

                Balls.Add(new WreckingBall(restX, anchorY, StringLength, BallRadius, 1f));

                float spread = BallRadius * 0.6f;
                _chains.Add(new Chain(
                    new Vector2(restX - spread, anchorY),
                    new Vector2(restX + spread, anchorY),
                    StringLength));

                Blocks.Add(new Block(
                    new Vector2(restX - BallRadius, anchorY + StringLength - BallRadius),
                    BallRadius * 2f, BallRadius * 2f, 1f));
            }
        }

        public void Reset() => Build();

        /// <param name="dragIndex">Index of ball currently dragged (-1 = none).</param>
        public void Update(int dragIndex)
        {
            // --- 1. Pendulum physics (gravity + air drag) on every ball -------
            for (int i = 0; i < Balls.Count; i++)
                Balls[i].Update();

            // --- 2. String tension + inextensibility (Hooke + normal force) ---
            for (int i = 0; i < Balls.Count; i++)
                _chains[i].Update(Balls[i].Position);

            // --- 3. Ball–ball collision (momentum + Newton's 3rd Law) ---------
            for (int i = 0; i < Balls.Count - 1; i++)
            {
                if (i == dragIndex || i + 1 == dragIndex) continue;

                var a = Balls[i];
                var b = Balls[i + 1];

                float dist    = System.MathF.Abs(b.Position.X - a.Position.X);
                float minDist = a.Radius + b.Radius;

                if (dist < minDist)
                {
                    // Capture pre-collision ω to compute impulse magnitude for Block
                    float omegaA_pre = a.AngularVelocity;

                    a.ResolveCollision(b);

                    // Normal force ≈ impulse / dt  (dt = 1 frame)
                    float dOmega = System.MathF.Abs(a.AngularVelocity - omegaA_pre);
                    float J      = a.Mass * dOmega * a.StringLength;   // J = m·Δv
                    Blocks[i].LastNormalForce     = J;
                    Blocks[i + 1].LastNormalForce = J;   // equal & opposite
                }
                else
                {
                    Blocks[i].LastNormalForce     = 0f;
                    Blocks[i + 1].LastNormalForce = 0f;
                }
            }
        }

        public void Draw(SpriteBatch sb, Texture2D pixel)
        {
            foreach (var chain in _chains)
                chain.Draw(sb, pixel);

            foreach (var ball in Balls)
                ball.Draw(sb, pixel);
        }
    }
}