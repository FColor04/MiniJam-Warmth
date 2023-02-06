using System.Linq;
using Microsoft.Xna.Framework;

namespace ReFactory.GameScripts;

public class Item
{
    public readonly string name;
    public int count;
    
    public Item(string name, int count)
    {
        this.name = name;
        this.count = count;
    }

    public Item(ItemReference reference, int count = 1)
    {
        name = reference.name;
        this.count = count;
    }

    public ItemReference Reference => ItemList.Items.FirstOrDefault(item => item.name == name);
}