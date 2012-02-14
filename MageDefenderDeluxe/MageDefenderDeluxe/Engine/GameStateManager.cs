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


namespace MageDefenderDeluxe.Engine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameStateManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Stack<GameState> gameStates = new Stack<GameState>();

        public GameStateManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public void ChangeToState(GameState state)
        {
            if (gameStates.Count > 0)
            {
                gameStates.Pop();
            }
            gameStates.Push(state);
        }

        public void Add(GameState state)
        {
            gameStates.Push(state);
        }

        public void Back()
        {
            if (gameStates.Count > 0)
            {
                gameStates.Pop();
            }
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

        public bool IsGameStateChange()
        {
            if (gameStates.Count > 0)
            {
                return gameStates.Peek().IsStateChange();
            }
            return false;
        }

        public int GetNextState()
        {
            if (gameStates.Count > 0)
            {
                return gameStates.Peek().NextState;
            }
            return -1;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (gameStates.Count > 0)
            {
                gameStates.Peek().Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (gameStates.Count > 0)
            {
                gameStates.Peek().Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
