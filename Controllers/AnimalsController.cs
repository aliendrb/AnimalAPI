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
    public IActionResult CreateAnimal(CreateAnimalDTOs request) 
    {
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var sqlCommand = new SqlCommand(
                "INSERT INTO Animals (IdAnimal, Name, Description, Category, Area) values (@1, @2, @3, @4, @5);", //SELECT CAST(SCOPE_IDENTITY()) as int
                sqlConnection
                );
            sqlCommand.Parameters.AddWithValue("@1", request.IdAnimal);
            sqlCommand.Parameters.AddWithValue("@2", request.Name);
            sqlCommand.Parameters.AddWithValue("@3", request.Description);
            sqlCommand.Parameters.AddWithValue("@4", request.Category);
            sqlCommand.Parameters.AddWithValue("@5", request.Area);
            sqlCommand.Connection.Open();

            var id = sqlCommand.ExecuteScalar();
            Console.WriteLine(id);

            // Check if the id is DBNull or null
            if (id == DBNull.Value || id == null)
            {
                // Handle the scenario where id is DBNull or null
                // For example, return a BadRequest or any appropriate response
                return BadRequest("Unable to retrieve the id of the newly created record.");
            }

            // Convert the id to an integer
            int newId;
            if (!int.TryParse(id.ToString(), out newId))
            {
                // Handle the scenario where id cannot be converted to an integer
                // For example, return a BadRequest or any appropriate response
                return BadRequest("Unable to convert the id to an integer.");
            }

            return Created($"animals/{newId}", new CreateAnimalResponse((int)newId, request));
        }
    }
    //[HttpPut("api/animals/{id}")]
    [HttpDelete("api/animals/{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        return Ok();
    }
}