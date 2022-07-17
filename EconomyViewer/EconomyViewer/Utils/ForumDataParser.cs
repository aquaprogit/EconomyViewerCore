using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace EconomyViewer.Utils;

internal class ForumDataParser
{
    public static Dictionary<string, string> GetServerNamesToLinks()
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument serversPage = new HtmlDocument();
        serversPage.LoadHtml(web.Load(@"https://f.simpleminecraft.ru/index.php?/forum/49-jekonomika/").Text);
        List<HtmlNode>? links = serversPage.DocumentNode.SelectNodes("//a[@href]")
            .Where(node => {
                string innerText = node.InnerText.Trim();
                return innerText.StartsWith("Экономика") && innerText.Length > "Экономика".Length + 1;
            })
            .ToList();

        return new Dictionary<string, string>(links
            .Select(link => new KeyValuePair<string, string>(link.InnerText.Trim().Split()[1], link.Attributes["href"].Value)));
    }
}
