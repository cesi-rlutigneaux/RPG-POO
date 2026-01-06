using RPG_POO.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Interfaces
{
    public interface ILootable
    {
        IReadOnlyList<Item> DropTable { get; }
    }
}
