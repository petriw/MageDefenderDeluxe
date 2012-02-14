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
using SkinnedModel;


namespace MageDefenderDeluxe.Engine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AnimatedModel : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Model theModel;
        AnimationPlayer animationPlayer;
        SkinningData skinningData;

        Matrix render;
        Matrix view;
        Matrix projection;

        string modelName = "";


        public AnimatedModel(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(string model)
        {
            modelName = model;
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            theModel = Game.Content.Load<Model>(modelName);

            // Look up our custom skinning information.
            skinningData = theModel.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            base.LoadContent();
        }

        public void PlayAnimation(string id)
        {
            if (skinningData != null)
            {
                AnimationClip clip = skinningData.AnimationClips[id];

                animationPlayer.StartClip(clip);
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Matrix render, Matrix view, Matrix projection)
        {
            // TODO: Add your update code here
            animationPlayer.Update(gameTime.ElapsedGameTime, true, render);

            this.render = render;
            this.view = view;
            this.projection = projection;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = Game.GraphicsDevice;

            Matrix[] bones = animationPlayer.GetSkinTransforms();

            // Render the skinned mesh.
            foreach (ModelMesh mesh in theModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
