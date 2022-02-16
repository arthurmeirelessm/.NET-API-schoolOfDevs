using Microsoft.AspNetCore.Mvc;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Services;

namespace SchoolOfDevs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        //AQUI ABAIXO É SETADO QUAL TIPO DE METODO VAI SER CHAMADO DO IUSERSERVICE(QUE CHAMA OS TIPOS SE SAO CREATE, GETALL, UPDATE) PARA A CRIAÇÃO DO ENDPOINT 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user) => Ok(await _service.Create(user));

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetById(id));

        [HttpPut("{id}")]

        public async Task<IActionResult> Update([FromBody] User userIn, int id)
        {
            await _service.Update(userIn, id);
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