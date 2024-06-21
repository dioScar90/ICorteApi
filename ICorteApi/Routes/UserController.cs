// using AutoMapper;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using barberapp.Entities;
// using barberapp.Context;
// using barberapp.Dtos;

// namespace barberapp.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class UserController : ControllerBase
// {
//     private readonly AppDbContext _context;
//     private IMapper _mapper { get; }

//     public UserController(AppDbContext context, IMapper mapper)
//     {
//         _context = context;
//         _mapper = mapper;
//     }

//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
//     {
//         var users = await _context.Users.Where(u => u.IsActive == true).Include(u => u.Addresses.Where(a => a.IsActive == true)).ToListAsync();
        
//         var convertedUsers = _mapper.Map<IEnumerable<GetUserDto>>(users);

//         return Ok(convertedUsers);
//     }

//     private async Task<ActionResult<IEnumerable<Address>>> GetAddresses(int id)
//     {
//         var addresses = await _context.Addresses.Where(a => a.UserId == id && a.IsActive == true).ToListAsync();
//         return addresses;
//     }

//     [HttpGet("{id}")]
//     public async Task<ActionResult<GetUserDto>> GetUser([FromRoute] int id)
//     {
//         var user = await _context.Users.Include(u => u.Addresses.Where(a => a.IsActive == true)).FirstOrDefaultAsync(u => u.Id == id && u.IsActive == true);
        
//         if (user is null)
//             return NotFound();
        
//         var convertedUser = _mapper.Map<GetUserDto>(user);
        
//         return Ok(convertedUser);
//     }

//     // private List<Address> GetAddressesIntoList(ICollection<GetAddressDto> addresses)
//     // {
//     //     var newList = addresses.Aggregate(new List<Address>(), (acc, curr) => {
//     //         Address address = new()
//     //         {
//     //             StreetType      = curr.StreetType,
//     //             Street          = curr.Street,
//     //             Number          = curr.Number,
//     //             Complement      = curr.Complement,
//     //             Neighborhood    = curr.Neighborhood,
//     //             City            = curr.City,
//     //             State           = curr.State,
//     //             PostalCode      = curr.PostalCode
//     //         };

//     //         acc.Add(address);

//     //         return acc;
//     //     });

//     //     return newList;
//     // }
    
//     [HttpPost]
//     public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
//     {
//         User newUser = _mapper.Map<User>(dto);
//         await _context.Users.AddAsync(newUser);
//         int newId = await _context.SaveChangesAsync();

//         var objOk = new {
//             id = newUser.Id,
//             message = "User created successfully"
//         };

//         // var addressesList = GetAddressesIntoList(dto.Addresses);

//         // var novo = new User() {
//         //     FirstName   = dto.FirstName,
//         //     LastName    = dto.LastName,
//         //     Email       = dto.Email,
//         //     Phone       = dto.Phone,
//         //     Password    = dto.Password,
//         //     Addresses   = addressesList,
//         //     Role        = dto.Role
//         // };
        
//         // await _context.Users.AddAsync(novo);
//         // await _context.SaveChangesAsync();

//         // return Ok({message: "User created successfully"});
//         return Ok(objOk);
//     }

//     [HttpPut("{id}")]
//     public IActionResult UpdateAgendamento([FromRoute] int id, [FromBody] Agendamento agendamento)
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
//     public async Task<IActionResult> DeleteUser([FromRoute] int id)
//     {
//         var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    
//         if (user is null)
//             return NotFound();
        
//         if (user.IsActive == false)
//             return BadRequest("User had already been deleted.");
        
//         user.IsActive = false;
//         user.UpdatedAt = DateTime.Now;
//         await _context.SaveChangesAsync();
        
//         return Ok($"User {user.GetFullName()} deleted successfully.");
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
