// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using ICorteApi.Entities;
// using ICorteApi.Context;
// using ICorteApi.Dtos;

// namespace ICorteApi.Routes;

// public static class AppointmentsEndpoint
// {
//     public static void MapAppointmentsEndpoint(this IEndpointRouteBuilder app)
//     {
//         const string INDEX = "";
//         var group = app.MapGroup("appointment");

//         // group.MapGet(INDEX, GetAppointments);
//         group.MapGet("{id}", GetAppointment);
//         group.MapPost(INDEX, CreateAppointment);
//         // group.MapPut("{id}", UpdateAppointment);
//         // group.MapDelete("{id}", DeleteAppointment);
//     }
    
//     public static async Task<Results<Ok<Appointment>, NotFound<string>>> GetAppointment(int id, ICorteContext context)
//     {
//         var appointment = await context.Appointments
//             .SingleOrDefaultAsync(a => a.IsActive && a.Id == id);

//         if (appointment is null)
//             return TypedResults.NotFound("Agendamento não encontrado");

//         return TypedResults.Ok(appointment);
//     }
    
//     public static async Task<Results<Created<int>, BadRequest<string>>> CreateAppointment(AppointmentDto dto)
//     {
//         try
//         {
//             Appointment appointment = new()
//             {
//                 public override int Id { get; set; }
//                 public int BarberId { get; set; }
//                 public int ClientId { get; set; }
//                 public int? PaymentId { get; set; }
//                 public DateTime AppointmentDate { get; set; }
//                 public string ServicesRequested { get; set; }

//                 // Navigation Properties
//                 public Barber Barber { get; set; }
//                 public Client Client { get; set; }
//                 public Payment? Payment { get; set; }
//                 public ICollection<Service> Services { get; set; } = [];
//             }
//         }
//         catch (Exception ex)
//         {
//             return TypedResults.BadRequest(ex.Message);
//         }
//     }

//     [HttpPut("{id}")]
//     public IActionResult UpdateAgendamento(int id, Agendamento agendamento)
//     {
//         if (id != agendamento.Id)
//         {
//             return BadRequest();
//         }

//         _context.Entry(agendamento).State = EntityState.Modified;
//         _context.SaveChanges();

//         return NoContent();
//     }

//     [HttpDelete("{id}")]
//     public IActionResult DeleteAgendamento(int id)
//     {
//         var agendamento = _context.Agendamentos.Find(id);
//         if (agendamento == null)
//         {
//             return NotFound();
//         }

//         _context.Agendamentos.Remove(agendamento);
//         _context.SaveChanges();

//         return NoContent();
//     }

//     // private readonly ApplicationDbContext _context;

//     // public ProductsController(ApplicationDbContext context)
//     // {
//     //     _context = context;
//     // }

//     // // CRUD -> Creade - Read - Update - Delete

//     // // Create
//     // [HttpPost]
//     // public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProctDto dto)
//     // {
//     //     var newProduct = new ProductEntity()
//     //     {
//     //         Brand = dto.Brand,
//     //         Title = dto.Title
//     //     };

//     //     await _context.Products.AddAsync(newProduct);
//     //     await _context.SaveChangesAsync();

//     //     return Ok("Product Saved Successfully");
//     // }

//     // // Read
//     // [HttpGet]
//     // public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
//     // {
//     //     var products = await _context.Products.OrderByDescending(q => q.CreatedAt).ToListAsync();

//     //     return Ok(products);
//     // }

//     // [HttpGet]
//     // [Route("{id}")]
//     // public async Task<ActionResult<ProductEntity>> GetProductById([FromRoute] int id)
//     // {
//     //     var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

//     //     if (product is null)
//     //         return NotFound("Product Not Found");
        
//     //     return Ok(product);
//     // }

//     // // Update
//     // [HttpPut]
//     // [Route("{id}")]
//     // public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] CreateUpdateProctDto dto)
//     // {
//     //     var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

//     //     if (product is null)
//     //         return NotFound("Product Not Found");
        
//     //     product.Title = dto.Title;
//     //     product.Brand = dto.Brand;
//     //     product.UpdatedAt = DateTime.Now;

//     //     await _context.SaveChangesAsync();

//     //     return Ok("Product Updated Successfully");
//     // }

//     // // Delete
//     // [HttpDelete]
//     // [Route("{id}")]
//     // public async Task<IActionResult> DeleteProduct([FromRoute] int id)
//     // {
//     //     var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

//     //     if (product is null)
//     //         return NotFound("Product Not Found");
        
//     //     _context.Products.Remove(product);
//     //     await _context.SaveChangesAsync();

//     //     return Ok("Product Deleted Successfully");
//     // }
// }
