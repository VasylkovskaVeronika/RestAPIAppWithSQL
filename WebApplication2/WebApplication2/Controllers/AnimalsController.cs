using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
        SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")); 
        conn.Open();
        return Ok();
    }
}