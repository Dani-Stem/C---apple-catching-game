using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace MonoGame
{
    class Root : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int winheight = 0;
        int winwidth = 0;

        // Background
        Rectangle recBg;
        Texture2D textureBg;

        // Basket
        Rectangle recBask;
        Texture2D textureBask;
        int basketSpeed = 10;

        // Apples
        List<Apple> apples;
        Rectangle recApple;
        Texture2D textureApple;
        Random applernd = new Random();
        float Movespeed = 1.5f;
        int time = 0;
        int timeout = 1500;
        TimeSpan ptime, atime;
        //scoreboard
        int scoreBoard = 0;
        SpriteFont scoreFont;


        // Keyboard Inputs
        KeyboardState key;

        public Root()
        {
            graphics = new GraphicsDeviceManager(this);

            // Window
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 540;

            // Fullscreen Window (Max Resolution)
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // Fullscreen (Max Resolution)
            //graphics.IsFullScreen = true;

            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Log.Clear();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            recBg = new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            recBask = new Rectangle(0,GraphicsDevice.Viewport.Height - 100, 100, 100);
            recApple = new Rectangle(applernd.Next(GraphicsDevice.Viewport.Width - 32), 0, 32,36);
            winheight = GraphicsDevice.Viewport.Height;
            winwidth = GraphicsDevice.Viewport.Width;

            apples = new List<Apple>();

            atime = TimeSpan.FromSeconds(1.5f);

            Log.Print("Terminal Active\n");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureBg = Content.Load<Texture2D>("trees2");
            textureBask = Content.Load<Texture2D>("basket");
            textureApple = Content.Load<Texture2D>("Apple");
            scoreFont = Content.Load<SpriteFont>("scoreFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            key = Keyboard.GetState();
            // TODO: Add your update logic here
            Inputs();

            int x, y;
            Random ran = new Random();
            x = ran.Next(0, GraphicsDevice.Viewport.Width - 64);
            y = ran.Next(0, 0);

            if (gameTime.TotalGameTime - ptime > atime)
            {
                ptime = gameTime.TotalGameTime;
                AddApples(new Vector2(x, y));
            }

            Updateapples();
            Collision();

            base.Update(gameTime);
        }
        protected void Inputs(){
            if(key.IsKeyDown(Keys.Right)){
                recBask.X += basketSpeed;
            }

            if(key.IsKeyDown(Keys.Left)){
                recBask.X -= basketSpeed;
            }

            if(recBask.X < 0){
                recBask.X = 0;
            }

            if(recBask.X > winwidth - 100){
                recBask.X = winwidth - 100;
            }
        }

        protected void Collision()
        {

            for (int i = 0; i < apples.Count; i++)
            {
                recApple = new Rectangle
               ((int)apples[i].Position.X - apples[i].Width / 2, (int)apples[i].Position.Y - apples[i].Height / 2,
               apples[i].Width, apples[i].Height);

                if(recApple.Intersects(recBask))
                {
                    //soundEffects[0].CreateInstance().Play();
                    apples.RemoveAt(i);
                    recApple.Y = 0;
                    recApple.X = applernd.Next(GraphicsDevice.Viewport.Width - 100);

                    scoreBoard += 10;
                    //applenum -= 1;
                }

                if (recApple.Y > GraphicsDevice.Viewport.Height)
                {
                    apples.RemoveAt(i);
                    //applemissed += 1;
                    //applenum -= 1;
                    recApple.X = applernd.Next(GraphicsDevice.Viewport.Width);
                    recApple.Y = 0;
                }

            }
        }

        private void AddApples(Vector2 position)
        {
            Apple apple = new Apple();
            apple.Initialize(GraphicsDevice.Viewport, textureApple, position, Movespeed);
            apples.Add(apple);
        }

        private void RemoveApples()
        {

        }

        private void Updateapples()
        {
            for (int i = apples.Count - 1; i >= 0; i--)
            {
                apples[i].Update();

                if (apples[i].Active == false)
                {
                    apples.RemoveAt(i);
                }
            }

        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LemonChiffon);
            spriteBatch.Begin();
            spriteBatch.Draw(textureBg, recBg, Color.White);
            spriteBatch.Draw(textureBask, recBask, Color.White);
            spriteBatch.DrawString(scoreFont, "Your Score:" + scoreBoard.ToString(), new Vector2(10,10), Color.White);
            for (int i = 0; i < apples.Count; i++)
            {
                apples[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
