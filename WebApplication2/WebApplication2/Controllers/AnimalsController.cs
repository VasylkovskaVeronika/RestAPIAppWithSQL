using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController: ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration conf)
    {
        _configuration = conf;
    }
    [HttpGet]
    public IActionResult getAnimals()
    {
        //string connStr = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect" +
                        // " Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi" +
                         //" Subnet Failover=False";
        using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")); 
        conn.Open();

        using SqlCommand command = new SqlCommand("SELECT * FROM ANIMAL", conn);

        var reader = command.ExecuteReader();

        List<Animal> res = new List<Animal>();
        int idOrdinal = reader.GetOrdinal(("AnimalId"));
        int nameOfOrdinal = reader.GetOrdinal(("Name"));
        int descOrdinal = reader.GetOrdinal(("Description"));
        int catOrdinal = reader.GetOrdinal(("Category"));
        int areaOrdinal = reader.GetOrdinal(("Area"));
        
        while (reader.Read())
        {
            res.Add(new Animal(
                reader.GetInt32(idOrdinal),
                reader.GetString(nameOfOrdinal),
                reader.GetString(descOrdinal),
                reader.GetString(catOrdinal),
                reader.GetString(areaOrdinal)
                ));
        }

        return Ok(res);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal animal)
    {
        using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default"));
        conn.Open();

        using SqlCommand command = new SqlCommand();
        command.Connection = conn;
           command.CommandText= "INSERT INTO ANIMAL VALUES(@aname, @ad, @ac, @aa)";
           command.Parameters.AddWithValue("@aname", animal.Name);
           command.Parameters.AddWithValue("@ad", animal.Desc);
           command.Parameters.AddWithValue("@ac", animal.Category);
           command.Parameters.AddWithValue("@aa", animal.Area);
           command.ExecuteNonQuery();
    return Created("", null);
    }
}