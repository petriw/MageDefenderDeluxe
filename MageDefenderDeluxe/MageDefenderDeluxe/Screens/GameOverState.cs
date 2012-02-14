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
    public class GameOverState : GameState
    {
        Texture2D gameOverScreen;
        SpriteBatch spriteBatch;

        public GameOverState(Game game)
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameOverScreen = this.Game.Content.Load<Texture2D>("Textures\\gameover");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            IPlayer player = (IPlayer)Game.Services.GetService(typeof(IPlayer));

            GamePadState playerState = GamePad.GetState(player.PlayerIndexSaved);
            KeyboardState keyboard = Keyboard.GetState();

            if (playerState.Buttons.X == ButtonState.Pressed || keyboard.IsKeyDown(Keys.E))
            {
                NextState = (int)MageDefenderStates.MainMenu;
                ChangeState = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            spriteBatch.Begin();
            spriteBatch.Draw(gameOverScreen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
