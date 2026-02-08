using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace A3_NovelVisualization;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont font;
    
    private List<string> uniqueWords;
    private List<string> displayWords;
    private List<Color> displayColors;
    private Dictionary<int, int> wordFrequency;
    
    private bool showUniqueWords = true;
    private KeyboardState oldKeyState;
    private Random random;
    
    private Color color1 = new Color(74, 144, 226);
    private Color color2 = new Color(230, 126, 34);
    private Color color3 = new Color(46, 204, 113);

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        _graphics.PreferredBackBufferWidth = 700;
        _graphics.PreferredBackBufferHeight = 600;
    }

    protected override void Initialize()
    {
        random = new Random();
        uniqueWords = new List<string>();
        displayWords = new List<string>();
        displayColors = new List<Color>();
        wordFrequency = new Dictionary<int, int>();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        font = Content.Load<SpriteFont>("MyFont");
        
        string[] lines = File.ReadAllLines("Content/uniquewords.txt");
        for (int i = 0; i < lines.Length; i++)
        {
            uniqueWords.Add(lines[i]);
        }
        
        string[] freqLines = File.ReadAllLines("Content/wordfrequency.txt");
        for (int i = 0; i < freqLines.Length; i++)
        {
            string[] parts = freqLines[i].Split(':');
            int freq = int.Parse(parts[0]);
            int count = int.Parse(parts[1]);
            wordFrequency.Add(freq, count);
        }
        
        PickRandomWords();
    }

    private void PickRandomWords()
    {
        displayWords.Clear();
        displayColors.Clear();
        
        for (int i = 0; i < 40; i++)
        {
            int randomIndex = random.Next(uniqueWords.Count);
            displayWords.Add(uniqueWords[randomIndex]);
            
            int colorPick = random.Next(3);
            if (colorPick == 0)
            {
                displayColors.Add(color1);
            }
            else if (colorPick == 1)
            {
                displayColors.Add(color2);
            }
            else
            {
                displayColors.Add(color3);
            }
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState keyState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();
        
        if (keyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter))
        {
            showUniqueWords = !showUniqueWords;
        }
        
        if (showUniqueWords && mouseState.LeftButton == ButtonState.Pressed)
        {
            PickRandomWords();
        }
        
        oldKeyState = keyState;
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        
        _spriteBatch.Begin();
        
        if (showUniqueWords)
        {
            DisplayUniqueWords();
        }
        else
        {
            DisplayWordFrequency();
        }
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
    
    private void DisplayUniqueWords()
    {
        if (font == null)
        {
            Console.WriteLine("Font is null!");
            return;
        }
        
        float x = 20;
        float y = 20;
        
        for (int i = 0; i < displayWords.Count; i++)
        {
            string word = displayWords[i];
            Vector2 size = font.MeasureString(word);
            
            if (x + size.X > 680)
            {
                x = 20;
                y = y + font.LineSpacing + 10;
            }
            
            if (y + size.Y > 580)
            {
                break;
            }
            
            Color wordColor = displayColors[i];
            
            _spriteBatch.DrawString(font, word, new Vector2(x, y), wordColor);
            
            x = x + size.X + 10;
        }
    }
    
    private void DisplayWordFrequency()
    {
        float x = 20;
        float y = 580;
        
        foreach (KeyValuePair<int, int> pair in wordFrequency)
        {
            int frequency = pair.Key;
            int count = pair.Value;
            
            float barWidth = count / 10f;
            if (barWidth > 660)
            {
                barWidth = 660;
            }
            
            float barHeight = 5;
            
            Texture2D rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.CornflowerBlue });
            
            _spriteBatch.Draw(rect, new Rectangle((int)x, (int)y, (int)barWidth, (int)barHeight), Color.CornflowerBlue);
            
            y = y - 6;
            
            if (y < 20)
            {
                break;
            }
        }
    }
}