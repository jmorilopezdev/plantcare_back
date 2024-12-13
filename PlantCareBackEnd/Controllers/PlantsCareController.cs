using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantCareBackEnd.Data;
using PlantCareBackEnd.Models;

namespace PlantCareBackEnd.Controllers
{
    [ApiController()]
    [Route("api/plants")]
    public class PlantsCareController : ControllerBase
    {
        // Definiciones y asunciones
        // Se implementa el controlador PlantCare para las operaciones funcionales de la aplicacion CRUD
        // Se asume por la definficion de la prueba no hay un repositorio o bd para las operaciones CRUD, por lo tanto se trabajara con datos en memoria temporal que podran 
        // ser operables simulando la conexion a una bd.

        private readonly PlantDbContext _context;

        public PlantsCareController(PlantDbContext context)
        {
            _context = context;
        }

        //listar todos los registros plant care
        [HttpGet]
        public ActionResult<IEnumerable<Plant>> GetAll()
        {
            return _context.Plants.ToList();
        }

        //lista plant care por id
        [HttpGet("{id}")]
        public ActionResult<Plant> GetById(string id)
        {
            var plant = _context.Plants.Find(id);

            if (plant == null)
            {
                return NotFound();
            }

            return plant;
        }

        //agregar nuevo plant care y devuelve el registro creado
        [HttpPost]
        public ActionResult<Plant> Create(Plant plant)
        {
            plant.Id = Guid.NewGuid().ToString();

            _context.Plants.Add(plant);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = plant.Id }, plant);
        }

        //actualiza plant care por id
        [HttpPut("{id}")]
        public IActionResult Update(string id, Plant plant)
        {
            if (id != plant.Id)
            {
                return BadRequest();
            }

            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Plants.Any(e => e.Id == id))
                {
                    _context.Entry(plant).State = EntityState.Detached;
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
