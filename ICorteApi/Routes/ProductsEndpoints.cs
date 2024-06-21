// using Microsoft.AspNetCore.Http.HttpResults;

// namespace BarberAppApi.Routes;

// public static class ProductsEndpoints
// {
//     public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
//     {
//         var group = app.MapGroup("api/products");

//         group.MapPost("", CreateProduct);
//         group.MapGet("", CreateProduct);
//         group.MapPost("", CreateProduct);
//         group.MapPost("", CreateProduct);
//     }

//     public static async Task<Results<Created>> CreateProduct(CreateProductRequest request, ISender sender)
//     {
//         var command = new CreateProductCommand(
//             request.Name,
//             request.Sku,
//             request.Currency,
//             request.Amount
//         );

//         await sender.Send(command);

//         return Results.Created("Tudo bem, tudo bangos");
//     }
// }
