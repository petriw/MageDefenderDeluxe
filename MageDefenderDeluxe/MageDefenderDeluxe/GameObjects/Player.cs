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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using MageDefenderDeluxe.Interfaces;
using MageDefenderDeluxe.Engine;


namespace MageDefenderDeluxe.GameObjects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent, IPlayer
    {
        PlayerIndex playerIndexSaved;
        int atGameLevel;
        Vector3 position;
        AnimatedModel playerModel;

        bool animIdlePlaying = true;
        bool animWalkLeftPlaying = false;
        bool animWalkRightPlaying = false;
        bool animCastPlaying = false;

        float addToPosX = 0.0f;

        public float Direction
        {
            get
            {
                float dir = 0;
                if (addToPosX > 0)
                {
                    dir = 1;
                }
                else if (addToPosX < 0)
                {
                    dir = -1;
                }
                else
                {
                    dir = 0;
                }
                return dir;
            }
        }

        float displayLevelUpTimer = 0.0f;
        public float DisplayLevelUpTimer
        {
            get { return displayLevelUpTimer; }
            set { displayLevelUpTimer = value; }
        }

        int manaPotions;
        public int ManaPotions
        {
            get { return manaPotions; }
            set { manaPotions = value; }
        }

        int healthPotions;
        public int HealthPotions
        {
            get { return healthPotions; }
            set { healthPotions = value; }
        }

        int ap;
        public int Ap
        {
            get { return ap; }
            set { ap = value; }
        }

        int xpToNextLevel;
        public int XpToNextLevel
        {
            get { return xpToNextLevel; }
            set { xpToNextLevel = value; }
        }

        float mana;
        public float Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        int gold;
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        int xp;
        public int Xp
        {
            get { return xp; }
            set { xp = value; }
        }

        int level;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        int agility;
        public int Agility
        {
            get { return agility; }
            set { agility = value; }
        }

        int strength;
        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        int constitution;
        public int Constitution
        {
            get { return constitution; }
            set { constitution = value; }
        }

        int intelligence;
        public int Intelligence
        {
            get { return intelligence; }
            set { intelligence = value; }
        }

        int wisdom;
        public int Wisdom
        {
            get { return wisdom; }
            set { wisdom = value; }
        }

        public int CalculateSpellRecharge()
        {
            return 100 + (1000 - ((Constitution * 5) + (Agility * 5)));
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }


        public int MaxHealth
        {
            get { return (strength * 10); }
        }

        public int MaxMana
        {
            get { return (wisdom * 10); }
        }

        public int AtGameLevel
        {
            get { return atGameLevel; }
            set { atGameLevel = value; }
        }



        public PlayerIndex PlayerIndexSaved
        {
            get { return playerIndexSaved; }
            set { playerIndexSaved = value; }
        }

        bool playerIndexSet = false;

        public bool PlayerIndexSet
        {
            get { return playerIndexSet; }
            set { playerIndexSet = value; }
        }

        public bool LevelUp()
        {
            if (xp >= xpToNextLevel)
            {
                // Store XP gained above level during levelup
                int diff = xp - xpToNextLevel;

                Ap += 5;
                gold += 50 * level;
                xp = diff;
                level++;
                xpToNextLevel = Convert.ToInt32(level * 100 * 1.5f);

                displayLevelUpTimer = 2000;
                // learn new spells


                return true;
            }
            else return false;
        }

        public void ResetAfterGameOver()
        {
            position = new Vector3(0, 0, 0);

            Ap = 0;

            manaPotions = 5;
            healthPotions = 5;

            strength = 10;
            constitution = 10;
            intelligence = 10;
            wisdom = 10;
            agility = 10;

            mana = 100;
            health = 100;
            gold = 0;
            xpToNextLevel = 0;
            level = 0;
            Xp = 0;

            LevelUp();
        }

        public Player(Game game)
            : base(game)
        {
            position = new Vector3(0, 0, 0);
            playerModel = new AnimatedModel(game);

            Ap = 0;

            manaPotions = 5;
            healthPotions = 5;

            strength = 10;
            constitution = 10;
            intelligence = 10;
            wisdom = 10;
            agility = 10;

            mana = 100;
            health = 100;
            gold = 0;
            xpToNextLevel = 0;
            level = 0;

            LevelUp();

        }

        public override void Initialize()
        {
            atGameLevel = 1;

            agility = 10;
            constitution = 10;

            playerModel.Initialize("Models/mageX");
            playerModel.PlayAnimation("Idle");

            base.Initialize();
        }


        /// <summary>
        /// Play a given animation in the player model
        /// </summary>
        /// <param name="id"></param>
        public void PlayAnimation(int id)
        {
            switch (id)
            {
                case 0:
                    {
                        if (!animIdlePlaying)
                        {
                            animIdlePlaying = true;
                            animWalkLeftPlaying = false;
                            animWalkRightPlaying = false;
                            animCastPlaying = false;
                            playerModel.PlayAnimation("Idle");
                            
                        }
                        break;
                    }
                case 1:
                    {
                        if (!animWalkLeftPlaying)
                        {
                            animIdlePlaying = false;
                            animWalkLeftPlaying = true;
                            animWalkRightPlaying = false;
                            animCastPlaying = false;

                            playerModel.PlayAnimation("Left");
                        }
                        break;
                    }
                case 2:
                    {
                        if (!animWalkRightPlaying)
                        {
                            animIdlePlaying = false;
                            animWalkLeftPlaying = false;
                            animWalkRightPlaying = true;
                            animCastPlaying = false;

                            playerModel.PlayAnimation("Right");
                        }
                        break;
                    }
                case 3:
                    {
                        if (!animCastPlaying)
                        {
                            animIdlePlaying = false;
                            animWalkLeftPlaying = false;
                            animWalkRightPlaying = false;
                            animCastPlaying = true;

                            playerModel.PlayAnimation("Start");
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ICamera camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            Matrix objectMatrix = Matrix.CreateRotationX(0) * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateRotationZ(0) * Matrix.CreateTranslation(position);
            Matrix renderMatrix = Matrix.CreateScale(0.0080f);
            renderMatrix = objectMatrix * renderMatrix;

            GamePadState playerState = GamePad.GetState(this.PlayerIndexSaved);
            KeyboardState keyboard = Keyboard.GetState();

            LevelUp();

            addToPosX = 0.0f;
            if (!keyboard.IsKeyDown(Keys.Left) || (keyboard.IsKeyDown(Keys.Right)))
            {
                addToPosX = (playerState.ThumbSticks.Left.X * ((float)gameTime.ElapsedGameTime.Milliseconds) * ((float)(5 + (agility / 2)) / 10.0f));
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                addToPosX = (-1.0f * ((float)gameTime.ElapsedGameTime.Milliseconds) * ((float)(5 + (agility / 2)) / 10.0f));
            }

            if (keyboard.IsKeyDown(Keys.Right))
            {
                addToPosX = (1.0f * ((float)gameTime.ElapsedGameTime.Milliseconds) * ((float)(5 + (agility / 2)) / 10.0f));
            }

            position.X -= addToPosX;

            if (Direction > 0)
                PlayAnimation(1);
            else if (Direction < 0)
                PlayAnimation(2);
            else if (Direction == 0)
                PlayAnimation(0);

            // Stop player from moving to the sides
            if (position.X >= 1500)
                position.X = 1500;

            if (position.X <= -1500)
                position.X = -1500;

            playerModel.Update(gameTime, renderMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
            displayLevelUpTimer -= gameTime.ElapsedGameTime.Milliseconds;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            playerModel.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}