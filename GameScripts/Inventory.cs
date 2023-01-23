using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MiniJam_Warmth.GameScripts;

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
        bool SlotCondition(Item i) => i != null && i.Reference == item.Reference && i.Count < i.Reference.maxStack;
        while (items.FindIndex(SlotCondition) is var index && index != -1)
        {
            items[index].Count++;
            item.Count--;
            if (item.Count <= 0) return true;
        }
        while(items.FindIndex(i => i == null) is var index && index != -1)
        {
            int parsedCount = Math.Min(item.Reference.maxStack, item.Count);
            items[items.FindIndex(i => i == null)] = new Item(item.Reference, parsedCount);
            item.Count -= parsedCount;
            if (item.Count <= 0)
                return true;
        }

        if (items.Count == 8) return false;
        items.Add(item);
        return true;
    }
}