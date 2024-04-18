using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController: ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAnimalsRepository _repository;
    public AnimalsController(IConfiguration conf, IAnimalsRepository repo)
    {
        _configuration = conf;
        _repository = repo;
    }
    [HttpGet]
    public IActionResult GetAnimals([FromQuery] string orderBy)
    {
        var availableValues = new HashSet<string> {"name", "description", "category", "area"};
        orderBy = availableValues.Contains(orderBy.ToLower()) ? orderBy : "name";
        var res = _repository.GetAnimals(orderBy);
        return Ok(res);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal animal)
    {
        bool res = _repository.AddAnimal(animal);
        if(res)
            return Created("", null);
        return NotFound();
    }

    [HttpPut]
    public IActionResult UpdateAnimal(Animal updated)
    {
        bool res = _repository.UpdateAnimal(updated);
        if(res)
            return Ok(updated);
        return NotFound();
    }

    [HttpDelete]
    public IActionResult DeleteAnimalById(int id)
    {
        bool res = _repository.DeleteAnimal(id);
        if(res)
            return Ok();
        return NotFound();
    }
}