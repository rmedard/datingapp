using System;
using System.Collections.Generic;
using System.Linq;
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
            var user = await _repository.GetUser(id);

//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
//
//            if (user == null)
//            {
//                return NotFound();
//            }

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        // PUT: api/Users/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            if (id != user.Id)
//            {
//                return BadRequest();
//            }
//
//            _context.Entry(user).State = EntityState.Modified;
//
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!UserExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//
//            return NoContent();
//        }
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