using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IDatingRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repository.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repository.GetUser(id);

            if (userFromRepo == null)
            {
                return NotFound($"Could not find user with an ID if {id}");
            }

            if (loggedInUserId != userFromRepo.Id)
            {
                return Unauthorized();
            }

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user {id} failed on save");
        }
//
//        // POST: api/Users
//        [HttpPost]
//        public async Task<IActionResult> PostUser([FromBody] User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();
//
//            return CreatedAtAction("GetUser", new { id = user.Id }, user);
//        }
//
//        // DELETE: api/Users/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteUser([FromRoute] int id)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
//            if (user == null)
//            {
//                return NotFound();
//            }
//
//            _context.Users.Remove(user);
//            await _context.SaveChangesAsync();
//
//            return Ok(user);
//        }
//
//        private bool UserExists(int id)
//        {
//            return _context.Users.Any(e => e.Id == id);
//        }
    }
}