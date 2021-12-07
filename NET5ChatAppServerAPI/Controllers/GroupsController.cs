using System.Collections.Generic;
using System.Linq;
using System;

using Microsoft.AspNetCore.Mvc;
using NET5ChatAppServerAPI.Models;
using NET5ChatAppServerAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace NET5ChatAppServerAPI.Controllers
{
	public class GroupsController : BaseApiController
	{
		private readonly ChatAppDbContext _context;

		public GroupsController(ChatAppDbContext context)
		{
			this._context = context;
		}

		[HttpGet]
		public async Task<object> Index()
		{
			var records = from o in this._context.Groups
						  select o;

			if (records.Any() == false)
			{
				return "not found";
			}

			return await records.AsNoTrackingWithIdentityResolution().ToListAsync();	
		}

		[HttpGet]
		public async Task<object> Get()
		{
			var records = from o in this._context.Groups
						  select o;

			if (records.Any() == false)
			{
				return null;
			}

			return await records.AsNoTrackingWithIdentityResolution().ToListAsync();
		}
	}
}
