#region File Description
//-----------------------------------------------------------------------------
// FireParticleSystem.cs
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
    /// Custom particle system for creating a flame effect.
    /// </summary>
    class FireParticleSystem : ParticleSystem
    {
        public FireParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures\\fire";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromMilliseconds(1000);

            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = 0.5f;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, 1, -5);

            settings.MinColor = new Color(255, 128, 128, 10);
            settings.MaxColor = new Color(255, 255, 255, 40);

            settings.MinStartSize = 1;
            settings.MaxStartSize = 2;

            settings.MinEndSize = 2;
            settings.MaxEndSize = 5;

            //// Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
