namespace WebApplication2.Models;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }

    public Animal(int id, string name, string desc, string category, string area)
    {
        Id = id;
        Name = name;
        Desc = desc;
        Category = category;
        Area = area;
    }
}