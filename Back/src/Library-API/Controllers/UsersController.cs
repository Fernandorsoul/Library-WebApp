/*using Library_API.Data;
using Library_API.Models;
using Microsoft.AspNetCore.Mvc;


namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<ValuesController>
        private readonly DataContext _dataContext;
        public UsersController(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return _dataContext.Users.ToList();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var getById = _dataContext.Users.FirstOrDefault(x => x.Id == id);
            if (getById == null)
            {
                return NotFound();
            }
            return Ok(getById);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult<User> CreateUser ([FromBody] User user)
        {
            if (_dataContext.Users.FirstOrDefault(u=> u.Email.ToLower() == user.Email.ToLower())!=null) 
            {
                ModelState.AddModelError("CustomError", "Usuario ja cadastrado"!);
                return BadRequest(ModelState);
            }

            if (user == null) 
            {
                return NotFound();
            }

            if (user.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
          
            user.Id = _dataContext.Users.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            _dataContext.Users.Add(user);

            return CreatedAtRoute("CreateUser", new {id = user.Id}, User);

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if(user == null) 
            {
                return BadRequest();
            }
            var USER = _dataContext.Users.FirstOrDefault(x => x.Id == id);
            USER.Name = user.Name;
            USER.Email = user.Email;
            USER.Password = user.Password;
            
            _dataContext.SaveChanges();

            return Ok(user);

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
*/
using Library_API.Data;
using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return _dataContext.Users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var user = _dataContext.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if (_dataContext.Users.Any(u => u.Email.ToLower() == user.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Email já cadastrado.");
                return BadRequest(ModelState);
            }

            user.Id = 0; // Reset the Id to ensure it's a new user
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || id != user.Id)
                return BadRequest();

            var existingUser = _dataContext.Users.FirstOrDefault(x => x.Id == id);

            if (existingUser == null)
                return NotFound();

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;

            _dataContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}
