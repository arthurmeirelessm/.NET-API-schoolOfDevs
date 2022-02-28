using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Dto.Note;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Exceptions;
using SchoolOfDevs.Helpers;


// NO SERVICES TEREMOS O CÉREBRO DA API, ONDE FAZEMOS TODOS OS PUSHS DE ELEMENTOS DO SQL SERVER E MONTAR COMO UMA CONSULTA
namespace SchoolOfDevs.Services
{

    public interface INoteService
    {
        public Task<NoteResponse> Create(NoteRequest note);
        public Task<NoteResponse> GetById(int id);
        public Task<List<NoteResponse>> GetAll();
        public Task Update (NoteRequest userIn, int id);
        public Task Delete(int id);

    }
    public class NoteService : INoteService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public NoteService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NoteResponse> Create(NoteRequest noteRequest)
        {

            Note note = _mapper.Map<Note>(noteRequest);
            _context.Notes.Add(note);
           await _context.SaveChangesAsync();

            return _mapper.Map<NoteResponse>(note);
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
        public async Task<List<NoteResponse>> GetAll()
        {
              List<Note> notes = await _context.Notes.ToListAsync();
            return notes.Select(x => _mapper.Map<NoteResponse>(x)).ToList();
        }

        public async Task<NoteResponse> GetById(int id)
        {
            Note noteDb = await _context.Notes
               .SingleOrDefaultAsync(u => u.Id == id);

            if (noteDb is null)
            {
                throw new KeyNotFoundException($"Note {id} not found");
            }

            return _mapper.Map<NoteResponse>(noteDb);
        }

        public async Task Update(NoteRequest noteRequest, int id)
        {
            if (noteRequest.Id != id)
                throw new BadRequestException("Route id differs Note id");

            Note noteDb = await _context.Notes
                .AsNoTracking()
                 .SingleOrDefaultAsync(u => u.Id == id);

            if (noteDb is null)
            {
                throw new KeyNotFoundException($"Note {id} not found");
            }

            noteDb = _mapper.Map<Note>(noteRequest);

            //ENTRY() E ()ENTITYSTATE.MODIFIED PARA METODOS UPDATE
            _context.Entry(noteRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
