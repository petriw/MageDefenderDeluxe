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
using MageDefenderDeluxe.GameObjects;
using MageDefenderDeluxe.ParticleEngine;
using MageDefenderDeluxe.LevelSettings;


namespace MageDefenderDeluxe.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LevelState : GameState
    {
        SpriteBatch spriteBatch;

        Player player;
        Camera camera;
        Scenery scenery;
        CastleHandler castleHandler;
        SpellHandler spellHandler;
        EnemyHandler enemyHandler;

        Texture2D userInterface;
        Texture2D userInterfaceBack;
        Texture2D blankTexture;
        Texture2D levelUpNotifier;
        Texture2D waveCompleted;
        Texture2D tutorial;
        Texture2D bossInfo;
        List<Texture2D> spellIcons;

        ParticleSystem fireBallParticles;
        ParticleSystem slowEnemyParticles;
        ParticleSystem poisonEnemyParticles;
        ParticleSystem magicMissilePartilces;
        ParticleSystem dirtPartilces;

        SpriteFont m_font;
        SpriteFont m_fontLarge;
        float cameraZoom = 0.8f;
        float pressTimerPrimary = 100;
        float pressTimerSecondary = 100;
        float pressTimerSpellBrowse = 100;
        float drinkPotionTimer = 100;
        private bool isCurrentLevelABoss;

        public void CheckIfGameOver()
        {
            if (castleHandler.IsGameOver())
            {
                NextState = (int)MageDefenderStates.GameOver;
                ChangeState = true;

                player.ResetAfterGameOver();
                enemyHandler.ResetAfterGameOver();
                castleHandler.ResetAfterGameOver();
                spellHandler.ResetAfterGameOver();

            }
        }

        public LevelState(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

            player = new Player(game);
            camera = new Camera(game);
            scenery = new Scenery(game);
            castleHandler = new CastleHandler(game);
            spellHandler = new SpellHandler(game);
            enemyHandler = new EnemyHandler(game);

            fireBallParticles = new FireParticleSystem(game, game.Content);
            slowEnemyParticles = new SlowParticleSystem(game, game.Content);
            poisonEnemyParticles = new PoisonParticleSystem(game, game.Content);
            magicMissilePartilces = new MagicMissileParticleSystem(game, game.Content);
            dirtPartilces = new DirtParticleSystem(game, game.Content);

            spellIcons = new List<Texture2D>();

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            m_font = Game.Content.Load<SpriteFont>(@"Comic");
            m_fontLarge = Game.Content.Load<SpriteFont>(@"ComicLarge");

            player.Initialize();
            Game.Services.AddService(typeof(Interfaces.IPlayer), player);

            camera.Initialize();
            camera.Position = new Vector3(0 * cameraZoom, 1688.753f * cameraZoom, -2228.385f * cameraZoom);
            camera.LookAt = new Vector3(0, 0, 20);
            Game.Services.AddService(typeof(Interfaces.ICamera), camera);

            scenery.Initialize();
            castleHandler.Initialize();
            Game.Services.AddService(typeof(Interfaces.ICastle), castleHandler.Castle);
            spellHandler.Initialize();
            Game.Services.AddService(typeof(Interfaces.ISpellHandler), spellHandler);
            enemyHandler.Initialize();

            enemyHandler.SpawnEnemyTypes.Add(EnemyHandler.Enemies.Slime);

            fireBallParticles.Initialize();
            slowEnemyParticles.Initialize();
            poisonEnemyParticles.Initialize();
            magicMissilePartilces.Initialize();
            dirtPartilces.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            userInterface = Game.Content.Load<Texture2D>("Textures/mdUI");
            blankTexture = Game.Content.Load<Texture2D>("Textures/blank");
            levelUpNotifier = Game.Content.Load<Texture2D>("Textures/levelup");
            userInterfaceBack = Game.Content.Load<Texture2D>("Textures/mdUI_back");
            waveCompleted = Game.Content.Load<Texture2D>("Textures/WaveCompleted");
            tutorial = Game.Content.Load<Texture2D>("Textures/helpscreen");
            bossInfo = Game.Content.Load<Texture2D>("Textures/bossHealthBar");

            spellIcons.Add(Game.Content.Load<Texture2D>("Textures\\icon_fireball"));
            spellIcons.Add(Game.Content.Load<Texture2D>("Textures\\icon_slow"));
            spellIcons.Add(Game.Content.Load<Texture2D>("Textures\\icon_poison"));
            spellIcons.Add(Game.Content.Load<Texture2D>("Textures\\icon_MM"));
            spellIcons.Add(Game.Content.Load<Texture2D>("Textures\\icon_energyball"));
            spellIcons.Add(Game.Content.Load<Texture2D>("Textures\\icon_MM2"));
            spellIcons.Add(Game.Content.Load<Texture2D>("Textures\\icon_MM4"));

            base.LoadContent();
        }

        public void PrepareNextLevel()
        {
            pressTimerPrimary = 500;
            pressTimerSecondary = 500;
            pressTimerSpellBrowse = 500;
            drinkPotionTimer = 500;

            Level nextLevel;
            try
            {
                nextLevel = Game.Content.Load<Level>("Levels/level" + (castleHandler.Castle.Level + 1));
            }
            catch(Exception ex)
            {
                nextLevel = new Level();
            }

            isCurrentLevelABoss = nextLevel.IsBoss;

            if (isCurrentLevelABoss)
            {
                if (!nextLevel.BossCodeName.Equals("None"))
                {
                    enemyHandler.BossBehaviour = Game.Content.Load<BossBehaviour>("Bosses/" + nextLevel.BossCodeName);
                }
                else
                {
                    enemyHandler.BossBehaviour = null;
                }
            }
            else
            {
                enemyHandler.BossBehaviour = null;
            }

            string[] spawnEnemiesofTypes = nextLevel.SpawnEnemies.Split(',');


            spellHandler.NextLevel();
            enemyHandler.NextLevel();
            castleHandler.NextLevel(nextLevel);

            player.Mana = player.MaxMana;
            player.Health = player.MaxHealth;

            foreach(String s in spawnEnemiesofTypes)
            {
                if (s.Equals("Slime1"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Slime);
                }
                if (s.Equals("Slime2"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Slime2);
                }
                if (s.Equals("Slime3"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Slime3);
                }
                if (s.Equals("Slime4"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Slime4);
                }
                if (s.Equals("Ghost"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Ghost);
                }
                if (s.Equals("Zombie"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Zombie);
                }
                if (s.Equals("Skeleton"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Skeleton);
                }
                if (s.Equals("Boss"))
                {
                    enemyHandler.SpawnEnemyTypes.Add(GameObjects.EnemyHandler.Enemies.Boss);
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            CheckIfGameOver();

            pressTimerPrimary -= gameTime.ElapsedGameTime.Milliseconds * 2;
            pressTimerSecondary -= gameTime.ElapsedGameTime.Milliseconds;
            pressTimerSpellBrowse -= gameTime.ElapsedGameTime.Milliseconds;
            drinkPotionTimer -= gameTime.ElapsedGameTime.Milliseconds;

            GamePadState playerState = GamePad.GetState(player.PlayerIndexSaved);
            KeyboardState keyboard = Keyboard.GetState();

            cameraZoom += playerState.ThumbSticks.Right.Y/50.0f;
            if (cameraZoom <= 0.2f)
                cameraZoom = 0.2f;

            if (cameraZoom >= 1.76f)
                cameraZoom = 1.76f;


            camera.Position = new Vector3(0 * cameraZoom, 1688.753f * cameraZoom, -2228.385f);

            // Browse spells to the right
            if ((playerState.Buttons.RightShoulder == ButtonState.Pressed || keyboard.IsKeyDown(Keys.D)) && pressTimerSpellBrowse < 0)
            {
                int ss = (int)spellHandler.SelectedSpellIndex;
                if (ss < spellHandler.LearnedSpells.Count - 1)
                {
                    spellHandler.SelectedSpellIndex++;
                    spellHandler.SelectedSpell = spellHandler.LearnedSpells[spellHandler.SelectedSpellIndex];
                }
                pressTimerSpellBrowse = 100;
            }

            // Browse spells to the left
            if ((playerState.Buttons.LeftShoulder == ButtonState.Pressed || keyboard.IsKeyDown(Keys.A)) && pressTimerSpellBrowse < 0)
            {
                int ss = (int)spellHandler.SelectedSpellIndex;
                if (ss > 0)
                {
                    spellHandler.SelectedSpellIndex--;
                    spellHandler.SelectedSpell = spellHandler.LearnedSpells[spellHandler.SelectedSpellIndex];
                }
                pressTimerSpellBrowse = 100;
            }

            // Shoot primary spell
            if ((playerState.Buttons.X == ButtonState.Pressed || keyboard.IsKeyDown(Keys.LeftControl)) && pressTimerPrimary < 0)
            {
                int manaCost = spellHandler.GetManaCost(GameObjects.SpellHandler.Spells.Fireball);
                if (player.Mana >= manaCost)
                {
                    pressTimerPrimary = player.CalculateSpellRecharge();
                    spellHandler.AddSpell(player.Position, GameObjects.SpellHandler.Spells.Fireball, Vector3.Zero);
                    player.Mana -= manaCost;
                    player.PlayAnimation(3);

                    //shootCue = soundBank.GetCue("shoot");
                    //if (!shootCue.IsPlaying)
                    //{
                    //    shootCue.Play();
                    //}
                }
            }

            // Shoot secondary spell
            if ((playerState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.LeftAlt)) && pressTimerSecondary < 0)
            {
                int manaCost = spellHandler.GetManaCost(spellHandler.SelectedSpell);
                if (player.Mana >= manaCost)
                {
                    //shootCue = soundBank.GetCue("shoot");
                    //if (!shootCue.IsPlaying)
                    //{
                    //    shootCue.Play();
                    //}

                    pressTimerSecondary = player.CalculateSpellRecharge();
                    if (enemyHandler.EnemiesList.Count > 0)
                    {
                        Random rndEnemyOnScreen = new Random();
                        if (spellHandler.SelectedSpell == SpellHandler.Spells.MagicMissileX2 || spellHandler.SelectedSpell == SpellHandler.Spells.MagicMissileX4)
                        {
                            spellHandler.AddSpell(player.Position, SpellHandler.Spells.MagicMissile, enemyHandler.EnemiesList[rndEnemyOnScreen.Next(0, enemyHandler.EnemiesList.Count)].Position);
                        }
                        else
                        {
                            spellHandler.AddSpell(player.Position, spellHandler.SelectedSpell, enemyHandler.EnemiesList[rndEnemyOnScreen.Next(0, enemyHandler.EnemiesList.Count)].Position);
                        }

                        // some special rules regarding some spells are defined under here
                        if (spellHandler.SelectedSpell == SpellHandler.Spells.MagicMissileX2)
                        {
                            spellHandler.AddSpell(player.Position, SpellHandler.Spells.MagicMissile, enemyHandler.EnemiesList[rndEnemyOnScreen.Next(0, enemyHandler.EnemiesList.Count)].Position);
                        }

                        if (spellHandler.SelectedSpell == MageDefenderDeluxe.GameObjects.SpellHandler.Spells.MagicMissileX4)
                        {
                            spellHandler.AddSpell(player.Position, SpellHandler.Spells.MagicMissile, enemyHandler.EnemiesList[rndEnemyOnScreen.Next(0, enemyHandler.EnemiesList.Count)].Position);
                            spellHandler.AddSpell(player.Position, SpellHandler.Spells.MagicMissile, enemyHandler.EnemiesList[rndEnemyOnScreen.Next(0, enemyHandler.EnemiesList.Count)].Position);
                            spellHandler.AddSpell(player.Position, SpellHandler.Spells.MagicMissile, enemyHandler.EnemiesList[rndEnemyOnScreen.Next(0, enemyHandler.EnemiesList.Count)].Position);
                        }

                    }
                    else
                    {
                        spellHandler.AddSpell(player.Position, spellHandler.SelectedSpell, new Vector3(player.Position.X, 300, player.Position.Z));
                    }

                    player.Mana -= manaCost;
                    player.PlayAnimation(3);
                }
            }

            // Drink a health potion
            if ((playerState.Buttons.B == ButtonState.Pressed || keyboard.IsKeyDown(Keys.W)) && drinkPotionTimer < 0)
            {
                if (player.HealthPotions > 0)
                {
                    drinkPotionTimer = 1000;
                    player.HealthPotions -= 1;
                    player.Health += 50 + (player.Strength * 5);
                    if (player.Health >= player.MaxHealth)
                    {
                        player.Health = player.MaxHealth;
                    }
                }
            }

            // Drink a mana potion
            if ((playerState.Buttons.Y == ButtonState.Pressed || keyboard.IsKeyDown(Keys.S)) && drinkPotionTimer < 0)
            {
                if (player.ManaPotions > 0)
                {
                    drinkPotionTimer = 1000;
                    player.ManaPotions -= 1;
                    player.Mana += 50 + (player.Constitution * 5);
                    if (player.Mana >= player.MaxMana)
                    {
                        player.Mana = player.MaxMana;
                    }
                }
            }

            enemyHandler.IsCollidingWithSpell(spellHandler);
            enemyHandler.IsCollidingWithPlayer(player.Position);

            scenery.Update(gameTime);
            player.Update(gameTime);
            castleHandler.Update(gameTime);
            camera.Update(gameTime);
            spellHandler.Update(gameTime, playerState.ThumbSticks.Right.X);
            enemyHandler.Update(gameTime);


            // Set proj and view matrix on particles
            fireBallParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            slowEnemyParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            poisonEnemyParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            magicMissilePartilces.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            dirtPartilces.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);

            if (player.Position.X < -1200)
            {
                for (int i = 0; i < 2; i++)
                {
                    magicMissilePartilces.AddParticle(new Vector3(player.Position.X * 0.008f, player.Position.Y * 0.008f, player.Position.Z * 0.008f), Vector3.Zero);
                }
            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    dirtPartilces.AddParticle(new Vector3(player.Position.X * 0.008f, player.Position.Y * 0.008f, player.Position.Z * 0.008f), Vector3.Zero);
                }
            }

            // Update particles
            poisonEnemyParticles.Update(gameTime);
            slowEnemyParticles.Update(gameTime);
            fireBallParticles.Update(gameTime);
            magicMissilePartilces.Update(gameTime);
            dirtPartilces.Update(gameTime);


            if (enemyHandler.AllEnemiesDead && enemyHandler.WaveFinishedTimer < 0)
            {
                // Hardcode some storymode

                // If the level you completed was 6, go to undead mode
                if (castleHandler.Castle.Level == 6)
                {
                    castleHandler.Castle.StoryStage = 2;
                    NextState = (int)MageDefenderStates.Story;
                }
                // If the level you completed was 12, go to XXX mode
                else if (castleHandler.Castle.Level == 12)
                {
                    NextState = (int)MageDefenderStates.Story;
                }
                else
                {
                    NextState = (int)MageDefenderStates.Shop;
                }

                ChangeState = true;

                PrepareNextLevel();
            }



            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            GraphicsDevice.Clear(Color.Black);
            scenery.Draw(gameTime);
            player.Draw(gameTime);
            castleHandler.Draw(gameTime);
            spellHandler.Draw(gameTime);
            enemyHandler.Draw(gameTime);

            spriteBatch.Begin();


            slowEnemyParticles.Draw(gameTime);
            poisonEnemyParticles.Draw(gameTime);
            fireBallParticles.Draw(gameTime);
            magicMissilePartilces.Draw(gameTime);
            dirtPartilces.Draw(gameTime);


            spriteBatch.Draw(userInterfaceBack, new Rectangle(0, 0, width, height), Color.White);
            // Draw healthbar
            float healthLeft = ((float)player.Health / (float)player.MaxHealth) * ((float)width * 0.16f);
            spriteBatch.Draw(blankTexture, new Rectangle(Convert.ToInt32(width * 0.047f), Convert.ToInt32(height * 0.07f), (int)healthLeft, Convert.ToInt32(height * 0.0222f)), Color.Red);

            float manaLeft = ((float)player.Mana / (float)player.MaxMana) * ((float)width * 0.16f);
            spriteBatch.Draw(blankTexture, new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.11f), (int)manaLeft, Convert.ToInt32(height * 0.0222f)), Color.Blue);

            float castleHealth = ((1.0f - ((float)player.Xp / (float)player.XpToNextLevel))) * ((float)height * 0.06f);
            spriteBatch.Draw(blankTexture, new Rectangle(Convert.ToInt32(width * 0.232f), Convert.ToInt32(height * 0.07f), Convert.ToInt32(height * 0.02f), (int)castleHealth), new Color(95, 68, 40, 255));

            spriteBatch.Draw(userInterface, new Rectangle(0, 0, width, height), Color.White);

            spriteBatch.DrawString(m_fontLarge, "" + (castleHandler.Castle.Upgrade) + "/6", new Vector2(Convert.ToInt32(width * 0.142f), Convert.ToInt32(height * 0.185)), Color.Black);
            spriteBatch.DrawString(m_fontLarge, "" + (castleHandler.Castle.Upgrade) + "/6", new Vector2(Convert.ToInt32(width * 0.14f), Convert.ToInt32(height * 0.18)), Color.White);

            spriteBatch.DrawString(m_fontLarge, "" + (player.Level), new Vector2(Convert.ToInt32(width * 0.245f), Convert.ToInt32(height * 0.07)), Color.Black);
            spriteBatch.DrawString(m_fontLarge, "" + (player.Level), new Vector2(Convert.ToInt32(width * 0.247f), Convert.ToInt32(height * 0.065)), Color.White);

            spriteBatch.DrawString(m_font, "" + (castleHandler.Castle.Level), new Vector2(Convert.ToInt32(width * 0.835f), Convert.ToInt32(height * 0.06)), Color.Black);
            spriteBatch.DrawString(m_font, "" + (castleHandler.Castle.Level), new Vector2(Convert.ToInt32(width * 0.837f), Convert.ToInt32(height * 0.055)), Color.White);

            //spriteBatch.DrawString(m_font, "" + (m_Castle.TheCastle.Wave), new Vector2(Convert.ToInt32(width * 0.835f), Convert.ToInt32(height * 0.1)), Color.Black);
            //spriteBatch.DrawString(m_font, "" + (m_Castle.TheCastle.Wave), new Vector2(Convert.ToInt32(width * 0.837f), Convert.ToInt32(height * 0.095)), Color.White);

            spriteBatch.DrawString(m_fontLarge, "" + (castleHandler.Castle.EnemiesInCurrentWave + enemyHandler.EnemiesList.Count), new Vector2(Convert.ToInt32(width * 0.855f), Convert.ToInt32(height * 0.195)), Color.Black);
            spriteBatch.DrawString(m_fontLarge, "" + (castleHandler.Castle.EnemiesInCurrentWave + enemyHandler.EnemiesList.Count), new Vector2(Convert.ToInt32(width * 0.857f), Convert.ToInt32(height * 0.190)), Color.White);

            spriteBatch.DrawString(m_fontLarge, "" + (player.ManaPotions), new Vector2(Convert.ToInt32(width * 0.875f), Convert.ToInt32(height * 0.86)), Color.Black);
            spriteBatch.DrawString(m_fontLarge, "" + (player.ManaPotions), new Vector2(Convert.ToInt32(width * 0.877f), Convert.ToInt32(height * 0.855)), Color.White);

            spriteBatch.DrawString(m_fontLarge, "" + (player.HealthPotions), new Vector2(Convert.ToInt32(width * 0.095f), Convert.ToInt32(height * 0.86)), Color.Black);
            spriteBatch.DrawString(m_fontLarge, "" + (player.HealthPotions), new Vector2(Convert.ToInt32(width * 0.097f), Convert.ToInt32(height * 0.855)), Color.White);


            switch (spellHandler.SelectedSpell)
            {
                case SpellHandler.Spells.Fireball:
                    {
                        spriteBatch.Draw(spellIcons[0], new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.18f), Convert.ToInt32(width * 0.025f * 1.5f), Convert.ToInt32(height * 0.0444f * 1.5f)), Color.White);
                        break;
                    }
                case SpellHandler.Spells.Slowball:
                    {
                        spriteBatch.Draw(spellIcons[1], new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.18f), Convert.ToInt32(width * 0.025f * 1.5f), Convert.ToInt32(height * 0.0444f * 1.5f)), Color.White);
                        break;
                    }
                case SpellHandler.Spells.PoisonBall:
                    {
                        spriteBatch.Draw(spellIcons[2], new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.18f), Convert.ToInt32(width * 0.025f * 1.5f), Convert.ToInt32(height * 0.0444f * 1.5f)), Color.White);
                        break;
                    }
                case SpellHandler.Spells.MagicMissile:
                    {
                        spriteBatch.Draw(spellIcons[3], new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.18f), Convert.ToInt32(width * 0.025f * 1.5f), Convert.ToInt32(height * 0.0444f * 1.5f)), Color.White);
                        break;
                    }
                case SpellHandler.Spells.EnergyBall:
                    {
                        spriteBatch.Draw(spellIcons[4], new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.18f), Convert.ToInt32(width * 0.025f * 1.5f), Convert.ToInt32(height * 0.0444f * 1.5f)), Color.White);
                        break;
                    }
                case SpellHandler.Spells.MagicMissileX2:
                    {
                        spriteBatch.Draw(spellIcons[5], new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.18f), Convert.ToInt32(width * 0.025f * 1.5f), Convert.ToInt32(height * 0.0444f * 1.5f)), Color.White);
                        break;
                    }
                case SpellHandler.Spells.MagicMissileX4:
                    {
                        spriteBatch.Draw(spellIcons[6], new Rectangle(Convert.ToInt32(width * 0.050f), Convert.ToInt32(height * 0.18f), Convert.ToInt32(width * 0.025f * 1.5f), Convert.ToInt32(height * 0.0444f * 1.5f)), Color.White);
                        break;
                    }
            }


            if (castleHandler.Castle.EnemiesInCurrentWave <= 0)
            {
                if (enemyHandler.AllEnemiesDead && enemyHandler.WaveFinishedTimer > 0)
                {
                    spriteBatch.Draw(waveCompleted, new Rectangle(0, 0, width, height), new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
            }

            if (castleHandler.Castle.Level <= 0)
            {
                 //spriteBatch.Draw(tutorial, new Rectangle(0, 0, width, height), new Color(1.0f, 1.0f, 1.0f, 0.2f));
            }

            if (player.DisplayLevelUpTimer > 0)
            {
                spriteBatch.Draw(levelUpNotifier, new Rectangle(0, 0, width, height), new Color(1.0f, 1.0f, 1.0f, (player.DisplayLevelUpTimer / 3000.0f)));
            }


            if (isCurrentLevelABoss)
            {
                if (enemyHandler.SpawnEnemyTypes.Count >= 1)
                {
                    if (enemyHandler.EnemiesList.Count >= 1)
                    {
                        spriteBatch.Draw(bossInfo, new Rectangle(0, 0, width, height), new Color(new Vector4(1, 1, 1, 1)));
                        //spriteBatch.DrawString(m_font, "Boss HP " + m_EnemyHandler.EnemiesList[0].Health, new Vector2(900, 80), Color.Black);
                        float bossHealth = ((float)enemyHandler.EnemiesList[0].Health / (float)enemyHandler.EnemiesList[0].MaxHealth) * ((float)width * 0.435f);
                        spriteBatch.Draw(blankTexture, new Rectangle(Convert.ToInt32(width * 0.267f), Convert.ToInt32(height * 0.1055f), (int)bossHealth, Convert.ToInt32(height * 0.0222f)), Color.Green);
                        spriteBatch.DrawString(m_font, "Boss: " + enemyHandler.EnemiesList[0].Name, new Vector2(Convert.ToInt32(width * 0.267f), Convert.ToInt32(height * 0.13f)), Color.Gold);
                    }
                }
            }

            spriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            base.Draw(gameTime);
        }
    }
}
