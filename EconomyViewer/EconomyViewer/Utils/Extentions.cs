using EconomyViewer.DB;
using EconomyViewer.Model;

namespace EconomyViewer.Utils
{
    internal static class Extentions
    {
        public static ItemDto AsDto(this Item self, string serverName)
        {
            return new ItemDto() {
                ID = self.ID,
                Header = self.Header,
                Count = self.Count,
                Price = self.Price,
                Mod = self.Mod,
                ServerName = serverName
            };
        }
        public static (Item, string serverName) FromDto(this ItemDto self)
        {
            return (new Item(self.Header, self.Count, self.Price, self.Mod, self.ID), self.ServerName);
        }
    }
}
