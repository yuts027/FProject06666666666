using FProjectCamping.Models.EFModels;
using FProjectCamping.Models.Respositories;
using FProjectCamping.Models.ViewModels.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using static FProjectCamping.MvcApplication;

namespace FProjectCamping.Models.Services
{
	public class CartService
	{
		private readonly OrderService _orderService = new OrderService();
		private readonly CartRepository _cartRepository = new CartRepository();
		private readonly CartItemsRepository _cartItemsRepository = new CartItemsRepository();
		private readonly RoomTypesRepository _roomTypesRepository = new RoomTypesRepository();
		private readonly RoomsRepository _roomsRepository = new RoomsRepository();

		public CartVm Get(string account)
		{
			var vm = new CartVm();

			var cart = _cartRepository.GetByMember(account);

			// 沒取到購物車資料防呆
			if (cart == null) return vm;

			var cartItem = _cartItemsRepository.Get(cart.Id);

			// 房型資料 TODO:也許後續改Join
			var roomTypes = _roomTypesRepository.GetAll();
			var rooms = _roomsRepository.GetAll();

			vm = AutoMapperHelper.MapperObj.Map<CartVm>(cart);
			vm.Items = AutoMapperHelper.MapperObj.Map<List<CartItemsVm>>(cartItem);

			foreach (var item in vm.Items)
			{
				var (roomName, roomTypeName, roomPrice, extraPrice) = GetRoomTypeName(roomTypes, rooms, item.RoomId);
				item.RoomName = roomTypeName;
				item.RoomNum = roomName;
				item.RoomPrice = roomPrice;
				item.ExtraPrice = extraPrice;
			}

			return vm;
		}

		public string AddToCart(string buyer, CartItemsVm vm)
		{
			string msg = "不可訂購重複的項目!";

			// 取得目前購物車主檔,若沒有就立即新增一筆並傳回
			CartVm cart = GetOrCreateCart(buyer);

			// 檢查是否有加入重複的明細檔，有的話，阻擋加入
			if (cart != null && !IsRepeat(cart, vm))
			{
				var entity = AutoMapperHelper.MapperObj.Map<CartItem>(vm);
				entity.CartId = cart.Id;

				//加入購物車,若明細不存在就新增一筆,若存在就更新數量
				_cartItemsRepository.AddCartItem(entity);

				msg = "加入購物車~";
			}

			return msg;
		}

		/// <summary>
		/// 取得目前購物車主檔,若沒有就立即新增一筆並傳回
		/// </summary>
		/// <param name="buyer"></param>
		/// <returns></returns>
		public CartVm GetOrCreateCart(string buyer)
		{
			var cart = _cartRepository.GetByMember(buyer);
			//var cart = this.Get(buyer);
			// 沒有購物車紀錄，立即新增一筆並傳回
			if (cart == null)
			{
				var entity = new Cart { MemberAccount = buyer };
				var id = _cartRepository.AddNew(entity);

				return new CartVm
				{
					Id = id,
					MemberAccount = entity.MemberAccount,
					Items = new List<CartItemsVm>()
				};
			}

			// 傳回目前購物車主檔/明細檔紀錄

			//return cart;
			return new CartVm
			{
				Id = cart.Id,
				MemberAccount = cart.MemberAccount,
				TotalPrice = cart.TotalPrice,
				Items = AutoMapperHelper.MapperObj.Map<List<CartItemsVm>>(cart.CartItems),
			};
		}

		public void DeleteCartItem(int cartItemId)
		{
			_cartItemsRepository.Delete(cartItemId);
		}

		public int ProcessCheckout(string account, CartVm cart)
		{
			// 建立訂單主檔明細檔
			var id = _orderService.CreateOrder(account, cart);

			// 清空購物車
			_cartRepository.EmptyCart(account);

			return id;
		}

		/// <summary>
		/// 更新 CartItems 與 TotalPrice
		/// </summary>
		/// <param name="vm"></param>
		public void Update(CartVm vm)
		{
			_cartItemsRepository.UpdateExtraBed(vm);
			_cartRepository.UpdateTotalPrice(vm.Id, vm.TotalPrice);
		}

		/// <summary>
		/// 比較欲新增的明細檔是否重複
		/// </summary>
		/// <param name="cartVm"></param>
		/// <param name="vm"></param>
		/// <returns></returns>
		private bool IsRepeat(CartVm cart, CartItemsVm vm)
		{
			// 檢查邏輯: 只要涵蓋到入住日與退房日 都被視為重複
			DateTime vmCheckInDate = Convert.ToDateTime(vm.CheckInDate).Date;
			DateTime vmCheckOutDate = Convert.ToDateTime(vm.CheckOutDate).Date;

			return cart.Items.Any(item =>
			{
				DateTime itemCheckInDate = Convert.ToDateTime(item.CheckInDate).Date;
				DateTime itemCheckOutDate = Convert.ToDateTime(item.CheckOutDate).Date;

				return item.RoomId == vm.RoomId &&
					   itemCheckInDate <= vmCheckOutDate &&
					   itemCheckOutDate >= vmCheckInDate;
			});
		}

		private (string roomName, string roomTypeName, int roomPrice, int extraPrice) GetRoomTypeName(List<RoomType> roomTypes, List<Room> rooms, int roomId)
		{
			var room = rooms.FirstOrDefault(x => x.Id == roomId);

			if (room == null) throw new Exception($"未設定 RoomId : {roomId} 的對應資料");

			// TODO 取金額要依假日調整
			return (room.RoomName,
					roomTypes.FirstOrDefault(x => x.Id == room.RoomTypeId).Name,
					room.WeekdayPrice,
					Convert.ToInt32(roomTypes.FirstOrDefault(x => x.Id == room.RoomTypeId).ExtraBedPrice));
		}
	}
}