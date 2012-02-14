using System;
using Microsoft.Xna.Framework;
using MageDefenderDeluxe.GameObjects;

namespace MageDefenderDeluxe.Interfaces
{
    interface IPlayer
    {
        bool PlayerIndexSet { get; set; }
        PlayerIndex PlayerIndexSaved { get; set; }
        Vector3 Position { get; set; }

        int Ap { get; set; }
        int Constitution { get; set; }
        int Gold { get; set; }
        int Health { get; set; }
        int HealthPotions { get; set; }
        int Intelligence { get; set; }
        int Level { get; set; }
        float Mana { get; set; }
        int ManaPotions { get; set; }
        int MaxHealth { get; }
        int MaxMana { get; }
        int Score { get; set; }
        int Strength { get; set; }
        int Wisdom { get; set; }
        int Xp { get; set; }
        int XpToNextLevel { get; set; }
        int Agility { get; set; }
        float DisplayLevelUpTimer { get; set; }

        int AtGameLevel { get; set; }
    }
}
