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


/*    [HttpGet("api/animals")]
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
    }*/


    [HttpGet("api/animals")]
    public IActionResult GetAllAnimals([FromQuery] string orderBy = "name")
    {
        var response = new List<GetAnimalsResponse>();
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            string orderByClause;
            if (string.IsNullOrEmpty(orderBy))
            {
                orderByClause = "ORDER BY name";
            }
            else
            {
                orderByClause = $"ORDER BY {orderBy}";
            }

            var sqlCommand = new SqlCommand($"SELECT * FROM Animals {orderByClause}", sqlConnection);
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


    [HttpGet("api/animals/{id}")]
    public IActionResult GetAnimal(int id) 
    {
        var response = new List<GetAnimalsResponse>();
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var sqlCommand = new SqlCommand("SELECT IdAnimal, Name, Description, Category, Area FROM Animals WHERE IdAnimal = @1", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@1", id);
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


    [HttpPost("api/animals")]
    public IActionResult CreateAnimal(CreateAnimalDTOs request) 
    {
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var sqlCommand = new SqlCommand(
                "INSERT INTO Animals (Name, Description, Category, Area) values (@1, @2, @3, @4); SELECT CAST(SCOPE_IDENTITY() as int)",
                sqlConnection
                );
            sqlCommand.Parameters.AddWithValue("@1", request.Name);
            sqlCommand.Parameters.AddWithValue("@2", request.Description);
            sqlCommand.Parameters.AddWithValue("@3", request.Category);
            sqlCommand.Parameters.AddWithValue("@4", request.Area);
            sqlCommand.Connection.Open();

            var id = sqlCommand.ExecuteScalar();

            return Created($"animals/{id}", new CreateAnimalResponse((int)id, request));
        }
    }


    [HttpPut("api/animals/{id}")]
    public IActionResult ReplaceAnimal(int id, ReplaceAnimalRequest request) 
    {
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default"))) 
        {
            var sqlCommand = new SqlCommand(
                "UPDATE Animals SET Name = @1, Description = @2, Category = @3, Area = @4 WHERE IdAnimal = @5"
                , sqlConnection);
            sqlCommand.Parameters.AddWithValue("@1", request.Name);
            sqlCommand.Parameters.AddWithValue("@2", request.Description);
            sqlCommand.Parameters.AddWithValue("@3", request.Category);
            sqlCommand.Parameters.AddWithValue("@4", request.Area);
            sqlCommand.Parameters.AddWithValue("@5", id);
            sqlCommand.Connection.Open();

            var affectedRows = sqlCommand.ExecuteNonQuery();
            return affectedRows == 0 ? NotFound() : NoContent();
        }
    }


    [HttpDelete("api/animals/{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var sqlCommand = new SqlCommand("DELETE FROM Animals WHERE IdAnimal = @1", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@1", id);
            sqlCommand.Connection.Open();
            var affectedRows = sqlCommand.ExecuteNonQuery();

            return affectedRows == 0 ? NotFound() : NoContent();
        }
    }
}