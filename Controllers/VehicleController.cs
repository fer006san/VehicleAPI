using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Models;

namespace VehicleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/vehicles
    public class VehiclesController : ControllerBase
    {
        // Persistencia en MEMORIA (vida del proceso)
        private static readonly List<Vehicle> Data = new()
        {
            new Vehicle { Make = "Ford",  Model = "Fiesta", Year = 2018 },
            new Vehicle { Make = "Toyota",Model = "Corolla",Year = 2020 }
        };

        // GET api/vehicles?make=Ford&year=2020 (Query params)
        [HttpGet]
        public ActionResult<IEnumerable<Vehicle>> Get([FromQuery] string? make, [FromQuery] int? year)
        {
            var q = Data.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(make))
                q = q.Where(v => v.Make.Contains(make, StringComparison.OrdinalIgnoreCase));

            if (year is > 0)
                q = q.Where(v => v.Year == year);

            return Ok(q.ToList());
        }

        // GET api/vehicles/{id} (Path param)
        [HttpGet("{id:guid}")]
        public ActionResult<Vehicle> GetById(Guid id)
        {
            var v = Data.FirstOrDefault(x => x.Id == id);
            return v is null ? NotFound() : Ok(v);
        }

        // POST api/vehicles  (Body JSON)
        [HttpPost]
        public ActionResult<Vehicle> Create([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // Asegurar que tenga Id si vino vacío
            if (vehicle.Id == Guid.Empty) vehicle.Id = Guid.NewGuid();

            Data.Add(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        // PUT api/vehicles/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Replace(Guid id, [FromBody] Vehicle vehicle)
        {
            var existing = Data.FirstOrDefault(x => x.Id == id);
            if (existing is null) return NotFound();

            // Reemplazo de campos (mantengo el Id de la ruta)
            existing.Make  = vehicle.Make;
            existing.Model = vehicle.Model;
            existing.Year  = vehicle.Year;

            return NoContent(); // 204
        }

        // DELETE api/vehicles/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var existing = Data.FirstOrDefault(x => x.Id == id);
            if (existing is null) return NotFound();

            Data.Remove(existing);
            return NoContent();
        }
    }
}