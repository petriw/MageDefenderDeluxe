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
    public class CreditsState : GameState
    {
        SpriteBatch spriteBatch;
        Texture2D creditsImage;
        Texture2D creditsBG;

        public CreditsState(Game game)
            : base(game)
        {
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
            creditsImage = Game.Content.Load<Texture2D>("Textures/Credits");
            creditsBG = this.Game.Content.Load<Texture2D>("Textures/pressStartLoop");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            IPlayer player = (IPlayer)Game.Services.GetService(typeof(IPlayer));

            GamePadState playerState = GamePad.GetState(player.PlayerIndexSaved);
            KeyboardState keyboard = Keyboard.GetState();

            if (playerState.Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(Keys.E))
            {
                NextState = (int)MageDefenderStates.MainMenu;
                ChangeState = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(creditsBG, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            spriteBatch.Draw(creditsImage, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
