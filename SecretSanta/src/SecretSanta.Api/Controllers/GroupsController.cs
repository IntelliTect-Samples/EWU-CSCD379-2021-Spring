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
    public class GroupsController : ControllerBase
    {
        private IGroupRepository GroupRepository { get; }
        public IUserRepository UserRepository { get; }

        public GroupsController(IGroupRepository repository, IUserRepository userRepository)
        {
            GroupRepository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            UserRepository = userRepository ?? throw new System.ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public IEnumerable<Dto.Group> Get()
        {
            return GroupRepository.List().Select(x => Dto.Group.ToDto(x)!);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dto.Group?>> Get(int id)
        {
            Dto.Group? group = Dto.Group.ToDto(await GroupRepository.GetItem(id), true);
            if (group is null) return NotFound();
            foreach(Dto.User user in 
                (await GroupRepository.GetUsers(id)).Select(item => Dto.User.ToDto(item)!))
            {
                group.Users.Add(user);
            }
            foreach(Dto.Assignment assignment in 
                GroupRepository.GetAssignments(id).Select(item => Dto.Assignment.ToDto(item)!))
            {
                group.Assignments.Add(assignment);
            }
            return group;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await GroupRepository.Remove(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Dto.Group), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Dto.Group?>> Post([FromBody] Dto.Group group)
        {
            return Dto.Group.ToDto(await GroupRepository.Create(Dto.Group.FromDto(group)!));
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Put(int id, [FromBody] Dto.UpdateGroup? group)
        {
            Data.Group? foundGroup = await GroupRepository.GetItem(id);
            if (foundGroup is not null)
            {
                foundGroup.Name = group?.Name ?? "";

                await GroupRepository.Save(foundGroup);
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("{id}/remove")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Remove(int id, [FromBody] int userId)
        {
            if(await GroupRepository.RemoveUser(id, userId))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("{id}/add")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Add(int id, [FromBody] int userId)
        {
            if(await GroupRepository.AddUser(id, userId)){
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("{id}/assign")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> CreateAssignments(int id)
        {
            AssignmentResult result = await GroupRepository.GenerateAssignments(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
    }
}
