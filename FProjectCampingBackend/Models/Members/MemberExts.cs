﻿using FProjectCampingBackend.Models.EFModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Member = FProjectCampingBackend.Models.EFModels.Member;

namespace FProjectCampingBackend.Models.ViewModels.Members
{
	public static class MemberExts
	{
		public static MemberVm ToMemberVm(this Member member)
		{
			if (member == null)
			{
				return null;
			}

			return new MemberVm
			{
				Id = member.Id,
				Name = member.Name,
				Account = member.Account,
				Photo = member.Photo,
				Email = member.Email,
				PhoneNum = member.PhoneNum,
				CreatedTime = member.CreatedTime,
				Enabled = member.Enabled,
				IsConfirmed = member.IsConfirmed,
			};
		}
	}
}