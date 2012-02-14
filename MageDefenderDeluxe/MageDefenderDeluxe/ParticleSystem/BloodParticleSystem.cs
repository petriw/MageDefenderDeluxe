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
    class BloodParticleSystem : ParticleSystem
    {
        public BloodParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures\\particle";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromMilliseconds(2000);

            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity =-1;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = -1.5f;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, 0, 0);

            settings.MinColor = new Color(255, 0, 0, 255);
            settings.MaxColor = new Color(255, 0, 0, 255);

            settings.MinStartSize = 1;
            settings.MaxStartSize = 2;

            settings.MinEndSize = 2;
            settings.MaxEndSize = 3;

            //// Use additive blending.
            //settings.SourceBlend = Blend.SourceAlpha;
            //settings.DestinationBlend = Blend.InverseSourceAlpha;
        }
    }
}
