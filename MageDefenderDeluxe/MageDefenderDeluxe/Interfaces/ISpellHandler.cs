using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MageDefenderDeluxe.GameObjects;

namespace MageDefenderDeluxe.Interfaces
{
    interface ISpellHandler
    {
        System.Collections.Generic.List<GameObjects.SpellHandler.Spells> LearnedSpells { get; set; }
        void LearnSpell(GameObjects.SpellHandler.Spells learn);
        int NumberOfSpells { get; set; }
        GameObjects.SpellHandler.Spells SelectedSpell { get; set; }
        int SelectedSpellIndex { get; set; }
        System.Collections.Generic.List<Spell> SpellList { get; set; }
        System.Collections.Generic.List<Spell> SpellReference { get; set; }
        int GetSpellPrice(GameObjects.SpellHandler.Spells spellType);
        string GetSpellDescription(GameObjects.SpellHandler.Spells spellType);
    }
}
