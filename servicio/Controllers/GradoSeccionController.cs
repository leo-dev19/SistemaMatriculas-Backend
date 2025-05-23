using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using servicio.Data;
using servicio.Models;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradoSeccionController : ControllerBase
    {
        private readonly MyAppContext _appContext;

        public GradoSeccionController(MyAppContext appContext)
        {
            _appContext = appContext;
        }

        // Obtener todas las secciones
        [HttpGet]
        public IActionResult GetAllGradoSecciones()
        {
            var gradoSecciones = _appContext.GradoSecciones.ToList();
            return Ok(gradoSecciones);
        }

        // Obtener una sección por ID
        [HttpGet("{id}")]
        public IActionResult GetGradoSeccionById(int id)
        {
            var gradoSeccion = _appContext.GradoSecciones.FirstOrDefault(g => g.Id == id);
            if (gradoSeccion == null)
            {
                return NotFound($"GradoSeccion con ID {id} no encontrada.");
            }
            return Ok(gradoSeccion);
        }

        // Agregar una nueva sección
        [HttpPost]
        public IActionResult AddGradoSeccion([FromBody] GradoSeccionDTO gradoSeccionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gradoSeccion = new GradoSeccion
            {
                Nombre = gradoSeccionDTO.Nombre
            };

            _appContext.GradoSecciones.Add(gradoSeccion);
            _appContext.SaveChanges();

            return CreatedAtAction(nameof(GetGradoSeccionById), new { id = gradoSeccion.Id }, gradoSeccion);
        }

        // Editar una sección existente
        [HttpPut("{id}")]
        public IActionResult EditGradoSeccion(int id, [FromBody] GradoSeccionDTO gradoSeccionDTO)
        {
            var gradoSeccion = _appContext.GradoSecciones.FirstOrDefault(g => g.Id == id);
            if (gradoSeccion == null)
            {
                return NotFound($"GradoSeccion con ID {id} no encontrada.");
            }

            gradoSeccion.Nombre = gradoSeccionDTO.Nombre;
            _appContext.SaveChanges();

            return Ok($"GradoSeccion con ID {id} ha sido actualizada.");
        }

        // Eliminar una sección
        [HttpDelete("{id}")]
        public IActionResult DeleteGradoSeccion(int id)
        {
            var gradoSeccion = _appContext.GradoSecciones.FirstOrDefault(g => g.Id == id);
            if (gradoSeccion == null)
            {
                return NotFound($"GradoSeccion con ID {id} no encontrada.");
            }

            _appContext.GradoSecciones.Remove(gradoSeccion);
            _appContext.SaveChanges();

            return Ok($"GradoSeccion con ID {id} ha sido eliminada.");
        }
    }
}
