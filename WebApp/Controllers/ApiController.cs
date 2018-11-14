using Microsoft.AspNetCore.Mvc;
using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
	[ApiController]
	[Route("api")]
	public class ApiController : Controller
	{
		public ApiController(FacadeService facadeService)
		{
			FacadeService = facadeService;
		}

		private FacadeService FacadeService { get; }

		[HttpGet("map")]
		public IEnumerable<KeyedItem<Guid, Map>> Map()
		{
			return FacadeService.Facade.GetMaps().ToKeyed().OrderBy(i => i.Value.Name);
		}

		[HttpGet("race")]
		public IEnumerable<KeyedItem<Guid, Race>> Race()
		{
			return FacadeService.Facade.GetBasis().Races.ToKeyed().OrderBy(i => i.Value.Name);
		}

		[HttpGet("player")]
		public IEnumerable<KeyedItem<Guid, Player>> Player(TeamSelection teamSelection)
		{
			return FacadeService.Facade.GetTeam(teamSelection).Players.ToKeyed().OrderBy(i => i.Value.Name);
		}
	}
}
