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


namespace MageDefenderDeluxe.Engine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StaticModel : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Model theModel;

        Matrix view;
        Matrix projection;
        Matrix render;

        public Model TheModel
        {
            get { return theModel; }
            set { theModel = value; }
        }

        public StaticModel(Game game)
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

        public void Load(string modelname)
        {
            theModel = Game.Content.Load<Model>(modelname);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Matrix render, Matrix view, Matrix projection)
        {
            // TODO: Add your update code here
            this.render = render;
            this.view = view;
            this.projection = projection;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[theModel.Bones.Count];
            
            theModel.CopyAbsoluteBoneTransformsTo(transforms);
            
            foreach (ModelMesh mesh in theModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.SpecularPower = 1000.0f;

                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = transforms[mesh.ParentBone.Index] * render;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
