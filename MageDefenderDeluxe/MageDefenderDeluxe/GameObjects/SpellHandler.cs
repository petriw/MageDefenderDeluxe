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


namespace MageDefenderDeluxe.GameObjects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpellHandler : Microsoft.Xna.Framework.DrawableGameComponent, ISpellHandler
    {
        public enum Spells { Fireball = 1, Slowball = 2, PoisonBall, MagicMissile, EnergyBall, MagicMissileX2, MagicMissileX4 };
        
        ParticleSystem killEnemyParticles;
        ParticleSystem fireBallParticles;
        ParticleSystem slowEnemyParticles;
        ParticleSystem poisonEnemyParticles;
        ParticleSystem magicMissilePartilces;

        SpriteBatch spriteBatch;

        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;
        Cue collideCue;
        Cue shootCue;

        List<Spell> spells;

        public List<Spell> SpellList
        {
            get { return spells; }
            set { spells = value; }
        }
        List<Spell> spellReference;

        public List<Spell> SpellReference
        {
            get { return spellReference; }
            set { spellReference = value; }
        }
        List<Spells> learnedSpells;

        public List<Spells> LearnedSpells
        {
            get { return learnedSpells; }
            set { learnedSpells = value; }
        }

        List<StaticModel> staticSpellModels;

        int selectedSpellIndex = 0;

        public int SelectedSpellIndex
        {
            get { return selectedSpellIndex; }
            set { selectedSpellIndex = value; }
        }
        GameObjects.SpellHandler.Spells selectedSpell = Spells.Fireball;
        int numberOfSpells = 0;

        public int NumberOfSpells
        {
            get { return numberOfSpells; }
            set { numberOfSpells = value; }
        }

        public GameObjects.SpellHandler.Spells SelectedSpell
        {
            get { return selectedSpell; }
            set { selectedSpell = value; }
        }

        public void LearnSpell(SpellHandler.Spells learn)
        {
            learnedSpells.Add(learn);
        }

        public int GetManaCost(Spells spellType)
        {
            int returnValue = 0;
            foreach (Spell s in spellReference)
            {
                if (s.Type == spellType)
                {
                    returnValue = s.Mana;
                }
            }
            return returnValue;
        }

        public void NextLevel()
        {
            this.spells.Clear();
        }

        public SpellHandler(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            spells = new List<Spell>();
            staticSpellModels = new List<StaticModel>();
            spellReference = new List<Spell>();
            learnedSpells = new List<Spells>();

            killEnemyParticles = new SpellExplosionParticleSystem(game, game.Content);
            fireBallParticles = new FireParticleSystem(game, game.Content);
            slowEnemyParticles = new SlowParticleSystem(game, game.Content);
            poisonEnemyParticles = new PoisonParticleSystem(game, game.Content);
            magicMissilePartilces = new MagicMissileParticleSystem(game, game.Content);

        }

        public void LoadSpells()
        {
            spellReference.Add(new Spell(Spells.Fireball, Spell.Alignments.Damage, 0, 1, 10, 1, 3, false, 100, new Vector3(0), 0, "Deals damage|Starter spell", Vector3.Zero));
            spellReference.Add(new Spell(Spells.Slowball, Spell.Alignments.Slow, 1, 3, 20, 2, 2, false, 100, new Vector3(0), 1000, "Slows the enemy by|50 % pr. hit", Vector3.Zero));
            spellReference.Add(new Spell(Spells.PoisonBall, Spell.Alignments.Poison, 2, 5, 30, 3, 3, false, 100, new Vector3(0), 2000, "Poisons the enemy.|Damage over time", Vector3.Zero));
            spellReference.Add(new Spell(Spells.MagicMissile, Spell.Alignments.Damage, 3, 10, 40, 6, 4, false, 100, new Vector3(0), 3000, "Moderate damage.|Homing: Random enemy", Vector3.Zero));
            spellReference.Add(new Spell(Spells.EnergyBall, Spell.Alignments.Damage, 3, 20, 40, 8, 0.6f, true, 200, new Vector3(0), 4000, "Damage while touching.|Controllable: R-stick", Vector3.Zero));
            spellReference.Add(new Spell(Spells.MagicMissileX2, Spell.Alignments.Damage, 3, 10, 20, 10, 4, false, 100, new Vector3(0), 5000, "2 Magic Missiles.|Homing: Random enemy", Vector3.Zero));
            spellReference.Add(new Spell(Spells.MagicMissileX4, Spell.Alignments.Damage, 3, 10, 10, 12, 4, false, 100, new Vector3(0), 6000, "4 Magic Missiles.|Homing: Random enemy", Vector3.Zero));

            learnedSpells.Add(Spells.Fireball);
            //learnedSpells.Add(Spells.Slowball);
            //learnedSpells.Add(Spells.PoisonBall);
            //learnedSpells.Add(Spells.MagicMissile);
            //learnedSpells.Add(Spells.EnergyBall);
            //learnedSpells.Add(Spells.MagicMissileX2);
            //learnedSpells.Add(Spells.MagicMissileX4);
        }

        public void AddSpell(Vector3 pos, Spells s, Vector3 target)
        {
            pos.Y += 300;
            pos.Z += 100;

            switch (s)
            {
                case Spells.Fireball:
                {
                    spells.Add(new Spell(Spells.Fireball, Spell.Alignments.Damage, 0, 1, 10, 1, 3, false, 100, pos, 0, "Deals damage|Starter spell", target));
                    break;
                }
                case Spells.Slowball:
                {
                    spells.Add(new Spell(Spells.Slowball, Spell.Alignments.Slow, 0, 3, 20, 2, 2, false, 100, pos, 1000, "Slows the enemy by|50 % pr. hit", target));
                    break;
                }
                case Spells.PoisonBall:
                {
                    spells.Add(new Spell(Spells.PoisonBall, Spell.Alignments.Poison, 0, 5, 30, 3, 3, false, 100, pos, 2000, "Poisons the enemy.|Damage over time", target));
                    break;
                }
                case Spells.MagicMissile:
                {
                    spells.Add(new Spell(Spells.MagicMissile, Spell.Alignments.Damage, 0, 10, 40, 6, 4, false, 100, pos, 3000, "Moderate damage.|Homing: Random enemy", target));
                    break;
                }
                case Spells.EnergyBall:
                {
                    spells.Add(new Spell(Spells.EnergyBall, Spell.Alignments.Damage, 0, 20, 40, 8, 0.6f, true, 200, pos, 4000, "Damage while touching.|Controllable: R-stick", target));
                    break;
                }
                case Spells.MagicMissileX2:
                {
                    spells.Add(new Spell(Spells.MagicMissileX2, Spell.Alignments.Damage, 0, 10, 20, 10, 4, false, 100, pos, 5000, "2 Magic Missiles.|Homing: Random enemy", Vector3.Zero));
                    break;
                }
                case Spells.MagicMissileX4:
                {
                    spells.Add(new Spell(Spells.MagicMissileX4, Spell.Alignments.Damage, 0, 10, 10, 12, 4, false, 100, pos, 6000, "4 Magic Missiles.|Homing: Random enemy", Vector3.Zero));
                    break;
                }
            }

            shootCue = soundBank.GetCue("shoot");
            if (!shootCue.IsPlaying)
            {
                shootCue.Play();
            }
        }

        public int GetSpellPrice(Spells spellType)
        {
            int returnValue = 0;
            foreach (Spell s in spellReference)
            {
                if (s.Type == spellType)
                {
                    returnValue = s.Price;
                }
            }
            return returnValue;
        }

        public string GetSpellDescription(Spells spellType)
        {
            string returnValue = "";
            foreach (Spell s in spellReference)
            {
                if (s.Type == spellType)
                {
                    returnValue = s.Description;
                }
            }
            return returnValue;
        }

        public void ResetAfterGameOver()
        {
            spells.Clear();

            learnedSpells.Clear();
            learnedSpells.Add(Spells.Fireball);
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            foreach (StaticModel s in staticSpellModels)
            {
                s.Initialize();
            }

            killEnemyParticles.Initialize();
            fireBallParticles.Initialize();
            slowEnemyParticles.Initialize();
            poisonEnemyParticles.Initialize();
            magicMissilePartilces.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadSpells();
            staticSpellModels.Add(new StaticModel(Game));
            staticSpellModels[0].Load("Models/fireball");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            engine = new AudioEngine("Content\\music.xgs");
            soundBank = new SoundBank(engine, "Content\\SoundBank.xsb");
            waveBank = new WaveBank(engine, "Content\\WaveBank.xwb");

            collideCue = soundBank.GetCue("collide");
            shootCue = soundBank.GetCue("shoot");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, float thumbSticksRightX)
        {
            engine.Update();
            ICamera camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            int itemsToDelete = 0;
            foreach (Spell s in spells)
            {
                if (s.Active)
                {
                    s.Update(camera.ViewMatrix, camera.ProjectionMatrix, gameTime, thumbSticksRightX);
                }
                else
                {
                    itemsToDelete++;
                }
            }


            while (itemsToDelete > 0)
            {
                int index = -1;
                foreach (Spell s in spells)
                {
                    index++;
                    if (!s.Active)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            collideCue = soundBank.GetCue("sizzle");
                            if (!collideCue.IsPlaying)
                            {
                                collideCue.Play();
                            }
                            killEnemyParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                        }
                        break;
                    }
                }
                spells.RemoveRange(index, 1);
                itemsToDelete--;
            }

            killEnemyParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            fireBallParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            slowEnemyParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            poisonEnemyParticles.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);
            magicMissilePartilces.SetCamera(camera.ViewMatrix, camera.ProjectionMatrix);

            foreach (Spell s in SpellList)
            {
                if (s.Type == GameObjects.SpellHandler.Spells.Fireball)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        fireBallParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                    }
                }

                if (s.Type == GameObjects.SpellHandler.Spells.Slowball)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        slowEnemyParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                    }
                }

                if (s.Type == GameObjects.SpellHandler.Spells.PoisonBall)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        poisonEnemyParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                    }
                }

                if (s.Type == GameObjects.SpellHandler.Spells.MagicMissile)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        magicMissilePartilces.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                    }
                }

                if (s.Type == GameObjects.SpellHandler.Spells.EnergyBall)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        magicMissilePartilces.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                        fireBallParticles.AddParticle(new Vector3(s.PositionX * 0.008f, s.PositionY * 0.008f, s.PositionZ * 0.008f), Vector3.Zero);
                    }
                }
            }

            killEnemyParticles.Update(gameTime);
            poisonEnemyParticles.Update(gameTime);
            slowEnemyParticles.Update(gameTime);
            fireBallParticles.Update(gameTime);
            magicMissilePartilces.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Spell s in spells)
            {
                staticSpellModels[s.ModelId].Update(gameTime, s.Render, s.View, s.Projection);
                staticSpellModels[s.ModelId].Draw(gameTime);
            }

            spriteBatch.Begin();
            killEnemyParticles.Draw(gameTime);
            slowEnemyParticles.Draw(gameTime);
            poisonEnemyParticles.Draw(gameTime);
            fireBallParticles.Draw(gameTime);
            magicMissilePartilces.Draw(gameTime);
            spriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            base.Draw(gameTime);
        }
    }
}
