using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository Repository { get; }

        public UsersController(IUserRepository repository)
        {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IEnumerable<Dto.User> Get()
        {

            return Repository.List().Select(x => Dto.User.ToDto(x)!);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dto.User?>> Get(int id)
        {
            List<Dto.User>? userAssignments = new();
            foreach(SecretSanta.Data.User fullUser in 
                (await Repository.GetAssignmentUsers(id)))
            {
                userAssignments.Add(Dto.User.ToDto(fullUser));
            }
            List<Dto.Gift>? gifts = new();
            foreach(SecretSanta.Data.Gift gift in Repository.GetGifts(id))
            {
                gifts.Add(Dto.Gift.ToDto(gift));
            }
            Dto.User? user = Dto.User.ToDto(await Repository.GetItem(id), userAssignments, gifts);
            if (user is null) return NotFound();
            return user;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await Repository.Remove(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Dto.User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Dto.User?>> Post([FromBody] Dto.User user)
        {
            return Dto.User.ToDto(await Repository.Create(Dto.User.FromDto(user)!));
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Put(int id, [FromBody] Dto.UpdateUser? user)
        {
            Data.User? foundUser = await Repository.GetItem(id);
            if (foundUser is not null)
            {
                foundUser.FirstName = user?.FirstName ?? "";
                foundUser.LastName = user?.LastName ?? "";

                await Repository.Save(foundUser);
                return Ok();
            }
            return NotFound();
        }
    }
}
