using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using AnimalAPI.DTOs;

namespace AnimalAPI.Controllers;

[ApiController]
[Route("controller-animals")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("api/animals")]
    public IActionResult GetAllAnimals() 
    {
        var response = new List<GetAnimalsResponse>();
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var sqlCommand = new SqlCommand("SELECT * FROM Animals", sqlConnection);
            sqlCommand.Connection.Open();
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read()) 
            {
                response.Add(new GetAnimalsResponse(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                    )
                );
            }
        }
        return Ok(response);
    }
    [HttpGet("api/animals{id}")]
    
    public IActionResult GetAnimal(int id) 
    {
        return Ok();
    }

    [HttpPost("api/animals")]
    public IActionResult CreateAnimal(CreateAnimalRequest request) 
    {
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var sqlCommand = new SqlCommand(
                "INSERT INTO Animals (IdAnimal"
                )
            return Created($"animals/{id}", new CreateAnimalResponse((int)id, request);
        }
    }
    [HttpPut("api/animals/{id}")]
    [HttpDelete("api/animals/{id}")]
}