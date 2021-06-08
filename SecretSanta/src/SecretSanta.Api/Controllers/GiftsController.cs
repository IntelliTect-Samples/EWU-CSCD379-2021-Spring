using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SecretSanta.Business;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private IGiftRepository GiftRepository { get; }

        public GiftsController(IGiftRepository repository)
        {
            GiftRepository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Dto.Group), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Dto.Gift?>> Post([FromBody] Dto.Gift gift)
        {
            return Dto.Gift.ToDto(await GiftRepository.Create(Dto.Gift.FromDto(gift)));
        }

        [HttpGet("{userId}")]
        public IEnumerable<Dto.Gift> Get(int userId)
        {
            return GiftRepository.List(userId).Select(item => Dto.Gift.ToDto(item)!);
        }
    }
}
