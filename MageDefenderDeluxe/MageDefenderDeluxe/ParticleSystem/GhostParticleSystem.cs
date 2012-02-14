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
    class GhostParticleSystem : ParticleSystem
    {
        public GhostParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures\\destroySpell";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromMilliseconds(2000);

            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = -0.5f;
            settings.MaxHorizontalVelocity = 0.5f;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = 0.5f;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, 0, 0);

            settings.MinColor = new Color(0, 0, 255, 100);
            settings.MaxColor = new Color(255, 255, 255, 170);

            settings.MinStartSize = 5;
            settings.MaxStartSize = 5;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 1;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
