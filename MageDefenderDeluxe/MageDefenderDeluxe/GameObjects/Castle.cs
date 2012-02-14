using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MageDefenderDeluxe.Interfaces;

namespace MageDefenderDeluxe.GameObjects
{
    public class Castle : ICastle
    {
        int storyStage;

        public int StoryStage
        {
            get { return storyStage; }
            set { storyStage = value; }
        }

        int upgrade;

        public int Upgrade
        {
            get { return upgrade; }
            set { upgrade = value; }
        }

        int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        int enemiesInCurrentWave;

        public int EnemiesInCurrentWave
        {
            get { return enemiesInCurrentWave; }
            set { enemiesInCurrentWave = value; }
        }

        public Castle()
        {
            upgrade = 1;
            level = 0;
            enemiesInCurrentWave = 3;
            storyStage = 1;
        }
    }
}
