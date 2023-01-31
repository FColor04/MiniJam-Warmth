using System.Linq;
using Microsoft.Xna.Framework;

namespace ReFactory.GameScripts;

public class Item
{
    public readonly string Name;
    public int Count;
    
    public Item(string name, int count)
    {
        Name = name;
        Count = count;
    }

    public Item(ItemReference reference, int count = 1)
    {
        Name = reference.name;
        Count = count;
    }

    public ItemReference Reference => ItemList.Items.FirstOrDefault(item => item.name == Name);
}