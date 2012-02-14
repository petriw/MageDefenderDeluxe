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
using MageDefenderDeluxe.ParticleEngine;
using MageDefenderDeluxe.LevelSettings;


namespace MageDefenderDeluxe.GameObjects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnemyHandler : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Enemies { Slime = 1, Slime2 = 2, Slime3, Slime4, Boss, Ghost, Skeleton, Zombie };
        public enum AI { Forward = 1, Sin = 2, Boss, Ghost };

        List<StaticModel> enemyModels;
        List<AnimatedModel> animatedEnemyModels;
        List<Enemy> enemies;
        List<Enemies> spawnEnemyTypes;

        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;
        Cue collideCue;

        // Boss realted, or to enemies that are shooting/spawning stuff.
        bool addOneEnemyThisFrame = false;
        Enemies enemyToAdd = Enemies.Slime;
        Vector3 addEnemyAtPos = new Vector3();
        float spitTimer;
        BossBehaviour bossBehaviour;

        public BossBehaviour BossBehaviour
        {
            get { return bossBehaviour; }
            set { bossBehaviour = value; }
        }
        // EOF Boss related

        float moveBoss = 0.0f;

        public List<Enemy> EnemiesList
        {
            get { return enemies; }
            set { enemies = value; }
        }

        ParticleSystem poisonEnemyParticles;

        SpriteBatch spriteBatch;

        ICastle castle;
        IPlayer player;
        private int spawnNewEnemy;
        float waveFinishedTimer = 3000;

        ParticleSystem bloodParticles;
        ParticleSystem killEnemyParticles;
        ParticleSystem ghostParticles;

        public void NextLevel()
        {
            waveFinishedTimer = 3000;
            spawnNewEnemy = 2000;
            this.enemies.Clear();
            this.spawnEnemyTypes.Clear();
        }

        public float WaveFinishedTimer
        {
            get { return waveFinishedTimer; }
            set { waveFinishedTimer = value; }
        }

        public List<Enemies> SpawnEnemyTypes
        {
            get { return spawnEnemyTypes; }
            set { spawnEnemyTypes = value; }
        }

        public void ResetAfterGameOver()
        {
            enemies.Clear();
        }

        public void GenerateEnemies()
        {
            // STATIC MODELS
            // ID = 0
            enemyModels.Add(new StaticModel(Game));
            enemyModels[0].Load("Models/blob");

            // ID = 1
            enemyModels.Add(new StaticModel(Game));
            enemyModels[1].Load("Models/blob_fat");

            // ID = 2
            enemyModels.Add(new StaticModel(Game));
            enemyModels[2].Load("Models/blob_boss");


            // ANIMATED MODELS
            // ID = 0
            animatedEnemyModels.Add(new AnimatedModel(Game));
            animatedEnemyModels[0].Initialize("Models/zombie");
            animatedEnemyModels[0].PlayAnimation("Start");

            // ID = 1
            animatedEnemyModels.Add(new AnimatedModel(Game));
            animatedEnemyModels[1].Initialize("Models/skeleton");
            animatedEnemyModels[1].PlayAnimation("Start");

            // ID = 2
            animatedEnemyModels.Add(new AnimatedModel(Game));
            animatedEnemyModels[2].Initialize("Models/ghost");
            animatedEnemyModels[2].PlayAnimation("Start");
        }

        public bool AllEnemiesDead
        {
            get
            {
                if (enemies.Count <= 0)
                {
                    return true;
                }
                return false;
            }
        }

        public void IsCollidingWithSpell(SpellHandler spellHandler)
        {
            IPlayer player = (IPlayer)Game.Services.GetService(typeof(IPlayer));

            foreach (Enemy e in enemies)
            {
                foreach (Spell s in spellHandler.SpellList)
                {
                    if (e.IsColliding(s))
                    {
                        if (s.Active)
                        {
                            // is player immune to the spells alignment?
                            bool isImune = false;
                            foreach (Spell.Alignments immune in e.ImuneAlignments)
                            {
                                if (immune == s.Alignment)
                                {
                                    isImune = true;
                                }
                            }

                            if (!isImune)
                            {
                                switch (s.Alignment)
                                {
                                    case Spell.Alignments.Damage:
                                        {
                                            e.Health -= Convert.ToInt32((s.BaseDamage * (player.Intelligence * 0.1f)));
                                            break;
                                        }
                                    case Spell.Alignments.Poison:
                                        {

                                            e.AlignmentsList.Add(Spell.Alignments.Poison);
                                            break;
                                        }
                                    case Spell.Alignments.Slow:
                                        {
                                            e.AlignmentsList.Add(Spell.Alignments.Slow);
                                            break;
                                        }
                                }
                            }

                            if (!s.DestoryOnHit)
                            {
                                s.BaseDamage = 0;
                                s.Active = false;
                            }
                        }

                        if (e.Health <= 0)
                        {
                            e.Active = false;
                            if (!s.DestoryOnHit)
                            {
                                s.Active = false;
                            }
                            player.Score += e.Score;
                            player.Xp += e.Xp;
                            player.Gold += e.Loot;
                        }
                    }
                }
            }
        }

        public void IsCollidingWithPlayer(Vector3 playerPos)
        {
            IPlayer player = (IPlayer)Game.Services.GetService(typeof(IPlayer));

            foreach (Enemy e in enemies)
            {
                if (e.IsColliding(playerPos))
                {
                    e.Active = false;
                    player.Health -= e.BaseDamage;
                }
            }
        }

        public void AddEnemy(Vector3 pos, Enemies e)
        {
            //pos.Y += 300;
            switch (e)
            {
                case Enemies.Slime:
                    {
                        Enemy slimeEnemy = new Enemy("Gooey1", false, 0, 10, 10, new Vector2(1, 1), 100, pos, AI.Forward, 10, 10, 100, 0);
                        enemies.Add(slimeEnemy);

                        break;
                    }
                case Enemies.Slime2:
                    {
                        Enemy slimeEnemy = new Enemy("Gooey2", false, 0, 10, 10, new Vector2(1, 3), 100, pos, AI.Sin, 15, 15, 200, 0);
                        enemies.Add(slimeEnemy);

                        break;
                    }
                case Enemies.Slime3:
                    {
                        Enemy slimeEnemy = new Enemy("Gooey3", false, 1, 20, 20, new Vector2(1, 2), 100, pos, AI.Forward, 20, 20, 300, 0);
                        enemies.Add(slimeEnemy);

                        break;
                    }
                case Enemies.Slime4:
                    {
                        Enemy slimeEnemy = new Enemy("Gooey4", false, 1, 30, 20, new Vector2(1, 2), 100, pos, AI.Sin, 25, 25, 400, 0);
                        enemies.Add(slimeEnemy);

                        break;
                    }
                case Enemies.Ghost:
                    {
                        Enemy slimeEnemy = new Enemy("Ghost", true, 2, 40, 60, new Vector2(2, 4), 100, pos, AI.Ghost, 40, 40, 400, 1);
                        enemies.Add(slimeEnemy);

                        break;
                    }
                case Enemies.Skeleton:
                    {
                        Enemy slimeEnemy = new Enemy("Skeleton", true, 1, 120, 40, new Vector2(1, 2), 100, pos, AI.Forward, 80, 50, 400, -1);
                        enemies.Add(slimeEnemy);

                        break;
                    }
                case Enemies.Zombie:
                    {
                        Enemy slimeEnemy = new Enemy("Zombie", true, 0, 160, 50, new Vector2(0.5f, 1), 100, pos, AI.Forward, 100, 60, 400, -1);
                        enemies.Add(slimeEnemy);

                        break;
                    }
                case Enemies.Boss:
                    {
                        Enemy boss1Enemy = new Enemy(bossBehaviour.BossName, bossBehaviour.Animated, bossBehaviour.ModelID, bossBehaviour.Hp, 10, new Vector2(0.2f, 0.2f), bossBehaviour.Radius, pos, AI.Boss, bossBehaviour.Loot, bossBehaviour.Xp, bossBehaviour.Score, 0);
                        enemies.Add(boss1Enemy);
                        break;
                    }
            }
            castle.EnemiesInCurrentWave--;
        }

        private void MoveEnemies(GameTime gameTime)
        {
            ICamera camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            int itemsToDelete = 0;

            foreach (Enemy s in enemies)
            {
                if (s.Active)
                {
                    switch (s.AI)
                    {
                        case AI.Forward:
                            {
                                s.PositionZ -= (s.Speed * (gameTime.ElapsedGameTime.Milliseconds)) / 4;
                                s.Update(camera.ViewMatrix, camera.ProjectionMatrix, gameTime);

                                break;
                            }
                        case AI.Sin:
                            {
                                s.PositionZ -= (s.Speed * (gameTime.ElapsedGameTime.Milliseconds)) / 4;
                                s.PositionX += (float)(Math.Sin(s.PositionZ / 1000) * s.Speed);
                                s.Update(camera.ViewMatrix, camera.ProjectionMatrix, gameTime);
                                break;
                            }
                        case AI.Ghost:
                            {
                                s.PositionZ -= (s.Speed * (gameTime.ElapsedGameTime.Milliseconds)) / 4;
                                s.PositionX += (float)(Math.Sin(s.PositionZ / 1000) * s.Speed);

                                // add particles to ghost
                                //for (int i = 0; i < 1; i++)
                                //{
                                //    ghostParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f + 1, s.PositionZ * 0.008f), Vector3.Zero);
                                //}

                                s.Update(camera.ViewMatrix, camera.ProjectionMatrix, gameTime);
                                break;
                            }
                        case AI.Boss:
                            {
                                float speedModifier = 1.0f;

                                if (s.PositionZ >= 4000)
                                {
                                    speedModifier = 4.0f;
                                }

                                moveBoss += (gameTime.ElapsedGameTime.Milliseconds) / 10;

                                
                                s.PositionZ -= (speedModifier) * ((s.Speed * (gameTime.ElapsedGameTime.Milliseconds)) / 4);
                                if (s.PositionZ <= 2000)
                                {
                                    s.PositionZ = 2000;
                                }

                                if (bossBehaviour.MoveStyle.Equals("Sin"))
                                {
                                    s.PositionX = (float)(Math.Sin((moveBoss / 100)) * 4 * 400);
                                }

                                if (bossBehaviour.SpitEnemies)
                                {
                                    if ((s.PositionX > (player.Position.X - 50)) && (s.PositionX < (player.Position.X + 50)))
                                    {
                                        addOneEnemyThisFrame = true;
                                        addEnemyAtPos = s.Position;

                                        if (bossBehaviour.SpitWhatEnemy == "Slime1")
                                        {
                                            enemyToAdd = Enemies.Slime;
                                        }
                                        if (bossBehaviour.SpitWhatEnemy == "Slime2")
                                        {
                                            enemyToAdd = Enemies.Slime2;
                                        }
                                        if (bossBehaviour.SpitWhatEnemy == "Slime3")
                                        {
                                            enemyToAdd = Enemies.Slime3;
                                        }
                                        if (bossBehaviour.SpitWhatEnemy == "Slime4")
                                        {
                                            enemyToAdd = Enemies.Slime4;
                                        }
                                        if (bossBehaviour.SpitWhatEnemy == "Skeleton")
                                        {
                                            enemyToAdd = Enemies.Skeleton;
                                        }
                                        if (bossBehaviour.SpitWhatEnemy == "Zombie")
                                        {
                                            enemyToAdd = Enemies.Zombie;
                                        }
                                        if (bossBehaviour.SpitWhatEnemy == "Ghost")
                                        {
                                            enemyToAdd = Enemies.Ghost;
                                        }
                                    }
                                }

                                s.Update(camera.ViewMatrix, camera.ProjectionMatrix, gameTime);
                                break;
                            }
                    }

                    if (s.Position.Z <= -1000)
                    {

                        castle.Upgrade -= 1;
                        for (int i = 0; i < 50; i++)
                        {
                            killEnemyParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Up);
                        }
                        s.Active = false;
                    }
                }
                else
                {
                    itemsToDelete++;
                }
            }

            while (itemsToDelete > 0)
            {
                int index = -1;
                foreach (Enemy s in enemies)
                {
                    index++;
                    if (!s.Active)
                    {
                        if (s.AI == AI.Boss)
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                // Create bigger explosion when killing a boss.
                                killEnemyParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), new Vector3(0, 10, 0));
                                bloodParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                killEnemyParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                                bloodParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f + 2, s.PositionZ * 0.008f), Vector3.Zero);
                            }
                        }

                        collideCue = soundBank.GetCue("collide");
                        if (!collideCue.IsPlaying)
                        {
                            collideCue.Play();
                        }

                        break;
                    }
                }
                enemies.RemoveRange(index, 1);
                itemsToDelete--;
            }
        }


        public void ApplyAlignments()
        {
            foreach (Enemy e in enemies)
            {
                // Apply all alignments on enemy
                foreach (Spell.Alignments a in e.AlignmentsList)
                {
                    if (a == Spell.Alignments.Slow)
                    {
                        e.Speed *= 0.5f;
                    }
                    else if (a == Spell.Alignments.Poison)
                    {
                        e.IsDOT = true;
                    }
                }

                // is enemy under a Damage over time effect?
                if (e.IsDOT)
                {
                    e.Health -= (player.Intelligence * 0.1f) * 0.1f;
                }

                // is player dead after DOT?
                if (e.Health <= 0)
                {
                    e.Active = false;
                    player.Score += e.Score;
                    player.Xp += e.Xp;
                    player.Gold += e.Loot;
                }

                // All alignments are added, clear list
                e.AlignmentsList.Clear();
            }
        }

        public EnemyHandler(Game game)
            : base(game)
        {
            enemyModels = new List<StaticModel>();
            animatedEnemyModels = new List<AnimatedModel>();
            enemies = new List<Enemy>();
            spawnEnemyTypes = new List<Enemies>();

            killEnemyParticles = new ExplosionParticleSystem(game, game.Content);
            bloodParticles = new BloodParticleSystem(game, game.Content);
            ghostParticles = new GhostParticleSystem(game, game.Content);
            poisonEnemyParticles = new PoisonParticleSystem(game, game.Content);

            spawnNewEnemy = 1000;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            foreach (StaticModel e in enemyModels)
            {
                e.Initialize();
            }

            foreach (AnimatedModel e in animatedEnemyModels)
            {
                e.Initialize();
            }

            //SpawnEnemyTypes.Add(Enemies.Slime);

            killEnemyParticles.Initialize();
            bloodParticles.Initialize();
            ghostParticles.Initialize();
            poisonEnemyParticles.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            engine = new AudioEngine("Content\\music.xgs");
            soundBank = new SoundBank(engine, "Content\\SoundBank.xsb");
            waveBank = new WaveBank(engine, "Content\\WaveBank.xwb");

            collideCue = soundBank.GetCue("collide");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            GenerateEnemies();

            base.LoadContent();
        }

        void SpawnEnemy(GameTime gameTime)
        {
            spawnNewEnemy -= gameTime.ElapsedGameTime.Milliseconds;
            spitTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (addOneEnemyThisFrame && spitTimer <= 0)
            {
                spitTimer = 500;
                addOneEnemyThisFrame = false;
                castle.EnemiesInCurrentWave++;
                this.AddEnemy(new Vector3(addEnemyAtPos.X, 0, addEnemyAtPos.Z), enemyToAdd);
            }
            addOneEnemyThisFrame = false;

            if (AllEnemiesDead)
            {
                waveFinishedTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if (spawnNewEnemy < 0 && castle.EnemiesInCurrentWave > 0)
            {
                Random rndEnemyFromList = new Random(gameTime.TotalGameTime.Milliseconds);
                int currentEnemyToSpawn = rndEnemyFromList.Next(0, SpawnEnemyTypes.Count);

                Random rndSpawnPosition = new Random(gameTime.TotalGameTime.Milliseconds);
                this.AddEnemy(new Vector3(rndSpawnPosition.Next(-1500, 1500), 0, 9000), SpawnEnemyTypes[currentEnemyToSpawn]);

                Random spawnNewEnemyR = new Random();

                spawnNewEnemy = spawnNewEnemyR.Next(800 - ((castle.Level) * 20), 3000 - ((castle.Level) * 20));
                if (spawnNewEnemy <= 100)
                    spawnNewEnemy = 100;
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            engine.Update();

            castle = (ICastle)Game.Services.GetService(typeof(ICastle));
            player = (IPlayer)Game.Services.GetService(typeof(IPlayer));
            // TODO: Add your update code here

            SpawnEnemy(gameTime);
            ApplyAlignments();
            MoveEnemies(gameTime);


            ICamera camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            killEnemyParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            bloodParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            ghostParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            poisonEnemyParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);

            // Add particles
            foreach (Enemy e in enemies)
            {
                if (e.IsDOT)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        poisonEnemyParticles.AddParticle(new Vector3(e.PositionX * 0.008f, e.PositionY * 0.008f + 2, e.PositionZ * 0.008f + 2), Vector3.Zero);
                    }
                }

                switch (e.ParticleSystemId)
                {
                    case 0:
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            poisonEnemyParticles.AddParticle(new Vector3(e.PositionX * 0.008f, e.PositionY * 0.008f + 2, e.PositionZ * 0.008f + 2), Vector3.Zero);
                        }
                        break;
                    }
                    case 1:
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            ghostParticles.AddParticle(new Vector3(e.PositionX * 0.008f, e.PositionY * 0.008f + 1, e.PositionZ * 0.008f), Vector3.Zero);
                        }
                        break;
                    }
                    default:
                    {
                        // None
                        break;
                    }
                }
            }

            killEnemyParticles.Update(gameTime);
            bloodParticles.Update(gameTime);
            ghostParticles.Update(gameTime);
            poisonEnemyParticles.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Enemy s in enemies)
            {
                if (s.Active)
                {
                    if (s.Animated)
                    {
                        animatedEnemyModels[s.ModelID].Update(gameTime, s.Render, s.View, s.Projection);
                        animatedEnemyModels[s.ModelID].Draw(gameTime);
                    }
                    else
                    {
                        enemyModels[s.ModelID].Update(gameTime, s.Render, s.View, s.Projection);
                        enemyModels[s.ModelID].Draw(gameTime);
                    }
                }
            }

            spriteBatch.Begin();
            killEnemyParticles.Draw(gameTime);
            bloodParticles.Draw(gameTime);
            ghostParticles.Draw(gameTime);
            poisonEnemyParticles.Draw(gameTime);
            spriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            base.Draw(gameTime);
        }


    }
}
