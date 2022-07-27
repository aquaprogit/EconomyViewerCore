using EconomyViewer.DAL.Entities;

namespace ItemTests;

public class ItemTest
{
    [Fact]
    public void IncreaseCountTest()
    {
        //Arrange
        Item item = new Item("Diamond", 1, 500, "Vanila");
        //Act
        item.Count = 3;
        //Assert
        Assert.Equal(3, item.Count);
        Assert.Equal(1500, item.Price);
    }
    [Fact]
    public void ChangeCountTest()
    {
        //Arrange
        Item item = new Item("Diamond", 1, 500, "Vanila");
        //Act
        item.ChangeCount(3);
        //Assert
        Assert.Equal(3, item.Count);
        Assert.Equal(500, item.Price);
    }
    [Fact]
    public void ListAddSame()
    {
        //Arrange
        ItemList list = new ItemList();
        Item item1 = new Item("Diamond", 1, 500, "Vanila");
        list.Add(item1);
        list.Add(item1);

        Item item2 = new Item("Diamond", 2, 1000, "Vanila");

        Assert.Equal(list[0], item2);
    }
}