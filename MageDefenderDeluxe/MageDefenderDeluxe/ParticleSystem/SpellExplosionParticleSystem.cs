#region File Description
//-----------------------------------------------------------------------------
// ExplosionParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MageDefenderDeluxe.ParticleEngine
{
    /// <summary>
    /// Custom particle system for creating the fiery part of the explosions.
    /// </summary>
    class SpellExplosionParticleSystem : ParticleSystem
    {
        public SpellExplosionParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures\\destroySpell";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromMilliseconds(600);
            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 5;
            settings.MaxHorizontalVelocity = 10;

            settings.MinVerticalVelocity = -5;
            settings.MaxVerticalVelocity = 5;

            settings.EndVelocity = 0;

            //settings.MinColor = Color.DarkGray;
            //settings.MaxColor = Color.Gray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 1;
            settings.MaxStartSize = 1;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 4;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
