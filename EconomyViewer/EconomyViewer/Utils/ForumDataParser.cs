using System.Collections.Generic;
using System.Linq;

using EconomyViewer.Model;

using HtmlAgilityPack;

namespace EconomyViewer.Utils;

internal static class ForumDataParser
{
    private static Dictionary<string, string>? _serverToLink;
    public static Dictionary<string, string> GetServerNamesToLinks()
    {
        if (_serverToLink != null)
            return _serverToLink;

        HtmlWeb web = new HtmlWeb();
        HtmlDocument serversPage = new HtmlDocument();
        serversPage.LoadHtml(web.Load(@"https://f.simpleminecraft.ru/index.php?/forum/49-jekonomika/").Text);
        List<HtmlNode>? links = serversPage.DocumentNode.SelectNodes("//a[@href]")
            .Where(node => {
                string innerText = node.InnerText.Trim();
                return innerText.StartsWith("Экономика") && innerText.Length > "Экономика".Length + 1;
            })
            .ToList();

        _serverToLink =
            new Dictionary<string, string>(links.Select(link =>
                new KeyValuePair<string, string>(link.InnerText.Trim().Split()[1], link.Attributes["href"].Value)));

        return _serverToLink;
    }
    public static List<Item> GetServerItemList(string serverName)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument page = new HtmlDocument();
        page.LoadHtml(web.Load(GetServerNamesToLinks()[serverName]).Text);

        List<Item> result = new List<Item>();

        var post = page.DocumentNode.SelectSingleNode(@"//div[@data-role=""commentContent""]");
        var lines = post.InnerText.Split("\n").Select(l => l.Trim()).Where(l => l.Length > 0);

        string currentMod = "";
        foreach (string line in lines)
        {
            Item? item = Item.FromString(line, currentMod);
            if (item == null)
                currentMod = line.TrimEnd(':', '*').Replace("&amp; ", "");
            else
                result.Add(item);
        }
        return result;
    }
}
