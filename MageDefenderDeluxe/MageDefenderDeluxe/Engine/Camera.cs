using Microsoft.Xna.Framework;
using MageDefenderDeluxe.Interfaces;
using System;

namespace MageDefenderDeluxe.Engine
{
    public class Camera : GameComponent, ICamera
    {
        private Vector3 position;
        private Vector3 lookAt;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        public Camera(Game game)
            : base(game)
        {
        }

        public Vector3 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public float PositionY
        {
            get { return this.position.Y; }
            set { this.position.Y = value; }
        }

        public Vector3 LookAt
        {
            get { return this.lookAt; }
            set { this.lookAt = value; }
        }
        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
            set { viewMatrix = value; }
        }
        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
            set { projectionMatrix = value; }
        }

        /// <param name="gameTime">Time elapsed since the last call to Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)</param>
        public override void Update(GameTime gameTime)
        {
            this.viewMatrix = Matrix.CreateLookAt(this.position, this.lookAt, Vector3.Up);
        }

        public override void Initialize()
        {
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                                        MathHelper.ToRadians((float)Math.PI / 4),
                                        this.Game.GraphicsDevice.DisplayMode.AspectRatio,
                                        2000.0f,
                                        6000.0f);

            base.Initialize();
        }
    }
}
