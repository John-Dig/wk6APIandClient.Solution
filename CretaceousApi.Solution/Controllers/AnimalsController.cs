using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CretaceousApi.Models;
using Newtonsoft.Json;

namespace CretaceousApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly CretaceousApiContext _context;

        public AnimalsController(CretaceousApiContext context)
        {
            _context = context;
        }

        // Pagination: https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-6.0
        /* public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
            var animals = from ani in _context.Animals
                        select ani; // NOTE! rebuilt FKA: _db = db; 
            switch (sortOrder)
            {
                case "name_desc":
                    animals = animals.OrderByDescending(ani => ani.Name);
                    break;
                case "Age":
                    animals = animals.OrderBy(ani => ani.Age);
                    break;
                case "age_desc":
                    animals = animals.OrderByDescending(ani => ani.Age);
                    break;
                default:
                    animals = animals.OrderBy(ani => ani.Name);
                    break;
            }
            return View(await animals.AsNoTracking().ToListAsync());
        }*/

        [Route("api/tryjson")] //remember this route is api/animals/api/tryjson 
        [HttpGet]
        public ActionResult<IEnumerable<Animal>> Get(int pageNumber = 1, int resultsPerPage = 2)
        {
            var query = _context.Animals.OrderBy(x => x.AnimalId);
            var totalResultCount = query.Count();
            var items = query.Skip((pageNumber - 1) * resultsPerPage).Take(resultsPerPage).ToList();
            var totalPages = (int)Math.Ceiling((double)totalResultCount / resultsPerPage);
            // $ dotnet add package Newtonsoft.Json --version 13.0.1 (v.2021)
            var metadata = new
            {
                totalResultCount,
                resultsPerPage,
                currentPage = pageNumber,
                totalPages
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return items.ToList();
           
        }


        // GET: api/Animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> Get( string species, string name, int minimumAge) //, int pageNumber = 1, int resultsPerPage = 2) //[FromQuery] // update params to handle query// & add'l q = name // [Range]: int minimumAge
        {
        // add search parameters to our Get() controller action so that we can request and retrieve filtered data.
        // http://localhost:5000/api/animals?species=dinosaur
            IQueryable<Animal> query = _context.Animals.AsQueryable();
            if ( species != null)
            {
                query = query.Where(entry => entry.Species == species);
            }
            // param: string name && q = name
            if ( name != null)
            {
                query = query.Where(entry => entry.Name == name); // http://localhost:5000/api/animals?species=dinosaur&name=matilda
            }
            // [Range=
            // ie. http://localhost:5000/api/animals?minimumAge=5
            if (minimumAge > 0)
            {
                query =  query.Where(entry => entry.Age >= minimumAge);
            }
        
        // public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        // {
            return await query.ToListAsync();
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return animal;
        }

        // PUT: api/Animals/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, Animal animal)
        {
            if (id != animal.AnimalId)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Animals
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimal", new { id = animal.AnimalId }, animal);
        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.AnimalId == id);
        }
    }
}
