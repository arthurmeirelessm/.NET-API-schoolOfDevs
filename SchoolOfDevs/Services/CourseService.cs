using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Helpers;


// NO SERVICES TEREMOS O CÉREBRO DA API, ONDE FAZEMOS TODOS OS PUSHS DE ELEMENTOS DO SQL SERVER E MONTAR COMO UMA CONSULTA
namespace SchoolOfDevs.Services
{

    public interface ICourseService
    {
        public Task<Course> Create(Course course);
        public Task<Course> GetById(int id);
        public Task<List<Course>> GetAll();
        public Task Update (Course courseIn, int id);
        public Task Delete(int id);

    }
    public class CourseService : ICourseService
    {
        private readonly DataContext _context;

        public CourseService(DataContext context)
        {
            _context = context;
        }

        public async Task<Course> Create(Course course)
        {
            _context.Courses.Add(course);
           await _context.SaveChangesAsync();

            return course;
        }

        public async Task Delete(int id)
        {
            Course courseDb = await _context.Courses
                .SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
            {
                throw new Exception($"Course {id} not found");
            }
            //CASO NÃO EXISTA O ID PASSADO, ELE IRÁ REMOVER COMO É FEITO ABAIXO PELO METODO 'REMOVE()'
            _context.Courses.Remove(courseDb);
            await _context.SaveChangesAsync();
        }

        //BUSCA A TABELA TODA E RETORNA COMO LISTA
        //NO GETTALL É USADO ARROW FUNCTION PELO FATO DE TER PRECISO DE APENAS UMA LINHA PARA INCREMENTAR O METODO, COMO É VISTO ABAIXO:
        public async Task<List<Course>> GetAll() =>  await _context.Courses.ToListAsync();

        public async Task<Course> GetById(int id)
        {
            Course courseDb = await _context.Courses
               .SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
            {
                throw new Exception($"Course {id} not found");
            }

            return courseDb;
        }

        public async Task Update(Course courseIn, int id)
        {
            if (courseIn.Id != id)
                throw new Exception("Route id differs Course id");

            Course userDb = await _context.Courses
                .AsNoTracking()
                 .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
            {
                throw new Exception($"Course {id} not found");
            }

            //ENTRY() E ()ENTITYSTATE.MODIFIED PARA METODOS UPDATE
            _context.Entry(courseIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
