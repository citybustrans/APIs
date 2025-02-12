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
            var resp = new Response();
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
    }
}
