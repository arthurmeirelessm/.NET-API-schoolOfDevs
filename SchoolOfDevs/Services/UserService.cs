using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Helpers;


// NO SERVICES TEREMOS O CÉREBRO DA API, ONDE FAZEMOS TODOS OS PUSHS DE ELEMENTOS DO SQL SERVER E MONTAR COMO UMA CONSULTA
namespace SchoolOfDevs.Services
{

    public interface IUserService
    {
        public Task<User> Create(User user);
        public Task<User> GetById(int id);
        public Task<List<User>> GetAll();
        public Task Update (User userIn, int id);
        public Task Delete(int id);

    }
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            //ESSE METODO ASYNC TEM O OBJETIVO DE PUXAR E VALIDAR SE EXISTE UM NOME NOME IGUAL AO QUE FOI PASSADO NO PARAMETRO (PERCEBE-SE QUE ALGO PARECIDO COM O FILTER DE JS É USADO
            User userDb = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName == user.UserName);

            if (userDb is not null)
            {
                throw new Exception($"UserName {user.UserName} already exist");
            }

            //CASO NÃO EXISTA O NOME UM NOME IGUAL NO BANCO, ESSE METODO CRIA UM NOVO USUARIO, COMO É MOSTRADO ABAIXO:
            _context.Users.Add(user);
           await _context.SaveChangesAsync();

            return user;
        }

        public async Task Delete(int id)
        {
            User userDb = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
            {
                throw new Exception($"User {id} not found");
            }
            //CASO NÃO EXISTA O ID PASSADO, ELE IRÁ REMOVER COMO É FEITO ABAIXO PELO METODO 'REMOVE()'
            _context.Users.Remove(userDb);
            await _context.SaveChangesAsync();
        }

        //BUSCA A TABELA TODA E RETORNA COMO LISTA
        //NO GETTALL É USADO ARROW FUNCTION PELO FATO DE TER PRECISO DE APENAS UMA LINHA PARA INCREMENTAR O METODO, COMO É VISTO ABAIXO:
        public async Task<List<User>> GetAll() =>  await _context.Users.ToListAsync();

        public async Task<User> GetById(int id)
        {
            User userDb = await _context.Users
               .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
            {
                throw new Exception($"User {id} not found");
            }

            return userDb;
        }

        public async Task Update(User userIn, int id)
        {
            if (userIn.Id != id)
                throw new Exception("Route id differs user id");

            User userDb = await _context.Users
                .AsNoTracking()
                 .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
            {
                throw new Exception($"User {id} not found");
            }

            //ENTRY() E ()ENTITYSTATE.MODIFIED PARA METODOS UPDATE
            _context.Entry(userIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
