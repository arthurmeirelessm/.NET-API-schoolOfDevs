using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Dto.Course;
using SchoolOfDevs.Dto.User;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Exceptions;
using SchoolOfDevs.Helpers;


// NO SERVICES TEREMOS O CÉREBRO DA API, ONDE FAZEMOS TODOS OS PUSHS DE ELEMENTOS DO SQL SERVER E MONTAR COMO UMA CONSULTA
namespace SchoolOfDevs.Services
{

    public interface ICourseService
    {
        public Task<CourseResponse> Create(CourseRequest user);
        public Task<CourseResponse> GetById(int id);
        public Task<List<CourseResponse>> GetAll();
        public Task Update (CourseRequest userIn, int id);
        public Task Delete(int id);
        
    }
    public class CourseService : ICourseService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CourseService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseResponse> Create(CourseRequest courseRequest)
        {
            Course course = _mapper.Map<Course>(courseRequest);

            _context.Courses.Add(course);
           await _context.SaveChangesAsync();

            return _mapper.Map<CourseResponse>(course);
        }

        public async Task Delete(int id)
        {
            Course courseDb = await _context.Courses
                .SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
            {
                throw new KeyNotFoundException($"Course {id} not found");
            }
            //CASO NÃO EXISTA O ID PASSADO, ELE IRÁ REMOVER COMO É FEITO ABAIXO PELO METODO 'REMOVE()'
            _context.Courses.Remove(courseDb);
            await _context.SaveChangesAsync();
        }

        //BUSCA A TABELA TODA E RETORNA COMO LISTA
        //NO GETTALL É USADO ARROW FUNCTION PELO FATO DE TER PRECISO DE APENAS UMA LINHA PARA INCREMENTAR O METODO, COMO É VISTO ABAIXO:
        public async Task<List<CourseResponse>> GetAll()
        {
          List<Course> courses = await _context.Courses.ToListAsync();
            return courses.Select(c => _mapper.Map<CourseResponse>(c)).ToList();
        }

        public async Task<CourseResponse> GetById(int id)
        {
            Course courseDb = await _context.Courses.Include(c => c.Teacher).SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
            {
                throw new KeyNotFoundException($"Course {id} not found");
            }

            return _mapper.Map<CourseResponse>(courseDb);
        }

        public async Task Update(CourseRequest courseRequest, int id)
        {
            if (courseRequest.Id != id)
                throw new BadRequestException("Route id differs Course id");

            Course courseDb = await _context.Courses
                .AsNoTracking()
                 .SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
                throw new KeyNotFoundException($"Course {id} not found");

            courseDb = _mapper.Map<Course>(courseRequest);

            //ENTRY() E ()ENTITYSTATE.MODIFIED PARA METODOS UPDATE
            _context.Entry(courseDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
