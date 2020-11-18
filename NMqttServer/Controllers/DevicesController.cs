using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using NMqttServer.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NMqttServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private ILiteDatabase _db;
        private ILiteCollection<Device> _col;

        public DevicesController(ILiteDatabase database)
        {
            _db = database;
            _col = _db.GetCollection<Device>("devices");

        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Device> Get()
        {
            return _col.FindAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{clientid}")]
        public Device Get(string clientid)
        {
            return _col.FindOne(x => x.ClientId == clientid);
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post(Device value)
        {
            var d = _col.FindOne(x => x.ClientId == value.ClientId);
            if(d != null)
            {
                return UnprocessableEntity("Object with the same ClientId already in the database.");
            }
            _col.Insert(value);
            return Ok();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
