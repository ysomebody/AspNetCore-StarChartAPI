using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route(""), ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext dbContext)
        {
            this._context = dbContext;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var star = _context.GetByIdWithSatellites(id);
            if (star == null)
            {
                return NotFound();
            }
            return Ok(star);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var stars = _context.GetByNameWithSatellites(name);
            if (!stars.Any())
            {
                return NotFound();
            }
            return Ok(stars);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var all = _context.GetAllWithSatellites();
            return Ok(all);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject star)
        {
            _context.CelestialObjects.Add(star);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id=star.Id }, star);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject star)
        {
            var s = _context.CelestialObjects.Find(id);
            if (s == null) return NotFound();

            s.Name = star.Name;
            s.OrbitalPeriod = star.OrbitalPeriod;
            s.OrbitedObjectId = star.OrbitedObjectId;
            _context.CelestialObjects.Update(s);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var s = _context.CelestialObjects.Find(id);
            if (s == null) return NotFound();

            s.Name = name;
            _context.CelestialObjects.Update(s);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var stars = from s in _context.CelestialObjects
                        where s.Id == id
                        select s;
            if (!stars.Any()) return NotFound();

            _context.CelestialObjects.RemoveRange(stars);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
