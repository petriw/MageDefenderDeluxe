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
    class PressStartParticleSystem : ParticleSystem
    {
        public PressStartParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures\\particle";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromMilliseconds(8000);

            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 0.3f;
            settings.MaxHorizontalVelocity = -0.3f;

            settings.MinVerticalVelocity = 0.0f;
            settings.MaxVerticalVelocity = -5.0f;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, -2, 0);

            settings.MinColor = new Color(0, 0, 255, 0);
            settings.MaxColor = new Color(0, 255, 255, 255);

            settings.MinStartSize = 1f;
            settings.MaxStartSize = 1f;

            settings.MinEndSize = 10;
            settings.MaxEndSize = 10;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
