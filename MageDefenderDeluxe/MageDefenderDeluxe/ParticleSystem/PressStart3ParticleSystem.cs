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
    class PressStart3ParticleSystem : ParticleSystem
    {
        public PressStart3ParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures\\blue_energy";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromMilliseconds(1000);
            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 5;
            settings.MaxHorizontalVelocity = 5;

            settings.MinVerticalVelocity = -5;
            settings.MaxVerticalVelocity = 15;

            settings.EndVelocity = 0;

            settings.Gravity = new Vector3(0, -5, 0);

            settings.MinColor = new Color(0, 0, 255, 0);
            settings.MaxColor = new Color(255, 255, 255, 255);

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 1;
            settings.MaxStartSize = 1;

            settings.MinEndSize = 2;
            settings.MaxEndSize = 3;

            settings.BlendState = BlendState.Additive;
        }
    }
}
