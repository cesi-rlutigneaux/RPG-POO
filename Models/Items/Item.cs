using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Items
{
    public abstract class Item
    {
        public string Name { get; }

        protected Item(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}
