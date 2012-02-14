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
using MageDefenderDeluxe.Engine;
using MageDefenderDeluxe.Screens;
using MageDefenderDeluxe.GameObjects;

namespace MageDefenderDeluxe
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MageDefender : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GameStateManager gameManager;

        StartState startState;
        MainMenuState mainMenuState;
        PressStartState pressStartState;
        StoryState storyState;
        LevelState levelState;
        ShopState shopState;
        GameOverState gameOverState;
        CreditsState creditsState;

        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;
        Cue menuCue;
        Cue ingameCue;

        public MageDefender()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = true;

            gameManager = new GameStateManager(this);

            startState = new StartState(this);
            mainMenuState = new MainMenuState(this);
            pressStartState = new PressStartState(this);
            storyState = new StoryState(this);
            levelState = new LevelState(this);
            shopState = new ShopState(this);
            gameOverState = new GameOverState(this);
            creditsState = new CreditsState(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            gameManager.Initialize();

            pressStartState.Initialize();
            startState.Initialize();
            mainMenuState.Initialize();
            storyState.Initialize();
            levelState.Initialize();
            shopState.Initialize();
            gameOverState.Initialize();
            creditsState.Initialize();

            startState.NextState = (int)MageDefenderStates.PressStart;

            gameManager.Add(startState);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            engine = new AudioEngine("Content\\music.xgs");
            soundBank = new SoundBank(engine, "Content\\SoundBank.xsb");
            waveBank = new WaveBank(engine, "Content\\WaveBank.xwb");

            menuCue = soundBank.GetCue("menuMusic");
            ingameCue = soundBank.GetCue("ingameMusic");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            engine.Update();

            gameManager.Update(gameTime);

            if (gameManager.IsGameStateChange())
            {
                switch (gameManager.GetNextState())
                {
                    case (int)MageDefenderStates.PressStart:
                    {
                        gameManager.ChangeToState(pressStartState);
                        break;
                    }
                    case (int)MageDefenderStates.MainMenu:
                    {
                        menuCue.Stop(AudioStopOptions.Immediate);
                        ingameCue.Stop(AudioStopOptions.Immediate);

                        menuCue = soundBank.GetCue("menuMusic");
                        if (!menuCue.IsPlaying)
                        {
                            menuCue.Play();
                        }
                        gameManager.ChangeToState(mainMenuState);
                        break;
                    }
                    case (int)MageDefenderStates.Story:
                    {
                        gameManager.ChangeToState(storyState);
                        break;
                    }
                    case (int)MageDefenderStates.Level:
                    {
                        menuCue.Stop(AudioStopOptions.Immediate);
                        ingameCue.Stop(AudioStopOptions.Immediate);

                        ingameCue = soundBank.GetCue("ingameMusic");
                        if (!ingameCue.IsPlaying)
                        {
                            ingameCue.Play();
                        }

                        gameManager.ChangeToState(levelState);
                        break;
                    }
                    case (int)MageDefenderStates.Shop:
                    {
                        gameManager.ChangeToState(shopState);
                        break;
                    }
                    case (int)MageDefenderStates.GameOver:
                    {
                        gameManager.ChangeToState(gameOverState);
                        break;
                    }
                    case (int)MageDefenderStates.Credits:
                    {
                        gameManager.ChangeToState(creditsState);
                        break;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            gameManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
