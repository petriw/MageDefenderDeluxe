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


namespace MageDefenderDeluxe.LevelSettings
{
    public class Level
    {
        public int LevelID = 1;
        public string SpawnEnemies = "Slime1";
        public int NumEnemies = 10;
        public bool IsBoss = false;
        public string BossCodeName = "GooeyKing";
    }
}
