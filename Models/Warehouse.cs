using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Warehouse{
    public int Id {get; set;}
    public string Name {get; set;} = null!;
    public int Capacity {get; set;}
    public ICollection<Product> Products { get; set; } = new List<Product>();//for betetr quering or idk something else
} 