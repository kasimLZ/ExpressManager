using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Helper
{
	public class Uploader
	{
		private const string SuccessState = "SUCCESS";
		private string state = SuccessState;
		private string URL = null;
		private string uploadpath = null;
		private string filename = null;
		private string originalName = null;
		private HttpPostedFile uploadFile = null;


		/// <summary>
		/// The main method of uploading files
		/// </summary>
		/// <param name="cxt"></param>
		/// <param name="pathbase"></param>
		/// <param name="filetype"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public Hashtable UpFile(HttpContext cxt, string pathbase, string[] filetype, int size)
		{
			pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
			uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径

			try
			{
				uploadFile = cxt.Request.Files[0];
				originalName = uploadFile.FileName;

				//目录创建
				CreateFolder();

				//格式验证
				if (CheckType(filetype))
				{
					state = "不允许的文件类型";
				}
				//大小验证
				if (CheckSize(size))
				{
					state = "文件大小超出网站限制";
				}
				//保存图片
				if (state.Equals(SuccessState))
				{
					filename = NewName;
					uploadFile.SaveAs(uploadpath + filename);
					URL = pathbase + filename;
				}
			}
			catch (Exception e)
			{
				state = e.Message;
				URL = "";
			}
			return UploadHashInfo;
		}

		/// <summary>
		/// 上传涂鸦的主处理方法
		/// </summary>
		/// <param name="cxt"></param>
		/// <param name="pathbase"></param>
		/// <param name="tmppath"></param>
		/// <param name="base64Data"></param>
		/// <returns></returns>
		public Hashtable UpScrawl(HttpContext cxt, string pathbase, string tmppath, string base64Data)
		{
			pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
			uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
			FileStream fs = null;
			try
			{
				//创建目录
				CreateFolder();
				//生成图片
				filename = Guid.NewGuid() + ".png";
				fs = File.Create(uploadpath + filename);
				byte[] bytes = Convert.FromBase64String(base64Data);
				fs.Write(bytes, 0, bytes.Length);

				URL = pathbase + filename;
			}
			catch (Exception e)
			{
				state = e.Message;
				URL = "";
			}
			finally
			{
				fs.Close();
				DeleteFolder(cxt.Server.MapPath(tmppath));
			}
			return UploadHashInfo;
		}

		/// <summary>
		/// Get file information
		/// </summary>
		/// <param name="cxt"></param>
		/// <param name="field"></param>
		/// <returns></returns>
		public string GetOtherInfo(HttpContext cxt, string field)
		{
			string info = null;
			if (cxt.Request.Form[field] != null && !String.IsNullOrEmpty(cxt.Request.Form[field]))
			{
				info = field == "fileName" ? cxt.Request.Form[field].Split(',')[1] : cxt.Request.Form[field];
			}
			return info;
		}

		/// <summary>
		/// Get upload information
		/// </summary>
		/// <returns></returns>
		private Hashtable UploadHashInfo
		{
			get
			{
				return new Hashtable
				{
					{ "state", state },
					{ "url", URL },
					{ "originalName", originalName },
					{ "name", Path.GetFileName(URL) },
					{ "size", uploadFile.ContentLength },
					{ "type", Path.GetExtension(originalName) }
				};
			}
		}

		/// <summary>
		/// Rename file
		/// </summary>
		/// <returns></returns>
		private string NewName
		{
			get
			{
				return Guid.NewGuid() + GetFileExtension;
			}
		}

		/// <summary>
		/// File type detection
		/// </summary>
		/// <param name="filetype"></param>
		/// <returns></returns>
		private bool CheckType(string[] filetype)
		{
			return filetype.ToList().IndexOf(GetFileExtension) == -1;
		}

		/// <summary>
		/// File size detection
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		private bool CheckSize(int size)
		{
			return uploadFile.ContentLength >= (size * 1024 * 1024);
		}

		/// <summary>
		/// Get the file extension
		/// </summary>
		/// <returns></returns>
		private string GetFileExtension
		{
			get
			{
				return "." + uploadFile.FileName.Split('.').Last().ToLower();
			}
		}

		/// <summary>
		/// Automatically create storage folders by date
		/// </summary>
		private void CreateFolder()
		{
			if (!Directory.Exists(uploadpath))
				Directory.CreateDirectory(uploadpath);
		}

		/// <summary>
		/// Delete storage folder
		/// </summary>
		/// <param name="path"></param>
		public void DeleteFolder(string path)
		{
			if (Directory.Exists(path))
				Directory.Delete(path, true);
		}
	}
}