using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Dto.User;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Exceptions;
using SchoolOfDevs.Helpers;
using BC = BCrypt.Net.BCrypt;

// NO SERVICES TEREMOS O CÉREBRO DA API, ONDE FAZEMOS TODOS OS PUSHS DE ELEMENTOS DO SQL SERVER E MONTAR COMO UMA CONSULTA
namespace SchoolOfDevs.Services
{

    public interface IUserService
    {
        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
        public Task<UserResponse> Create(UserRequest userRequest);
        public Task<UserResponse> GetById(int id);
        public Task<List<UserResponse>> GetAll();
        public Task Update (UserRequestUpdate userRequest, int id);
        public Task Delete(int id);

    }
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserService(DataContext context, IMapper mapper, IJwtService jwtService)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {      User userDb = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == request.UserName);

            if (userDb is null)
                throw new BadRequestException($"User {request.UserName} not found");

            else if (!BC.Verify(request.Password, userDb.Password))
                throw new BadRequestException("Incorret password");
            string token = _jwtService.GenerateJwtToken(userDb);
            return new AuthenticateResponse(userDb, token);

        }

        public async Task<UserResponse> Create(UserRequest userRequest)
        {

            if (!userRequest.Password.Equals(userRequest.ConfirmPassword))
                throw new BadRequestException("Password does not match ConfirmPassword");

            //ESSE METODO ASYNC TEM O OBJETIVO DE PUXAR E VALIDAR SE EXISTE UM NOME NOME IGUAL AO QUE FOI PASSADO NO PARAMETRO (PERCEBE-SE QUE ALGO PARECIDO COM O FILTER DE JS É USADO
            User userDb = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName == userRequest.UserName);

            if (userDb is not null)
                throw new BadRequestException($"UserName {userRequest.UserName} already exist");

            User user = _mapper.Map<User>(userRequest);

            if(user.TypeUser != Enuns.TypeUser.Teacher && userRequest.CoursesStudingIds.Any())
            {
                user.CoursesStuding = new List<Course>();   
                List<Course> courses = await _context.Courses.Where(e => userRequest.CoursesStudingIds.Contains(e.Id)).ToListAsync();
                foreach (Course course in courses)   
                {
                    user.CoursesStuding.Add(course);
                }
            }

            user.Password = BC.HashPassword(user.Password);

            //CASO NÃO EXISTA O NOME UM NOME IGUAL NO BANCO, ESSE METODO CRIA UM NOVO USUARIO, COMO É MOSTRADO ABAIXO:
            _context.Users.Add(user);
           await _context.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }

        public async Task Delete(int id)
        {
            User userDb = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
            {
                throw new KeyNotFoundException($"User {id} not found");
            }
            //CASO NÃO EXISTA O ID PASSADO, ELE IRÁ REMOVER COMO É FEITO ABAIXO PELO METODO 'REMOVE()'
            _context.Users.Remove(userDb);
            await _context.SaveChangesAsync();
        }

        //BUSCA A TABELA TODA E RETORNA COMO LISTA
        //NO GETTALL É USADO ARROW FUNCTION PELO FATO DE TER PRECISO DE APENAS UMA LINHA PARA INCREMENTAR O METODO, COMO É VISTO ABAIXO:
        public async Task<List<UserResponse>> GetAll()
        {
           List<User> users = await _context.Users.ToListAsync();
            return users.Select(e => _mapper.Map<UserResponse>(e)).ToList();
        }

            public async Task<UserResponse> GetById(int id)
        {
            User userDb = await _context.Users
                .Include(e => e.CoursesStuding) // Studing
                .Include(e => e.CoursesTeaching) // Teacher
               .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
            {
                throw new KeyNotFoundException($"User {id} not found");
            }

            return _mapper.Map<UserResponse>(userDb);
        }

        public async Task Update(UserRequestUpdate userRequest, int id)
        {

            if (userRequest.Id != id)
                throw new BadRequestException("Route id differs user id");

            else if (!userRequest.Password.Equals(userRequest.ConfirmPassword))
                throw new BadRequestException("Password does not match ConfirmPassword");

            User userDb = await _context.Users
                .Include(e => e.CoursesStuding) // Studing
                .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
                throw new BadRequestException($"User {id} not found");
            else if (BC.Verify(userRequest.CurrentPassword, userDb.Password))
                throw new BadRequestException ("Incorret password");


            await AddOrRemoveCourse(userDb, userRequest.CoursesStudingIds);

            userDb = _mapper.Map<User>(userRequest); // Talvez o createdAt possa ser default

            userDb.Password = BC.HashPassword(userRequest.Password);
            //ENTRY() E ()ENTITYSTATE.MODIFIED PARA METODOS UPDATE
            _context.Entry(userDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private async Task AddOrRemoveCourse(User userDb, int[] coursesIds)
        {
            int[] removedIds = userDb.CoursesStuding.Where(e => !coursesIds.Contains(e.Id)).Select(e => e.Id).ToArray();
            int[] addedIds = coursesIds.Where(e => !userDb.CoursesStuding.Select(u => u.Id).ToArray().Contains(e)).ToArray();

            if (!removedIds.Any() && !addedIds.Any()) 
            {
                _context.Entry(userDb).State = EntityState.Detached;
                return;
            }

            List<Course> tempCourse = await _context.Courses.Where(e => removedIds.Contains(e.Id) || addedIds.Contains(e.Id)).ToListAsync();


            List<Course> courseToBeRemoved = tempCourse.Where(c => removedIds.Contains(c.Id)).ToList();
            foreach(Course course in courseToBeRemoved)
            {
                userDb.CoursesStuding.Remove(course);
            }

            List<Course> courseToBeAdded = tempCourse.Where(c => addedIds.Contains(c.Id)).ToList();
            foreach (Course course in courseToBeAdded)
            {
                userDb.CoursesStuding.Add(course);
            }

            await _context.SaveChangesAsync();
            _context.Entry(userDb).State = EntityState.Detached;
        }
    }
}
