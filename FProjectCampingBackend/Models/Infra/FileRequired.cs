using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FProjectCampingBackend.Models.Infra.FileHelper
{
	public class FileRequired : IFileValidator
	{
		public void Validate(HttpPostedFileBase file)
		{
			if (file == null || file.ContentLength == 0)
			{
				throw new Exception("請選擇檔案");
			}
		}
	}
	public class FileSizeValidator : IFileValidator
	{
		private readonly int _maxBytes;

		public FileSizeValidator(int maxKBs)
		{
			if (maxKBs <= 0)
			{
				throw new ArgumentOutOfRangeException("maxKBs must be greater than zero");
			}
			_maxBytes = maxKBs * 1024;
		}

		public void Validate(HttpPostedFileBase file)
		{
			if (file == null || file.ContentLength <= 0)
			{
				// 如果檔案為空，不符合檔案大小驗證規則
				throw new Exception("請選擇檔案");
			}

			if (file.ContentLength > _maxBytes)
			{
				throw new Exception("檔案大小超過限制。");
			}
		}



		public class ImageValidator : IFileValidator
		{
			public void Validate(HttpPostedFileBase file)
			{
				if (file == null || file.ContentLength == 0)
				{
					return;
				}

				string[] imgExts = { ".jpg", ".jpeg", ".png" };
				string ext = Path.GetExtension(file.FileName).ToLower();

				if (!imgExts.Contains(ext))
				{
					throw new Exception("請上傳有效的圖片文件");
				}

			}


		}
	}
}
