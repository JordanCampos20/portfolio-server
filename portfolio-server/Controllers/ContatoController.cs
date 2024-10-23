using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using portfolio_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using portfolio_server.ViewModels;
using portfolio_server.Services;

namespace portfolio_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ContatoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ContatoServices _contatoServices;
        
        public ContatoController(IMapper mapper, ContatoServices contatoServices)
        {
            _mapper = mapper;
            _contatoServices = contatoServices; 
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ContatoDTO contatoDTO)
        {
            var t = _mapper.Map<Contato>(contatoDTO);

            var z = await _contatoServices.PostContato(t);

            if (z == null)
                return NoContent();

            var r = _mapper.Map<ContatoDTO>(z);

            return Ok(new ResponseViewModel("Enviado com sucesso!", 200));
        }
    }
}