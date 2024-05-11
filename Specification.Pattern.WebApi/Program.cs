using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Specification.Pattern.WebApi.Data;
using Specification.Pattern.WebApi.Entities;
using Specification.Pattern.WebApi.Specifications.Common;
using Specification.Pattern.WebApi.Specifications.Product;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsDb"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/Products", async ([AsParameters] GetProductsQuery query, ProductsDbContext db) =>
    {
        // var dbQuery = db.Products.AsNoTracking();
        //
        // if (query.StartingPrice.HasValue)
        //     dbQuery = dbQuery.Where(c => c.ListPrice >= query.StartingPrice);
        //
        // if (query.EndingPrice.HasValue)
        //     dbQuery = dbQuery.Where(c => c.ListPrice <= query.EndingPrice);
        //
        // if (query.Weight.HasValue)
        //     dbQuery = dbQuery.Where(c => c.Weight > query.Weight);
        //
        // if (!string.IsNullOrWhiteSpace(query.Size))
        //     dbQuery = dbQuery.Where(c => c.Size.Contains(query.Size));
        //
        // if (!string.IsNullOrWhiteSpace(query.Color))
        //     dbQuery = dbQuery.Where(c => c.Color.Equals(query.Color));
        //
        // return Results.Ok(await dbQuery.ToListAsync());

        Specification<Product> specification = Specification<Product>.Default;

        if (query.StartingPrice.HasValue)
            specification = specification.And(new ProductPriceLessSpecification(query.StartingPrice.Value));
        
        if (query.EndingPrice.HasValue)
            specification= specification.And(new ProductPriceMoreSpecification(query.EndingPrice.Value));
        
        if (query.Weight.HasValue)
            specification= specification.And(new ProductPriceWeightSpecification(query.Weight.Value));

        var expression = specification.ToExpression();

        var result = await db.Products.Where(expression).ToListAsync();

        return Results.Ok(result);

    }).WithName("Products")
    .WithOpenApi();

app.UseHttpsRedirection();



app.Run();


public class GetProductsQuery
{
    public decimal? StartingPrice { get; set; }
    public decimal? EndingPrice { get; set; }
    public decimal? Weight { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
}
