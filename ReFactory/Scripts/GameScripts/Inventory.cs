using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ReFactory.GameScripts;

public static class Inventory
{
    static Inventory()
    {
        items.AddRange(Enumerable.Repeat<Item>( null, 8));
    }
    public static List<Item> items = new ();

    public static bool AddItem(Item parsedItem)
    {
        var item = parsedItem;
        bool SlotCondition(Item i) => i != null && i.Reference == item.Reference && i.count < i.Reference.maxStack;
        while (items.FindIndex(SlotCondition) is var index && index != -1)
        {
            items[index].count++;
            item.count--;
            if (item.count <= 0) return true;
        }
        while(items.FindIndex(i => i == null) is var index && index != -1)
        {
            int parsedCount = Math.Min(item.Reference.maxStack, item.count);
            items[items.FindIndex(i => i == null)] = new Item(item.Reference, parsedCount);
            item.count -= parsedCount;
            if (item.count <= 0)
                return true;
        }

        if (items.Count == 8) return false;
        items.Add(item);
        return true;
    }
}