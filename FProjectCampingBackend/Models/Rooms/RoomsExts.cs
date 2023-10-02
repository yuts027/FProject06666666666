using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.ViewModels.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FProjectCampingBackend.Models.ViewModels.Rooms
{

	public static class RoomsExts
	{
		public static RoomsVm ToRoomsVm(this Room room)
		{
			if (room == null)
			{
				return null;
			}

			return new RoomsVm
			{
				Id = room.Id,
				RoomTypeName = room.RoomType.Name,
				RoomName = room.RoomName,
				WeekendPrice = room.WeekendPrice,
				WeekdayPrice = room.WeekdayPrice,
				Description = room.Description,
				Stock = room.Stock,
				Photo = room.Photo
			};
		}
	}
}