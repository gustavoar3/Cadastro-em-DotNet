using SistemaDeCadastro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SistemaDeCadastro.Data;
using SistemaDeCadastro.Models.DTO;

namespace SistemaDeCadastro.Controllers
{
    [ApiController]
    [Route("cadastro")]
    public class CadastroController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CadastroController(AppDbContext appDbContext)
            => _appDbContext = appDbContext;

        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<Pessoa>>> GetPessoas()
        {
            var pessoas = await _appDbContext
                .Pessoas
                .ToListAsync();

            if (pessoas == null) BadRequest();
            return Ok(pessoas);
        }

        [HttpGet]
        [Route("cpf")]
        public async Task<ActionResult<List<Pessoa>>> GetCpf(int id)
        {
            var pessoas = await _appDbContext
                .Pessoas
                .Where(p => p.Id == id)
                .ToListAsync();

            if (pessoas == null) BadRequest();
            return Ok(pessoas);
        }

        [HttpGet]
        [Route("aniversario")]
        public async Task<ActionResult<List<Pessoa>>> GetAniversario()
        {
            var pessoas = await _appDbContext
                .Pessoas
                .ToListAsync();

            var agora = DateTime.Now.DayOfYear;
            var niver = new List<string>();

            foreach (var pessoa in pessoas)
            {
                var data = Convert.ToDateTime(pessoa.Nascimento);
                if (data.DayOfYear >= agora && data.DayOfYear < agora + 7)
                {
                    niver.Add(pessoa.Cpf);
                }
            }

            if (niver == null) BadRequest();
            return Ok(niver);
        }

        [HttpPost("incluir")]
        public async Task<ActionResult<Pessoa>> AddCadastro(PessoaDTO cadastro)
        {
            var newPessoa = new Pessoa
            {
                Nome = cadastro.Nome,
                Cpf = cadastro.Cpf,
                Nascimento = cadastro.Nascimento,
                Tipo = cadastro.Tipo
            };

            _appDbContext.Pessoas.Add(newPessoa);
            await _appDbContext.SaveChangesAsync();

            return Ok(newPessoa);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteCadastro(int id)
        {
            var pessoa = _appDbContext.Pessoas
                .Find(id);
            if (pessoa == null) return BadRequest();

            _appDbContext.Pessoas.Remove(pessoa);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
