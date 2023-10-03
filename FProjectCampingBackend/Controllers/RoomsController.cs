using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FProjectCampingBackend.Models;
using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.Infra.FileHelper;
using FProjectCampingBackend.Models.Repostories;
using FProjectCampingBackend.Models.Rooms;
using FProjectCampingBackend.Models.Services;
using FProjectCampingBackend.Models.ViewModels;
using FProjectCampingBackend.Models.ViewModels.Members;
using FProjectCampingBackend.Models.ViewModels.Rooms;
using PagedList;
using static FProjectCampingBackend.Models.Infra.FileHelper.FileSizeValidator;

namespace FProjectCampingBackend.Controllers
{
	public class RoomsController : Controller
	{
		private AppDbContext db = new AppDbContext();
		private readonly DropdownListService _dropdownListService = new DropdownListService();

		// GET: Rooms

		public ActionResult Index(SearchRoomsVm vm, int page = 1, int pageSize = 6)
		{


			ViewData["RoomTypeId"] = new DropdownListService().PrepareRoomTypeData();
			var repo = new RoomsRepository(db);
			IQueryable<Room> rooms = repo.GetRooms(vm);

			var result = new List<RoomsVm>();

			foreach (var r in rooms)
			{

				var roomsVm = new RoomsVm
				{
					Id = r.Id,
					RoomName = r.RoomName,
					WeekendPrice = r.WeekendPrice,
					WeekdayPrice = r.WeekdayPrice,
					Description = r.Description,
					RoomTypeName = r.RoomType.Name,
					Stock = r.Stock,
					Photo = r.Photo,


				};
				result.Add(roomsVm);

			}

			var pagedRooms = result.OrderBy(x => x.Id).ToPagedList(page, pageSize);
			return View(pagedRooms);
		}


		public ActionResult Upload()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Upload(int Id, HttpPostedFileBase myfile)
		{
			var db = new AppDbContext();

			// 根據roomId 取得 Room 對象
			var room = db.Rooms.FirstOrDefault(p => p.Id == Id);

			if (room == null)
			{
				return View();
			}

			// 檢查是否有選擇檔案
			if (myfile != null && myfile.ContentLength > 0)
			{
				// 取得檔案名稱
				string fileName = Path.GetFileName(myfile.FileName);

				// 將檔案名稱存儲到 Room 對象的 Photo 屬性中
				room.Photo = fileName;

				// 保存到資料庫
				db.SaveChanges();
			}
			else
			{
				ModelState.AddModelError("myfile", "請選擇要上傳的檔案。");
			}

			return View();
		}
		//public ActionResult Upload(int Id, HttpPostedFileBase myfile)
		//{
		//	var db = new AppDbContext();

		//	//根據memberId,confirmCode取得Member
		//	var member = db.Rooms.FirstOrDefault(p => p.Id == Id);
		//	if (member == null)
		//	{
		//		return View();
		//	}

		//	string fileName;

		//	string path = Server.MapPath("~/files");
		//	IFileValidator[] validators = Verificationprofile();
		//	try
		//	{
		//		// 檢查是否有選擇檔案
		//		if (myfile != null && myfile.ContentLength > 0)
		//		{
		//			// 儲存檔案，並取得檔案名稱
		//			fileName = UploaderFileHelper.Save(myfile, path, validators);

		//			// 複製一份到前台網站的資料夾下
		//			Copyfile(fileName, path);
		//			member.Photo = fileName;
		//		}
		//		else
		//		{
		//			ModelState.AddModelError("myfile", "請選擇要上傳的檔案。");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		ModelState.AddModelError("myfile", ex.Message);
		//	}
		//	//將他更新為已確認

		//	db.SaveChanges();

		//	return View();
		//}

		// GET: Rooms/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Room room = db.Rooms.Find(id);
			if (room == null)
			{
				return HttpNotFound();
			}
			return View(room);
		}

		// GET: Rooms/Create
		public ActionResult Create()
		{
			ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name");
			return View();


		}

		// POST: Rooms/Create
		// 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
		// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(RoomsVm vm, HttpPostedFileBase myfile)
		{

			var a = ModelState.IsValid;

			ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", vm.RoomTypeId);
			string fileName;
			string path = Server.MapPath("~/files");


			IFileValidator[] validators = Verificationprofile();

			try
			{
				fileName = UploaderFileHelper
					.Save(myfile, path, validators);

				//copy一份到前台網站的資料夾下
				Copyfile(fileName, path);

			}
			catch (Exception ex)
			{
				ModelState.AddModelError("myfile", ex.Message);
				return View(vm);
			}

			vm.Photo = fileName;

			Room rooms = new Room
			{
				Id = vm.Id,
				RoomTypeId = vm.RoomTypeId,
				RoomName = vm.RoomName,
				Description = vm.Description,
				WeekendPrice = vm.WeekendPrice,
				WeekdayPrice = vm.WeekdayPrice,
				Status = vm.Status,
				Stock = vm.Stock,
				Photo = vm.Photo
	
			};


			if (ModelState.IsValid)
			{
				db.Rooms.Add(rooms);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(vm);

		}

		private static IFileValidator[] Verificationprofile()
		{
			IFileValidator[] validators =
							new IFileValidator[]
							{
					new FileRequired(),//必填
                    new FileStyleValidator(),//必須是圖檔
                    new FileSizeValidator(1024),//1MB

							};
			return validators;
		}

		private static void Copyfile(string fileName, string path)
		{
			string sourceFullPath = Path.Combine(path, fileName);
			string dest = System.Configuration
				.ConfigurationManager
				.AppSettings["frontSiteRootPath"];

			string destFullPath = Path.Combine(dest, fileName);

			System.IO.File.Copy(sourceFullPath, destFullPath, true);
		}

		// GET: Rooms/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Room room = db.Rooms.Find(id);
			if (room == null)
			{
				return HttpNotFound();
			}
			ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", room.RoomTypeId);
			return View(room);
		}

		// POST: Rooms/Edit/5
		// 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
		// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(RoomsEditVM vm, HttpPostedFileBase myfile)
		{
			if (ModelState.IsValid)
			{
				// 获取现有房间信息
				Room existingRoom = db.Rooms.Find(vm.Id);

				// 更新允许修改的字段
				if (vm.WeekendPrice.HasValue)
				{
					existingRoom.WeekendPrice = vm.WeekendPrice.Value;
				}
				if (!string.IsNullOrWhiteSpace(vm.RoomName))
				{
					existingRoom.RoomName = vm.RoomName;
				}

				if (vm.WeekdayPrice.HasValue)
				{
					existingRoom.WeekdayPrice = vm.WeekdayPrice.Value;
				}

				if (!string.IsNullOrWhiteSpace(vm.Description))
				{
					existingRoom.Description = vm.Description;
				}

				// 处理上传的新照片（如果有的话）
				if (myfile != null && myfile.ContentLength > 0)
				{
					string path = Server.MapPath("~/files");
					IFileValidator[] validators = Verificationprofile();
					try
					{
						string fileName = UploaderFileHelper.Save(myfile, path, validators);
						existingRoom.Photo = fileName;
						// 删除旧照片，可以根据需要实现
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("myfile", ex.Message);
						ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", vm.RoomTypeId);
						return View(vm);
					}
				}

				// 保存更新后的房间信息
				db.Entry(existingRoom).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index");
			}

			// 模型状态无效，返回编辑视图
			ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", vm.RoomTypeId);
			return View(vm);
		}


		//// GET: Rooms/Edit/5
		//public ActionResult Edit(int? id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	Room room = db.Rooms.Find(id);
		//	if (room == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", room.RoomTypeId);
		//	return View(room);
		//}

		//// POST: Rooms/Edit/5
		//// 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
		//// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
		//[HttpPost]
		//[ActionName("EditRoomDetails")]
		//public ActionResult EditRoomDetails([Bind(Include = "Id,RoomTypeId,RoomName,Description,WeekendPrice,WeekdayPrice,Status,Stock,Photo")] Room room)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		db.Entry(room).State = EntityState.Modified;
		//		db.SaveChanges();
		//		return RedirectToAction("Index");
		//	}
		//	ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", room.RoomTypeId);
		//	return View(room);
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//[ActionName("EditRoomPhoto")]
		//public ActionResult EditRoomPhoto(string photo, HttpPostedFileBase myfile)
		//{
		//	string fileName;
		//	string path = Server.MapPath("~/Files");
		//	IFileValidator[] validators =
		//	{
		//		new FileRequired(),
		//		new ImageValidator(),
		//		new FileSizeValidator(1024),
		//	};


		//	try
		//	{
		//		fileName = UploaderFileHelper.Save(myfile, path, validators);

		//		string sourceFullPath = Path.Combine(path, fileName);

		//		string dest = System.Configuration.ConfigurationManager.AppSettings["frontSiteRootPath"];
		//		string destFullPath = Path.Combine(dest, fileName);

		//		System.IO.File.Copy(sourceFullPath, destFullPath, true);

		//		// 將檔名存入 photo
		//		photo = fileName;

		//		//將紀錄存
		//		if (!ModelState.IsValid)
		//		{
		//			return View();
		//		}

		//		Edit(int.Parse(photo));
		//	}
		//	catch (Exception ex)
		//	{
		//		ModelState.AddModelError("myfile", ex.Message);
		//		// 在這裡處理文件上傳或處理錯誤，但不要影響 member 的值
		//		return View();
		//	}

		//	return RedirectToAction("Index");
		//}

		//private string EditPhoto(string photo, string account)
		//{
		//	var db = new AppDbContext();
		//	var memberInDb = db.Members.FirstOrDefault(p => p.Account == account);
		//	if (memberInDb.Account != account)
		//	{
		//		throw new Exception("您沒有權限修改別人資料");
		//	}

		//	memberInDb.Photo = photo;
		//	db.SaveChanges();
		//	return photo;
		//}

		//private string Edit(string photo, string roomName)
		//{
		//	var db = new AppDbContext();
		//	var RoomsInDb = db.Rooms.FirstOrDefault(p => p.RoomName == roomName);
		//	if (RoomsInDb.RoomName != roomName)
		//	{
		//		throw new Exception("您沒有權限修改別人資料");
		//	}

		//	RoomsInDb.Photo = photo;
		//	db.SaveChanges();
		//	return photo;
		//}

		// GET: Rooms/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Room room = db.Rooms.Find(id);
			if (room == null)
			{
				return HttpNotFound();
			}
			return View(room);
		}

		// POST: Rooms/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var room = db.Rooms.Find(id);

			var cartItemsToRemove = db.CartItems.Where(c => c.RoomId == id).ToList();
			db.CartItems.RemoveRange(cartItemsToRemove);

			var orderItemsToRemove = db.OrderItems.Where(o => o.RoomId == id).ToList();
			db.OrderItems.RemoveRange(orderItemsToRemove);

			db.Rooms.Remove(room);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
