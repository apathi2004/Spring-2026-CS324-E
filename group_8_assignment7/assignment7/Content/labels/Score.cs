using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment7.Content.labels;

public class Score
{
    public int ScoreNum;
    public String ScoreLabelText;
    public String ScoreText;

    public Score()
    {
        ScoreNum = 0;
        ScoreLabelText = "Score:";
        ScoreText = ScoreNum.ToString();
    }
    
    public void Update(int scoreIncrease)
    {
        ScoreNum += scoreIncrease;
        ScoreLabelText = ScoreNum.ToString();
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, Timer Timer, GraphicsDevice GraphicsDevice)
    {
        spriteBatch.DrawString(
            font,
            new StringBuilder(ScoreLabelText),
            new Vector2(
                GraphicsDevice.Viewport.Width - font.MeasureString(ScoreLabelText).X - 80, 
                5 + font.MeasureString(Timer.TimerLabelText).Y), 
            Color.White);
        spriteBatch.DrawString(
            font,
            new StringBuilder(Timer.TimerText),
            new Vector2(
                GraphicsDevice.Viewport.Width - font.MeasureString(Timer.TimerText).X - 20, 
                5 + font.MeasureString(Timer.TimerLabelText).Y),
            Color.White);
    }
}