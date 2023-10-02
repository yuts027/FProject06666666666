using FProjectCampingBackend.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FProjectCampingBackend.Models.Rooms
{
	public class SearchRoomsVm
	{
		public int RoomTypeId { get; set; }
		public string RoomtypeName { get; set; }
	}
}