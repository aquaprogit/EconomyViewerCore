namespace EconomyViewer.DB
{
    internal class ItemDto
    {
        public int ID { get; set; }
        public string Header { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Price { get; set; }
        public string Mod { get; set; } = string.Empty;
        public string ServerName { get; set; } = string.Empty;
    }
}
