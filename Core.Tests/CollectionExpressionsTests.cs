namespace Core.Tests;
public class CollectionExtensionsTests {
    [Fact]
    public void NullOrEmpty_WithNullEnumerable_ReturnsTrue() {
        IEnumerable<int>? collection = null;
        Assert.True(collection.NullOrEmpty());
    }

    [Fact]
    public void NullOrEmpty_WithEmptyEnumerable_ReturnsTrue() {
        var collection = Enumerable.Empty<int>();
        Assert.True(collection.NullOrEmpty());
    }

    [Fact]
    public void NullOrEmpty_WithNonEmptyEnumerable_ReturnsFalse() {
        var collection = new[] { 1, 2, 3 };
        Assert.False(collection.NullOrEmpty());
    }

    [Fact]
    public void NotNullOrEmpty_WithNullEnumerable_ReturnsFalse() {
        IEnumerable<int>? collection = null;
        Assert.False(collection.NotNullOrEmpty());
    }

    [Fact]
    public void NotNullOrEmpty_WithEmptyEnumerable_ReturnsFalse() {
        var collection = new List<int>();
        Assert.False(collection.NotNullOrEmpty());
    }

    [Fact]
    public void NotNullOrEmpty_WithNonEmptyEnumerable_ReturnsTrue() {
        var collection = new List<int> { 42 };
        Assert.True(collection.NotNullOrEmpty());
    }

    [Fact]
    public void NullOrEmpty_WithEmptyList_ReturnsTrue() {
        var list = new List<string>();
        Assert.True(list.NullOrEmpty());
    }

    [Fact]
    public void NullOrEmpty_WithNonEmptyList_ReturnsFalse() {
        var list = new List<string> { "a" };
        Assert.False(list.NullOrEmpty());
    }
}
