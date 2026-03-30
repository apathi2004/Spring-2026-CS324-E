using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment6
{
    /// <summary>
    /// Top-level orchestrator.
    /// Owns the BlockTower, handles mouse drag, and draws the frame + HUD.
    ///
    /// Physics summary of the full system each frame:
    ///
    ///   Forces on each ball:
    ///     Fg  = m·g            (gravity, downward)
    ///     Ft  = k·(|r|-L)·r̂   (Hooke's Law string tension, toward anchor)
    ///     Fd  = -b·v           (air resistance, opposes velocity)
    ///     Fn  = impulse/dt     (normal contact force, Newton's 3rd Law)
    ///
    ///   Collision:
    ///     J = m_a·m_b·(1+e)·Δv / (m_a+m_b)    e = 0.98  (nearly elastic)
    ///     Δp_a = -J,  Δp_b = +J                (conservation of momentum)
    ///     ΔKE   = J²·(m_a+m_b)/(2·m_a·m_b) · (1-e²)  ≥ 0 (energy dissipated)
    ///
    ///   Controls: click+drag any ball to pull back, release to swing.
    ///             Press R to reset.
    /// </summary>
    public class Pendulum
    {
        private readonly BlockTower _tower;
        private readonly Texture2D  _pixel;
        private readonly int        _screenW;
        private readonly int        _screenH;

        private int        _dragIndex = -1;
        private MouseState _prevMouse;

        public Pendulum(int screenW, int screenH, Texture2D pixel)
        {
            _screenW = screenW;
            _screenH = screenH;
            _pixel   = pixel;
            _tower   = new BlockTower(screenW, screenH);
        }

        public void Reset()
        {
            _tower.Reset();
            _dragIndex = -1;
        }

        // ── update ────────────────────────────────────────────────────────────
        public void Update()
        {
            var mouse    = Mouse.GetState();
            var mousePos = new Vector2(mouse.X, mouse.Y);

            HandleDrag(mouse, mousePos);
            _tower.Update(_dragIndex);

            _prevMouse = mouse;
        }

        private void HandleDrag(MouseState mouse, Vector2 mousePos)
        {
            bool down    = mouse.LeftButton  == ButtonState.Pressed;
            bool wasDown = _prevMouse.LeftButton == ButtonState.Pressed;

            // Pick ball on click
            if (down && !wasDown)
            {
                _dragIndex = -1;
                float best = float.MaxValue;
                for (int i = 0; i < _tower.Balls.Count; i++)
                {
                    float d = Vector2.Distance(mousePos, _tower.Balls[i].Position);
                    if (d < _tower.Balls[i].Radius + 14f && d < best)
                    {
                        best       = d;
                        _dragIndex = i;
                    }
                }
                if (_dragIndex >= 0)
                    _tower.Balls[_dragIndex].IsDragged = true;
            }

            // Release
            if (!down && wasDown && _dragIndex >= 0)
            {
                _tower.Balls[_dragIndex].IsDragged = false;
                _dragIndex = -1;
            }

            // While dragging: convert mouse X to angle (string stays taut)
            if (_dragIndex >= 0)
                _tower.Balls[_dragIndex].SetAngleFromX(mousePos.X);
        }

        // ── draw ─────────────────────────────────────────────────────────────
        public void Draw(SpriteBatch sb)
        {
            DrawFrame(sb);
            _tower.Draw(sb, _pixel);
        }

        private void DrawFrame(SpriteBatch sb)
        {
            var tl = _tower.FrameTopLeft;
            var tr = _tower.FrameTopRight;
            var bl = new Vector2(tl.X, tl.Y + _tower.FrameHeight);
            var br = new Vector2(tr.X, tr.Y + _tower.FrameHeight);

            DrawLine(sb, tl, tr, new Color(60, 62, 70), 8);   // top bar
            DrawLine(sb, tl, bl, new Color(60, 62, 70), 8);   // left leg
            DrawLine(sb, tr, br, new Color(60, 62, 70), 8);   // right leg
            DrawLine(sb, bl, br, new Color(60, 62, 70), 8);   // base bar
        }

        private void DrawLine(SpriteBatch sb, Vector2 from, Vector2 to,
                               Color color, int thickness)
        {
            Vector2 edge  = to - from;
            float   angle = MathF.Atan2(edge.Y, edge.X);
            sb.Draw(_pixel,
                new Rectangle((int)from.X, (int)from.Y, (int)edge.Length(), thickness),
                null, color, angle, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}