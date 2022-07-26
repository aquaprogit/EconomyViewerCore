﻿using System;
using System.Text.RegularExpressions;

namespace EconomyViewer.Model;
public class Item : ICloneable
{
    private static readonly Regex _itemPattern = new Regex(@"(?<Header>.+)\s(?<Count>[0-9]+) шт. - (?<Price>[0-9]+)$");
    private static readonly Regex _singleItemPattent = new Regex(@"(?<Header>.+)\W+(?<Price>\d+)");
    private int _count;

    public int ID { get; init; }
    public string Header { get; init; } = string.Empty;
    public int Count {
        get => _count;
        set {
            int pricePerOne = PriceForOne;
            _count = value;
            Price = pricePerOne * Count;
        }
    }
    public int Price { get; private set; }
    public int PriceForOne => Price / Count;
    public string Mod { get; init; } = string.Empty;
    public string? StringFormat => ToString();

    public Item() { }
    public Item(string header, int count, int price, string mod, int id = 0)
    {
        ID = id;
        Header = header ?? throw new ArgumentNullException(nameof(header));
        Count = count;
        Price = price;
        Mod = mod ?? throw new ArgumentNullException(nameof(mod));
    }

    public override bool Equals(object? obj)
    {
        return obj is Item item &&
               Header == item.Header &&
               PriceForOne == item.PriceForOne &&
               Mod == item.Mod;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Header, Count, Price, Mod);
    }
    public static Item? FromString(string value, string mod)
    {
        if (_itemPattern.IsMatch(value) == false && _singleItemPattent.IsMatch(value) == false)
            return null;
        var pattern = _itemPattern.IsMatch(value) ? _itemPattern : _singleItemPattent;
        var groups = pattern.Match(value).Groups;
        string header = groups["Header"].Value.TrimStart(' ', '\t');
        int count = _itemPattern.IsMatch(value) ? Convert.ToInt32(groups["Count"].Value) : 1;
        int price = Convert.ToInt32(groups["Price"].Value);
        return new Item(header, count, price, mod);
    }
    public void IncreaseCount(int value)
    {

    }
    public void DecreaseCount(int value)
    {
        if (value < 0)
            throw new InvalidOperationException("Value can not be less than zero.");
        if (value >= Count)
            throw new InvalidOperationException("Value can not be greater than source item count");

        int pricePerOne = PriceForOne;
        Count -= value;
        Price = pricePerOne * Count;
    }
    public override string? ToString()
    {
        return $"{Header} {Count} шт. - {Price}";
    }

    public object Clone()
    {
        return new Item(Header, Count, Price, Mod, ID);
    }
}
