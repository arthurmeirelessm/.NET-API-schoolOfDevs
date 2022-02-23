using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Exceptions;
using SchoolOfDevs.Helpers;


// NO SERVICES TEREMOS O CÉREBRO DA API, ONDE FAZEMOS TODOS OS PUSHS DE ELEMENTOS DO SQL SERVER E MONTAR COMO UMA CONSULTA
namespace SchoolOfDevs.Services
{

    public interface INoteService
    {
        public Task<Note> Create(Note note);
        public Task<Note> GetById(int id);
        public Task<List<Note>> GetAll();
        public Task Update (Note noteIn, int id);
        public Task Delete(int id);

    }
    public class NoteService : INoteService
    {
        private readonly DataContext _context;

        public NoteService(DataContext context)
        {
            _context = context;
        }

        public async Task<Note> Create(Note note)
        {
            _context.Notes.Add(note);
           await _context.SaveChangesAsync();

            return note;
        }

        public async Task Delete(int id)
        {
            Note noteDb = await _context.Notes
                .SingleOrDefaultAsync(u => u.Id == id);

            if (noteDb is null)
            {
                throw new KeyNotFoundException($"Note {id} not found");
            }
            //CASO NÃO EXISTA O ID PASSADO, ELE IRÁ REMOVER COMO É FEITO ABAIXO PELO METODO 'REMOVE()'
            _context.Notes.Remove(noteDb);
            await _context.SaveChangesAsync();
        }

        //BUSCA A TABELA TODA E RETORNA COMO LISTA
        //NO GETTALL É USADO ARROW FUNCTION PELO FATO DE TER PRECISO DE APENAS UMA LINHA PARA INCREMENTAR O METODO, COMO É VISTO ABAIXO:
        public async Task<List<Note>> GetAll() =>  await _context.Notes.ToListAsync();

        public async Task<Note> GetById(int id)
        {
            Note noteDb = await _context.Notes
               .SingleOrDefaultAsync(u => u.Id == id);

            if (noteDb is null)
            {
                throw new KeyNotFoundException($"Note {id} not found");
            }

            return noteDb;
        }

        public async Task Update(Note userIn, int id)
        {
            if (userIn.Id != id)
                throw new BadRequestException("Route id differs Note id");

            Note userDb = await _context.Notes
                .AsNoTracking()
                 .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
            {
                throw new KeyNotFoundException($"Note {id} not found");
            }

            userIn.CreatedAt = userDb.CreatedAt;

            //ENTRY() E ()ENTITYSTATE.MODIFIED PARA METODOS UPDATE
            _context.Entry(userIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
