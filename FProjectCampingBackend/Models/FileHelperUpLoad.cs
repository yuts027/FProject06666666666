using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FProjectCampingBackend.Models
{
	public interface IFileValidator
	{
		void Validate(HttpPostedFileBase file);//若失敗,就傳回錯誤訊息
	}
	public static class UploaderFileHelper
	{
		public static string Save(HttpPostedFileBase file, string path, IFileValidator[] validators)
		{
			if (validators != null)
			{
				foreach (var validator in validators)
				{
					validator.Validate(file);
				}
			}
			//如果沒有檔案,就結束
			if (file == null || file.ContentLength == 0) return string.Empty;

			//取一個檔名
			string ext = Path.GetExtension(file.FileName);
			string fileName = Path.GetRandomFileName() + ext;
			string fullPath = Path.Combine(path, fileName);
			file.SaveAs(fullPath);

			return fileName;
		}
	}
}