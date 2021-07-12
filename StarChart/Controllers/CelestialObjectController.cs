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
            var star = _context.GetById(id);
            if (star == null)
            {
                return NotFound();
            }
            return Ok(star);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var stars = _context.GetByName(name);
            if (!stars.Any())
            {
                return NotFound();
            }
            return Ok(stars);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var all = _context.GetAll();
            return Ok(all);
        }
    }
}
