// using BarberAppApi.Entities;
// using BarberAppApi.Repositories;
// using BarberAppApi.Services;
// using Microsoft.AspNetCore.Mvc;

// namespace BarberAppApi.Routes;

// [ApiController]
// [Route("v1")]
// public class LoginEndpoint : ControllerBase
// {
//     [HttpPost]
//     [Route("login")]
//     public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] User model)
//     {
//         var user = UserRepository.Get(model.Username, model.Password);

//         if (user is null)
//             return NotFound(new { message = "Usuário ou senha inválidos" });

//         var token = TokenService.GenerateToken(user);

//         user.Password = "";

//         return new
//         {
//             user = user,
//             token = token
//         };
//     }
// }
