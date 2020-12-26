using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pokedex5.Models;
using Pokedex5.ViewModels;

namespace Pokedex5.Controllers
{
    public enum SortField
    {
        Id, Name, Type
    }

    public enum SortDirection
    {
        ASC, DESC
    }
    public class PokedexController : Controller
    {
        private IPokedex pokedex;
        private IConfiguration configuration;
        private ILogger<PokedexController> logger;
        public PokedexController(IConfiguration configuration, ILogger<PokedexController> logger, IPokedex pokedex)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.pokedex = pokedex;
        }
        
        public IActionResult Index([FromQuery] SortField sort = SortField.Id, [FromQuery] SortDirection sortDirection = SortDirection.ASC)
        {
            logger.LogInformation("A user has used the pokedex");
            IEnumerable<Pokemon> pokemons = pokedex.GetAll();
            switch (sort)
            {
                case SortField.Id:
                    pokemons = (sortDirection == SortDirection.ASC) ? pokemons.OrderBy(p => p.Id) : pokemons.OrderByDescending(p => p.Id);
                    break;
                case SortField.Name:
                    pokemons = (sortDirection == SortDirection.ASC) ? pokemons.OrderBy(p => p.Name.English) : pokemons.OrderByDescending(p => p.Name.English);
                    break;
                case SortField.Type:
                    pokemons = (sortDirection == SortDirection.ASC) ? pokemons.OrderBy(p => p.Type[0]) : pokemons.OrderByDescending(p => p.Type[0]);
                    break;
            }
            PokedexListViewModel pokedexListViewModel = new PokedexListViewModel
            {
                PageGenerated = pokedex.GeneratedAt.ToString(),
                Pokemons = pokemons,
                SortField = sort,
                SortDirection = sortDirection
            };

            return View(pokedexListViewModel);
        }

        public IActionResult Details(int id)
        {
            Pokemon pokemon = pokedex.Get(id);
            if (pokemon != null)
            {
                return View(pokemon);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Details(int id, Pokemon pokemon)
        {
            Pokemon p = pokedex.Get(id);
            
            if (ModelState.IsValid)
            {
                p.Base = pokemon.Base;
                TempData["Message"] = $"{p.Name.English} was updated!";
                this.pokedex.Save(p);
            }
            return View(p);
        }
    }
}
