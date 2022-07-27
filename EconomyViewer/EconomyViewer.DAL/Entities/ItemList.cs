using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EconomyViewer.DAL.Entities;
public class ItemList : ICollection<Item>
{
    private readonly List<Item> _items;

    public Item this[int index] {
        get => _items[index];
        set => _items[index] = value;
    }
    public ItemList()
    {
        _items = new List<Item>();
    }

    public int Count => _items.Count;
    public bool IsReadOnly => false;

    public void Add(Item item)
    {
        var same = _items.FirstOrDefault(i => i.Equals(item));

        if (same == null)
            _items.Add((Item)item.Clone());
        else
            same.Count += item.Count;
    }

    public void Clear()
    {
        _items.Clear();
    }

    public bool Contains(Item item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(Item[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Item> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public bool Remove(Item item)
    {
        var same = _items.FirstOrDefault(i => i.Equals(item));
        if (same == null)
            throw new ArgumentException("Collection does not contains element.");

        if (same.Count == 1 || same.Count == item.Count)
            return _items.Remove(same);

        if (same.Count > item.Count)
        {
            same.Count -= item.Count;
            return true;
        }
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
