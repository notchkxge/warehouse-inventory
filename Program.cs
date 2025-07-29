using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//warehouse endPoints
// Get warehouse status (current load + capacity)
app.MapGet("/warehouses/status", async (AppDbContext context) => 
{
    var warehouseStatus = await context.Warehouses
        .Select(w => new 
        {
            w.Id,
            w.Name,
            CurrentLoad = w.Products.Sum(p =>(int?) p.Quantity) ?? 0,//handle null for capacity = 0
            w.Capacity,
            UtilizationPercentage = (w.Products.Sum(p => p.Quantity) * 100.0 / w.Capacity)
        })
        .ToListAsync();

    return Results.Ok(warehouseStatus);
});



app.MapPost("/warehouses/new", async (createWarehouseDTO dto, AppDbContext context)=>{
    //normalization of new input
    string normalizedInput = StringHelper.NormalizeName(dto.Name);


    //extracting raw form DB
    var existingNames = await context.Warehouses
        .Select(w => w.Name)
        .ToListAsync();

    //normalization of existing names in DB adn new input
    bool duplicatesCheck = existingNames
        .Select(StringHelper.NormalizeName)
        .Contains(normalizedInput);

    if(duplicatesCheck){
        return Results.Conflict("Name already taken");
    }

    var warehouse = new Warehouse{
        Name = dto.Name,
        Capacity = dto.Capacity
    };
        

    context.Warehouses.Add(warehouse);
    await context.SaveChangesAsync();

    return Results.Created($"/warehouses/{warehouse.Id}",warehouse);
    
});

app.MapDelete("/warehouses/{id}", async(int id, AppDbContext context) =>{
    var warehouseDeletion = await context.Warehouses
        .FindAsync(id);
        //warehouseDeletion is an object so we directly check for null
        if(warehouseDeletion == null){
            return Results.NotFound("Warehouse does not exist");
        }
        context.Warehouses.Remove(warehouseDeletion);
        await context.SaveChangesAsync();

        return Results.NoContent();
});

//finish swagger













//product endPoints
app.MapGet("/warehouses/{warehouseId}/products",async(int warehouseId, AppDbContext context)=>{
    var products = await context.Products
        .Where(p => p.WarehousesId == warehouseId)
        .ToListAsync();

    return Results.Ok(products); 
});



app.Run();