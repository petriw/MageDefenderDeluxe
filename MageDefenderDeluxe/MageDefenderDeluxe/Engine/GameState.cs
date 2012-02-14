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
    public abstract class GameState : Microsoft.Xna.Framework.DrawableGameComponent
    {
        int nextState = 0;
        bool changeState = false;

        public bool ChangeState
        {
            get { return changeState; }
            set { changeState = value; }
        }

        public int NextState
        {
            get { return nextState; }
            set { nextState = value; }
        }

        public GameState(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public bool IsStateChange()
        {
            if (changeState)
            {
                changeState = false;
                return true;
            }

            return false;
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
