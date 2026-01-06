using RPG_POO.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public sealed class Inventory
    {
        private readonly List<Item> _items = new();

        public IReadOnlyList<Item> Items => _items;

        public void Add(Item item) => _items.Add(item);

        public bool Remove(Item item) => _items.Remove(item);
    }
}
