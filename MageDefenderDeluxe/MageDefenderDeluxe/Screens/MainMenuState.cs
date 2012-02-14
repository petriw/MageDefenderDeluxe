using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MageDefenderDeluxe.Engine;
using MageDefenderDeluxe.Interfaces;


namespace MageDefenderDeluxe.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MainMenuState : GameState
    {
        SpriteBatch spriteBatch;

        Texture2D pressStartBGScreen;
        Texture2D selectorTexture;

        int itemSelected = 1;
        float selectTimer = 100;

        public float SelectTimer
        {
            get { return selectTimer; }
            set { selectTimer = value; }
        }

        public MainMenuState(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            selectorTexture = this.Game.Content.Load<Texture2D>("Textures\\shopSelector");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            pressStartBGScreen = Game.Content.Load<Texture2D>("Textures/mainMenu");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            selectTimer -= gameTime.ElapsedGameTime.Milliseconds;

            // TODO: Add your update code here
            IPlayer player = (IPlayer)Game.Services.GetService(typeof(IPlayer));
            GamePadState playerState = GamePad.GetState(player.PlayerIndexSaved);
            KeyboardState keybaord = Keyboard.GetState();

            if (selectTimer <= 0 && ((playerState.DPad.Down == ButtonState.Pressed) || (playerState.ThumbSticks.Left.Y <= -1.0f || keybaord.IsKeyDown(Keys.Down))))
            {
                selectTimer = 200;
                itemSelected++;
            }

            if (selectTimer <= 0 && ((playerState.DPad.Up == ButtonState.Pressed) || (playerState.ThumbSticks.Left.Y >= 1.0f || keybaord.IsKeyDown(Keys.Up))))
            {
                selectTimer = 200;
                itemSelected--;
            }

            if (playerState.Buttons.B == ButtonState.Pressed)
            {
                Game.Exit();
            }

            if (playerState.Buttons.A == ButtonState.Pressed)
            {
                switch (itemSelected)
                {
                    case 1: // Start game
                        {
                            NextState = (int)MageDefenderStates.Story;
                            ChangeState = true;
                            break;
                        }
                    case 5:
                        {
                            NextState = (int)MageDefenderStates.Credits;
                            ChangeState = true;
                            break;
                        }
                }
            }
            
            if (itemSelected <= 1)
                itemSelected = 1;
            if (itemSelected >= 5)
                itemSelected = 5;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            GraphicsDevice.Clear(Color.SteelBlue);
            
            spriteBatch.Begin();
            spriteBatch.Draw(pressStartBGScreen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            
            switch (itemSelected)
            {
                case 1:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.8f), Convert.ToInt32(height * 0.13f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        break;
                    }
                case 2:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.815f), Convert.ToInt32(height * 0.27f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        break;
                    }
                case 3:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.830f), Convert.ToInt32(height * 0.41f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        break;
                    }
                case 4:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.850f), Convert.ToInt32(height * 0.61f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        break;
                    }
                case 5:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.865f), Convert.ToInt32(height * 0.74f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
