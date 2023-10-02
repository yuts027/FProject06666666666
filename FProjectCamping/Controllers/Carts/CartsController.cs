using FProjectCamping.Models.Services;
using FProjectCamping.Models.ViewModels.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace FProjectCamping.Controllers.Carts
{
	public class CartsController : Controller
	{
		private readonly Models.Services.ProfileService _profileService = new Models.Services.ProfileService();
		private readonly CartService _cartService = new CartService();
		private readonly OrderService _orderService = new OrderService();

		[Authorize]
		public ActionResult Cart()
		{
			// 登入者
			var account = User.Identity.Name;
			var model = _cartService.Get(account);

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Cart(CartVm vm)
		{
			if (vm.Items == null)
			{
				return View(vm);
			}

			// Todo: 新增一隻刪除action 並重新渲染畫面
			_cartService.Update(vm);

			return RedirectToAction("OrderInfo");
		}

		[Authorize]
		public ActionResult OrderInfo()
		{
			var account = User.Identity.Name;
			var cart = _cartService.Get(account);

			// 預設帶入 訂房人資料
			cart.ContactProfile = _profileService.GetMemberProfile(account);
			return View(cart);
		}

		[Authorize]
		[HttpPost]
		public ActionResult OrderInfo(CartVm vm)
		{
			if (!ModelState.IsValid) return View(vm);

			// 檢查購物車要有項目 才能後續動作
			if (!vm.Items.Any()) { return View(vm); }

			var account = User.Identity.Name;
			//var cart = _cartService.GetOrCreateCart(account);
			var cart = _cartService.Get(account);
			cart.AllowCheckout = cart.Items.Any();

			if (!cart.AllowCheckout)
			{
				ModelState.AddModelError("", "購物車是空的,無法進行結帳");
				return View(vm);
			}

			cart.ContactProfile = vm.ContactProfile;
			cart.Memo = vm.Memo;
			cart.PaymentType = vm.PaymentType;

			foreach (var item in cart.Items)
			{
				item.RoomName = vm.Items.FirstOrDefault(x => x.Id == item.Id).RoomName;
			}

			#region *檢查要建立的訂單是否已經被建立過

			var repeatItem = _orderService.CheckCanCreate(cart);
			if (repeatItem.Any())
			{
				var textBuilder = new StringBuilder();
				textBuilder.AppendLine("您預定的房間，含有以下幾間已無空房的房號：");

				foreach (var item in repeatItem)
				{
					textBuilder.AppendLine($"{item.Room.RoomType.Name} {item.Room.RoomName} {item.CheckInDate:yyyy-MM-dd} - {item.CheckOutDate:yyyy-MM-dd}");
				}

				ViewBag.RepeatItems = textBuilder.ToString();
				return View(cart);
			}

			#endregion *檢查要建立的訂單是否已經被建立過

			var orderId = _cartService.ProcessCheckout(account, cart);
			return RedirectToAction("Pay", "Orders", new { orderId = orderId }); // 轉導到 Orders/Pay 並帶入參數: orderId
		}

		/// <summary>
		/// 將 CartItem加入購物車
		/// </summary>
		/// <param name="vm"></param>
		/// <returns></returns>
		[Authorize]
		[HttpPost]
		public string AddItem(CartItemsVm vm)
		{
			string buyer = User.Identity.Name; // 買家帳號
			var result = _cartService.AddToCart(buyer, vm); //加入購物車

			return result; //沒傳回任何東西
		}

		public void DeleteCartItem(int cartItemId)
		{
			_cartService.DeleteCartItem(cartItemId);
		}
	}
}