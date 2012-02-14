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
using MageDefenderDeluxe.ParticleEngine;


namespace MageDefenderDeluxe.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PressStartState : GameState
    {
        SpriteBatch spriteBatch;

        Texture2D pressStartScreen;
        Texture2D pressStartBGScreen;
        Texture2D pressStartMageScreen;

        ParticleSystem magicMissilePartilces;
        ParticleSystem magicMissilePartilces2;
        ParticleSystem magicMissilePartilces3;

        Camera camera;

        public PressStartState(Game game)
            : base(game)
        {
            camera = new Camera(game);

            magicMissilePartilces = new PressStartParticleSystem(game, game.Content);
            magicMissilePartilces2 = new PressStart2ParticleSystem(game, game.Content);
            magicMissilePartilces3 = new PressStart3ParticleSystem(game, game.Content);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            camera.Initialize();
            camera.Position = new Vector3(0, 0, 100.0f);
            camera.LookAt = new Vector3(0, 0, 0);

            magicMissilePartilces.Initialize();
            magicMissilePartilces2.Initialize();
            magicMissilePartilces3.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            pressStartScreen = Game.Content.Load<Texture2D>("Textures/Start_Title");
            pressStartBGScreen = Game.Content.Load<Texture2D>("Textures/PressStartLoop");
            pressStartMageScreen = Game.Content.Load<Texture2D>("Textures/Start_Mage");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            IPlayer player = (IPlayer)Game.Services.GetService(typeof(IPlayer));

            if (!player.PlayerIndexSet)
            {
                for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
                {
                    if (GamePad.GetState(index).Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(Keys.E))
                    {
                        player.PlayerIndexSaved = index;
                        player.PlayerIndexSet = true;
                        NextState = (int)MageDefenderStates.MainMenu;
                        ChangeState = true;
                        break;
                    }
                }
            }

            camera.Update(gameTime);
            magicMissilePartilces.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            magicMissilePartilces2.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            magicMissilePartilces3.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);

            for (int i = 0; i < 1; i++)
            {
                magicMissilePartilces.AddParticle(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Zero);
            }
            for (int i = 0; i < 5; i++)
            {
                magicMissilePartilces2.AddParticle(new Vector3(21.0f + (float)Math.Sin((float)gameTime.TotalGameTime.Milliseconds), -2.0f + (float)Math.Cos((float)gameTime.TotalGameTime.Milliseconds), 0.0f), Vector3.Zero);
            }
            for (int i = 0; i < 5; i++)
            {
                magicMissilePartilces3.AddParticle(new Vector3(21.0f + (float)Math.Sin((float)gameTime.TotalGameTime.Milliseconds), -2.0f + (float)Math.Cos((float)gameTime.TotalGameTime.Milliseconds), 0.0f), Vector3.Zero);
            }

            magicMissilePartilces.Update(gameTime);
            magicMissilePartilces2.Update(gameTime);
            magicMissilePartilces3.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            
            spriteBatch.Begin();

            spriteBatch.Draw(pressStartBGScreen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            spriteBatch.Draw(pressStartMageScreen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            spriteBatch.Draw(pressStartScreen, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);

            magicMissilePartilces.Draw(gameTime);
            magicMissilePartilces3.Draw(gameTime);
            magicMissilePartilces2.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
