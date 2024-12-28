using FluentAssertions;
using JsonChisel;

namespace JsonChiselTests;

public class NodeTests
{
    [Fact]
   public void Test1()
    {
        var tree = Node.Parse(new string[]
        {
            "a.b", "a.d"
        });
        tree.Children.Should().HaveCount(1);
        tree.Children.FirstOrDefault().Children.Should().HaveCount(2);
    }
    
    [Fact]
    public void Test2()
    {
        var tree = Node.Parse(new string[]
        {
            "c",
            "a.b", "a.d",
            "a.x.y"
        });
        tree.Children.Should().HaveCount(2);
        tree.Children.FirstOrDefault(x=>x.Name == "c").Children.Should().HaveCount(0);
        tree.Children.FirstOrDefault(x=>x.Name == "a").Children.Should().HaveCount(3);
    }
    
    [Fact]
    public void Test3()
    {
        var tree = Node.Parse(new string[]
        {
            "c",
            "a.b", "a.d",
            "a.x.y"
        });
        tree.Children.Should().HaveCount(2);
        tree.Children.FirstOrDefault(x=>x.Name == "c").Children.Should().HaveCount(0);
        tree.Children.FirstOrDefault(x=>x.Name == "a").Children.Should().HaveCount(3);
        tree.Children.FirstOrDefault(x=>x.Name == "a").Children.FirstOrDefault(x=>x.Name=="x").Children.Should().HaveCount(1);
    }
    
    [Fact]
    public void Test4()
    {
        var tree = Node.Parse(new string[]
        {
            "c.s.n",
            "s.n"
        });
        tree.Children.Should().HaveCount(2);
        tree.Children.FirstOrDefault(x=>x.Name == "c").Children.Should().HaveCount(1);
        tree.Children.FirstOrDefault(x=>x.Name == "s").Children.Should().HaveCount(1);
        
    }
    
    
    [Fact]
    public void Test5()
    {
        var tree = Node.Parse(new string[]
        {
            "a.b.c", "a.b.d", "a.e"
        });
        tree.Children.Should().HaveCount(1);
        tree.Children.FirstOrDefault(x => x.Name == "a").Children.Should().HaveCount(2);
        tree.Children.FirstOrDefault(x => x.Name == "a").Children.FirstOrDefault(x => x.Name == "b").Children.Should().HaveCount(2);
    }

    [Fact]
    public void Test6()
    {
        var tree = Node.Parse(new string[]
        {
            "x.y.z", "x.y", "x"
        });
        tree.Children.Should().HaveCount(1);
        tree.Children.FirstOrDefault(x => x.Name == "x").Children.Should().HaveCount(1);
        tree.Children.FirstOrDefault(x => x.Name == "x").Children.FirstOrDefault(x => x.Name == "y").Children.Should().HaveCount(1);
    }

    [Fact]
    public void Test7()
    {
        var tree = Node.Parse(new string[]
        {
            "m.n.o.p", "m.n.o.q", "m.n.r"
        });
        tree.Children.Should().HaveCount(1);
        tree.Children.FirstOrDefault(x => x.Name == "m").Children.Should().HaveCount(1);
        tree.Children.FirstOrDefault(x => x.Name == "m").Children.FirstOrDefault(x => x.Name == "n").Children.Should().HaveCount(2);
        tree.Children.FirstOrDefault(x => x.Name == "m").Children.FirstOrDefault(x => x.Name == "n").Children.FirstOrDefault(x => x.Name == "o").Children.Should().HaveCount(2);
    }
}