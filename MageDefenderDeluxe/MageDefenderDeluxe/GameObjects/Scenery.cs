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


namespace MageDefenderDeluxe.GameObjects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Scenery : Microsoft.Xna.Framework.DrawableGameComponent
    {
        StaticModel theGround;
        StaticModel theTrees;
        StaticModel theShrooms;
        StaticModel theRiver;

        public Scenery(Game game)
            : base(game)
        {
            theGround = new StaticModel(game);
            theTrees = new StaticModel(game);
            theShrooms = new StaticModel(game);
            theRiver = new StaticModel(game);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            theGround.Initialize();
            theTrees.Initialize();
            theShrooms.Initialize();
            theRiver.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            theGround.Load("Models/plane");
            theTrees.Load("Models/trees");
            theShrooms.Load("Models/shrooms");
            theRiver.Load("Models/planeRiver");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ICamera camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            Matrix objectMatrix = Matrix.CreateRotationX(0) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            Matrix renderMatrix = Matrix.CreateScale(0.0080f);
            renderMatrix = objectMatrix * renderMatrix;

            theGround.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            theRiver.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);

            objectMatrix = Matrix.CreateRotationX(0) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(0.0f, 50.0f, 0.0f);
            renderMatrix = Matrix.CreateScale(0.0080f);
            renderMatrix = objectMatrix * renderMatrix;
            theTrees.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            theShrooms.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            theGround.Draw(gameTime);
            theTrees.Draw(gameTime);
            theShrooms.Draw(gameTime);
            theRiver.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
