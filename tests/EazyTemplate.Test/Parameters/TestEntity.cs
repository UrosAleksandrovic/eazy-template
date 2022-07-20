using System;

namespace EazyTemplate.Test.Parameters;

internal class TestEntity
{
    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string[] EnumerableTest { get; set; }
    public TestEntity NestedExample { get; set; }
    public string NullTestProp => null;

    internal TestEntity(string id)
    {
        Id = id;
        CreatedOn = DateTime.UtcNow;
        NestedExample = null;
    }
}
