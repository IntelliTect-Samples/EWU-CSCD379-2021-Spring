using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;

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
        public IEnumerable<User> Get()
        {
            return Repository.List();
        }

        [HttpGet("{id}")]
        public ActionResult<User?> Get(int id)
        {
            User? user = Repository.GetItem(id);
            if (user is null) return NotFound();
            return user;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (Repository.Remove(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<User?> Post([FromBody] User? user)
        {
            if (user is null)
            {
                return BadRequest();
            }
            return Repository.Create(user);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User? user)
        {
            if (user is null)
            {
                return BadRequest();
            }
            
            User? foundUser = Repository.GetItem(id);
            if (foundUser is not null)
            {
                foundUser.FirstName = user.FirstName;
                foundUser.LastName = user.LastName;

                Repository.Save(foundUser);
                return Ok();
            }
            return NotFound();
        }
    }
}
