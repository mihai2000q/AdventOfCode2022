namespace AdventOfCode2022.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ProblemNameAttribute : Attribute
{
    public ProblemNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}