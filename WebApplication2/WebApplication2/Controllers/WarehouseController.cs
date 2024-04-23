using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWarehouseRepository _repo;

    public WarehouseController(IConfiguration c, IWarehouseRepository r)
    {
        _configuration = c;
        _repo = r;
    }
    [HttpPost]
    public IActionResult CreateProductWarehouse(ProductWarehouse warehouse)
    {
        if (_repo.ProductWithIDExists(warehouse.IdProduct)
            && _repo.WarehouseWithIDExists(warehouse.IdWarehouse)
            && warehouse.Amount > 0)
        {
            if (_repo.AddProductRequirementsCompleted(warehouse.IdProduct, warehouse.Amount, warehouse.CreatedAt))
            {
                if (_repo.IsOrderCompleted(warehouse.IdProduct, warehouse.Amount))
                {
                    _repo.UpdateFullfilledAt(warehouse.IdProduct, warehouse.Amount);
                    return Created("","Record inserted with ID = " + _repo.InsertProductWarehouse(warehouse.IdProduct, warehouse.IdWarehouse, warehouse.Amount));
                }
                return BadRequest("The order has already been completed.");
            }
            return NotFound("There is no suitable order.");
        }
        return NotFound("The product/warehouse with the given id does not exist");
    }
}