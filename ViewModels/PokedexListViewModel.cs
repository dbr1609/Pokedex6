using Pokedex6.Controllers;
using Pokedex6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex6.ViewModels
{
    public class PokedexListViewModel
    {
        public string PageGenerated { get; set; }
        public IEnumerable<Pokemon> Pokemons { get; set; }
        public SortField SortField { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
