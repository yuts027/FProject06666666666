using Dapper;
using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.Rooms;
using FProjectCampingBackend.Models.Services;
using FProjectCampingBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace FProjectCampingBackend.Models.Repostories
{
	public class RoomsRepository
	{

        //public IQueryable<Room> GetFilteredRooms(int roomTypeId, string name)
        //{
        //    IQueryable<Room> rooms = db.Rooms.Include(r => r.RoomType);

        //    if (roomTypeId > 0)
        //    {
        //        rooms = rooms.Where(r => r.RoomTypeId == roomTypeId);
        //    }

        //    if (!string.IsNullOrEmpty(name))
        //    {
        //        rooms = rooms.Where(r => r.RoomName == name);
        //    }

        //    return rooms;
        //}
        private readonly AppDbContext _dbContext;

        public RoomsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Room> GetRooms(SearchRoomsVm vm)
        {
            IQueryable<Room> rooms = _dbContext.Rooms;

            if (vm.RoomTypeId > 0)
            {
                rooms = rooms.Where(r => r.RoomTypeId == vm.RoomTypeId);
            }

            if (!string.IsNullOrEmpty(vm.RoomtypeName))
            {
                rooms = rooms.Where(r => r.RoomName == vm.RoomtypeName);
            }

            return rooms;
        }


    }
}

