﻿using System;
using System.Collections.Generic;
using System.Linq;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units;
using Xunit;

namespace AAEmu.UnitTests.Game.Models.Game.Units;

public class UnitTests
{
    [Fact]
    public void NoDuplicateAttributes()
    {
        //This tests to make sure no Attribute is attached to more than one property
        var unit = new Unit();

        foreach (var attr in Enum.GetValues<UnitAttribute>())
        {
            var props = unit.GetType().GetProperties()
            .Where(o => (o.GetCustomAttributes(typeof(UnitAttributeAttribute), true) as IEnumerable<UnitAttributeAttribute>)
                .Any(a => a.Attributes.Contains(attr)));

            Assert.True(props.Count() <= 1, $"{attr} is bound to multiple unit properties.");
        }

        unit = new Character(new UnitCustomModelParams());
        foreach (var attr in Enum.GetValues<UnitAttribute>())
        {
            var props = unit.GetType().GetProperties()
            .Where(o => (o.GetCustomAttributes(typeof(UnitAttributeAttribute), true) as IEnumerable<UnitAttributeAttribute>)
                .Any(a => a.Attributes.Contains(attr)));

            Assert.True(props.Count() <= 1, $"{attr} is bound to multiple unit properties.");
        }

        unit = new Npc();
        foreach (var attr in Enum.GetValues<UnitAttribute>())
        {
            var props = unit.GetType().GetProperties()
            .Where(o => (o.GetCustomAttributes(typeof(UnitAttributeAttribute), true) as IEnumerable<UnitAttributeAttribute>)
                .Any(a => a.Attributes.Contains(attr)));

            Assert.True(props.Count() <= 1, $"{attr} is bound to multiple unit properties.");
        }
    }
}
