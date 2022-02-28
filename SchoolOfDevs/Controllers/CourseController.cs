using Microsoft.AspNetCore.Mvc;
using SchoolOfDevs.Dto.Course;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Services;

namespace SchoolOfDevs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        //AQUI ABAIXO É SETADO QUAL TIPO DE METODO VAI SER CHAMADO DO ICourseSERVICE(QUE CHAMA OS TIPOS SE SAO CREATE, GETALL, UPDATE) PARA A CRIAÇÃO DO ENDPOINT 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseRequest course) => Ok(await _service.Create(course));

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetById(id));

        [HttpPut("{id}")]

        public async Task<IActionResult> Update([FromBody] CourseRequest course, int id)
        {
            await _service.Update(course, id);
            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }

    }
}