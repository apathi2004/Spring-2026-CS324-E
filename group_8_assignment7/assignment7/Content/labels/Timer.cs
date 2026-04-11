using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment7.Content.labels;

public class Timer
{
    public float TimerNum;
    public String TimerLabelText;
    public String TimerText;

    public Timer()
    {
        TimerNum = 0f;
        TimerLabelText = "Timer:";
        TimerText = TimerNum.ToString();
    }

    public void Update(GameTime gameTime)
    {
        TimerNum += gameTime.ElapsedGameTime.Seconds;
        TimerText = TimerNum.ToString();
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice graphicsDevice)
    {
        spriteBatch.DrawString(
            font, 
            new StringBuilder(TimerLabelText), 
            new Vector2(
                graphicsDevice.Viewport.Width - font.MeasureString(TimerLabelText).X - 80, 
                10), 
            Color.White);
        spriteBatch.DrawString(
            font,
            new StringBuilder(TimerText),
            new Vector2(
                graphicsDevice.Viewport.Width - font.MeasureString(TimerText).X - 20, 
                10),
            Color.White);
    }
}