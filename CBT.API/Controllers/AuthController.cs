using CBT.API.DTOs;
using CBT.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CBT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
            
        }
        [HttpGet("test")]
        public IActionResult Get()
        {
            var users = new List<string>{ "User1", "User2", "User3", "User4", "User5", "User6", };
            return Ok(users);
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDto model) 
        {
            Response? resp;
            try
            {
                if(string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                {
                    resp = new Response
                    {
                        Data = null,
                        ResponseCode = "99",
                        ResponseMessage = "Login Failed, Try again!"
                    };
                        
                    return BadRequest(resp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var jsonUser = JsonSerializer.Serialize(model.UserName);
            resp = new Response()
            {
                Data = jsonUser,
                ResponseCode = "00",
                ResponseMessage = "Login Successful!!"
            };
            return Ok(resp);
       
        }


        [HttpPost("register")]
        public IActionResult Register(RegisterDto model)
        {
            Response? resp;
            try
            {
                if (!ModelState.IsValid || model == null)
                {
                    resp = new Response
                    {
                        Data = null,
                        ResponseCode = "99",
                        ResponseMessage = "Registration Failed, Try again!"
                    };

                    return BadRequest(resp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Random rand = new Random();
            var fig = rand.Next(999999);
            var user1 = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                PasswordHash = $"CBT-{fig}AfvjUGTknCfFyfCTxCyfTh{model.Password.GetHashCode().ToString()}",
                Role = "Passenger"
            };
            var jsonUser = JsonSerializer.Serialize(user1);
            resp = new Response()
            {
                Data = jsonUser,
                ResponseCode = "00",
                ResponseMessage = "Registration Successful!!"
            };
            return Ok(resp);

        }
    }
}
