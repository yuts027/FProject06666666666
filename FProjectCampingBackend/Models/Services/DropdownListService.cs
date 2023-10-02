using FProjectCampingBackend.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FProjectCampingBackend.Models.Services
{
    public class DropdownListService
    {
        public SelectList GetEnabledDropdownList()
        {
            var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "請選擇" },
            new SelectListItem { Value = "true", Text = "是" },
            new SelectListItem { Value = "false", Text = "否" },
        };
            return new SelectList(items, "Value", "Text");
        }

        public SelectList GetIsConfirmedDropdownList()
        {
            var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "請選擇" },
            new SelectListItem { Value = "true", Text = "是" },
            new SelectListItem { Value = "false", Text = "否" },
        };
            return new SelectList(items, "Value", "Text");
        }

		public SelectList GetRankingList()
		{
			var items = new List<SelectListItem>
		    {
			    new SelectListItem { Value = "1", Text = "建立時間" },
			    new SelectListItem { Value = "2", Text = "編號" },
			    new SelectListItem { Value = "3", Text = "姓名" },
		    };
			return new SelectList(items, "Value", "Text");
		}

		public SelectList PrepareRoomTypeData()
		{
			var db = new AppDbContext();
			var items = db.RoomTypes
					   .Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name })
					   .ToList()
					   .Prepend(new SelectListItem() { Value = "0", Text = "" });
			return new SelectList(items, "Value", "Text", null);
		}
	}
}