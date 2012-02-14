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
using MageDefenderDeluxe.LevelSettings;


namespace MageDefenderDeluxe.GameObjects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CastleHandler : Microsoft.Xna.Framework.DrawableGameComponent
    {
        StaticModel castle0;
        StaticModel castle1;
        StaticModel castle2;
        StaticModel castle3;
        StaticModel castle4;
        StaticModel castle5;

        Castle castle;

        public void NextLevel(Level nextLevel)
        {
            castle.EnemiesInCurrentWave = nextLevel.NumEnemies;
            castle.Level++;
        }

        public bool IsGameOver()
        {
            if (castle.Upgrade <= 0)
            {
                return true;
            }
            return false;
        }

        public void ResetAfterGameOver()
        {
            castle.Upgrade = 1;
            castle.Level = 0;
            castle.EnemiesInCurrentWave = 3;
            castle.StoryStage = 1;
        }

        public Castle Castle
        {
            get { return castle; }
            set { castle = value; }
        }

        public CastleHandler(Game game)
            : base(game)
        {
            castle = new Castle();

            castle0 = new StaticModel(game);
            castle1 = new StaticModel(game);
            castle2 = new StaticModel(game);
            castle3 = new StaticModel(game);
            castle4 = new StaticModel(game);
            castle5 = new StaticModel(game);

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            castle0.Initialize();
            castle1.Initialize();
            castle2.Initialize();
            castle3.Initialize();
            castle4.Initialize();
            castle5.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            castle0.Load("Models/castle0");
            castle1.Load("Models/castle1");
            castle2.Load("Models/castle2");
            castle3.Load("Models/castle3");
            castle4.Load("Models/castle4");
            castle5.Load("Models/castle5");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ICamera camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            Matrix objectMatrix = Matrix.CreateRotationX(0) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(0.0f, 155.0f, 0.0f);
            Matrix renderMatrix = Matrix.CreateScale(0.0080f);
            renderMatrix = objectMatrix * renderMatrix;

            castle0.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            castle1.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            castle2.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            castle3.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            castle4.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            castle5.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            switch(castle.Upgrade)
            {
                case 1:
                {
                    castle0.Draw(gameTime);
                    break;
                }
                case 2:
                {
                    castle1.Draw(gameTime);
                    break;
                }
                case 3:
                {
                    castle2.Draw(gameTime);
                    break;
                }
                case 4:
                {
                    castle3.Draw(gameTime);
                    break;
                }
                case 5:
                {
                    castle4.Draw(gameTime);
                    break;
                }
                case 6:
                {
                    castle5.Draw(gameTime);
                    break;
                }
                default:
                {
                    castle0.Draw(gameTime);
                    break;
                }
            }

            base.Draw(gameTime);
        }
    }
}
