using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MageDefenderDeluxe.Interfaces
{
    interface ICastle
    {
        int StoryStage { get; set; }
        int Level { get; set; }
        int Upgrade { get; set; }
        int EnemiesInCurrentWave { get; set; }
    }
}
