using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Product{
    public int Id{get; set;}
    public string Name{get; set;} = null!;
    public int Quantity{get; set;}
    public int WarehousesId{get; set;}
    public Warehouse Warehouse{get; set;} = null!;
}