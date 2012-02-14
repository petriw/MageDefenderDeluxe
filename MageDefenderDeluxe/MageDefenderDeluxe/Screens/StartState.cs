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


namespace MageDefenderDeluxe.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StartState : GameState
    {
        SpriteBatch spriteBatch;

        Texture2D startScreen;

        float timer = 0;

        public StartState(Game game)
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
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // TODO: Add your initialization code here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            startScreen = Game.Content.Load<Texture2D>("Textures/loading");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer >= 2000)
            {
                this.ChangeState = true;
                timer = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Tomato);

            spriteBatch.Begin();
            spriteBatch.Draw(startScreen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
