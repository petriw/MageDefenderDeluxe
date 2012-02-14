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
    public class StoryState : GameState
    {
        SpriteBatch spriteBatch;

        Texture2D storyImage;
        Texture2D story2Image;

        public StoryState(Game game)
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            storyImage = Game.Content.Load<Texture2D>("Textures/story");
            story2Image = Game.Content.Load<Texture2D>("Textures/story2");

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
            KeyboardState keybaord = Keyboard.GetState();

            if (playerState.Buttons.Start == ButtonState.Pressed)
            {
                NextState = (int)MageDefenderStates.Shop;
                ChangeState = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            ICastle castle = (ICastle)Game.Services.GetService(typeof(ICastle));

            spriteBatch.Begin();
            switch (castle.StoryStage)
            {
                case 1:
                    {
                        spriteBatch.Draw(storyImage, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
                        break;
                    }
                case 2:
                    {
                        spriteBatch.Draw(story2Image, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
                        break;
                    }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
