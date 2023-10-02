using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FProjectCampingBackend.Models.Infra.FileHelper
{
	public class FileStyleValidator : IFileValidator
	{
		public void Validate(HttpPostedFileBase file)
		{
			if (file == null || file.ContentLength == 0)
			{
				throw new Exception("請選擇一個檔案。");
			}

			// 檢查檔案類型
			string fileExtension = Path.GetExtension(file.FileName).ToLower();

			// 圖片類型驗證
			if (IsImageFile(fileExtension)) return;
			//{
			//	throw new Exception("傳回圖片檔");
			//}

			// 壓縮檔類型驗證
			else if (IsArchiveFile(fileExtension))
			{
				throw new Exception("傳回壓縮檔");
			}

			// 文件檔類型驗證
			else if (IsDocumentFile(fileExtension))
			{
				throw new Exception("傳回文件檔");
			}
			else
			{
				throw new Exception("不支援的檔案類型。");
			}
		}

		private bool IsImageFile(string fileExtension)
		{
			string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
			return imageExtensions.Contains(fileExtension);
		}

		private bool IsArchiveFile(string fileExtension)
		{
			string[] archiveExtensions = { ".zip", ".rar", ".7z" };
			return archiveExtensions.Contains(fileExtension);
		}

		private bool IsDocumentFile(string fileExtension)
		{
			string[] documentExtensions = { ".pdf", ".doc", ".docx", ".txt" };
			return documentExtensions.Contains(fileExtension);
		}
	}
}
