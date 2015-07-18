//-----------------------------------------------------------------------
// <copyright file="ImageConverter.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using StructureMap;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class ImageConverter : IImageConverter
    {
        public string GetThumbPath(string path, int width, int height)
        {
            var physicalPath = StrixPlatform.Environment.MapPath(path);

            if (!IsImage(physicalPath))
            {
                return null;
            }

            var thumbDirectory = this.GetThumbDirectory();
            var thumbPath = this.GetThumbPath(physicalPath, thumbDirectory, width, height);

            if (!System.IO.File.Exists(thumbPath))
            {
                CreateThumb(physicalPath, thumbPath, width, height);
            }

            return thumbPath;
        }

        public string ImageAsPath(string path, int width, int height, bool keepAspectRatio = true)
        {
            if (!IsImage(path))
            {
                return null;
            }

            var fullPath = StrixPlatform.Environment.MapPath(path);

            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }

            var thumbDirectory = this.GetThumbDirectory();
            var thumbPath = this.GetThumbPath(fullPath, thumbDirectory, width, height);
            bool thumbExists = System.IO.File.Exists(thumbPath);

            if (!thumbExists)
            {
                CreateThumb(fullPath, thumbPath, width, height, keepAspectRatio);
            }

            var virtualPath = Web.Helpers.GetVirtualPath(thumbPath);

            if (!virtualPath.StartsWith("/"))
            {
                virtualPath = "/" + virtualPath;
            }

            return virtualPath;
        }

        public string ImageAsBase64(string path, int width, int height, bool keepAspectRatio = true)
        {
            if (!IsImage(path))
            {
                return null;
            }

            var fullPath = StrixPlatform.Environment.MapPath(path);

            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }

            var thumbDirectory = this.GetThumbDirectory();

            byte[] image = this.GetImage(fullPath, thumbDirectory, width, height, keepAspectRatio);

            if (image == null || image.Count() == 0)
            {
                Logger.Log(string.Format("The image {0} could not be found.", fullPath));
                return null;
            }

            return string.Format("data:image/{0};base64,{1}", Path.GetExtension(path), Convert.ToBase64String(image));
        }

        public byte[] Resize(string path, int width, int height, bool overwrite = false, bool keepAspectRatio = true)
        {
            if (path == null)
            {
                return null;
            }

            var directory = this.GetSecureDirectory(StrixPlatform.Environment.MapPath(Path.GetDirectoryName(path)));
            string fullPath = Path.Combine(directory, Path.GetFileName(path));
            return Resize(System.IO.File.ReadAllBytes(fullPath), new Size(width, height), fullPath, overwrite, keepAspectRatio);
        }

        private static bool IsImage(string extension)
        {
            extension = Path.GetExtension(extension).Replace(".", string.Empty);
            var extensions = StrixCms.Config.Files.ImageExtensions;
            return extensions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Trim().ToLower().Any(e => e.ToLower() == extension.ToLower());
        }

        private static byte[] Resize(byte[] data, Size newSize, string fullPath = null, bool overwrite = false, bool keepAspectRatio = true)
        {
            if (data.IsEmpty())
            {
                return null;
            }

            string extension = fullPath.Split('.').Last();
            ImageFormat format = GetFormat(extension);
            Stream stream = new MemoryStream(data);
            Image image = Image.FromStream(stream);
            stream.Dispose();

            Size resizeSize = new Size();

            if (keepAspectRatio)
            {
                // Resize the image if a maximum size was specified.
                if (image.Height > newSize.Height || image.Width > newSize.Width)
                {
                    int maxNewSize = newSize.Height >= newSize.Width ? newSize.Width : newSize.Height;
                    float ratio = (float)image.Width / (float)image.Height;

                    if (image.Width > image.Height)
                    {
                        resizeSize.Height = (int)(maxNewSize / ratio);
                        resizeSize.Width = maxNewSize;
                    }
                    else
                    {
                        resizeSize.Height = maxNewSize;
                        resizeSize.Width = (int)(maxNewSize * ratio);
                    }
                }
            }
            else
            {
                resizeSize = newSize;
            }

            Bitmap bitmap;

            if (resizeSize.Width > 0)
            {
                bitmap = new Bitmap(image, resizeSize);
            }
            else
            {
                bitmap = new Bitmap(image);
            }

            if (!string.IsNullOrWhiteSpace(fullPath))
            {
                if (overwrite)
                {
                    bitmap.Save(fullPath, format);
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(fullPath).Split('_').First();
                    string filePath = fullPath.Substring(0, fullPath.IndexOf(fileName) - 1);
                    fullPath = string.Format("{0}\\{1}_{2}_{3}.{4}", filePath, fileName, newSize.Width, newSize.Height, extension);
                    bitmap.Save(fullPath, format);
                }

                data = System.IO.File.ReadAllBytes(fullPath);
            }
            else
            {
                stream = new MemoryStream(data);
                bitmap.Save(stream, format);
                stream.Read(data, 0, (int)stream.Length);
                stream.Dispose();
            }

            bitmap.Dispose();
            return data;
        }

        private static ImageFormat GetFormat(string extension)
        {
            ImageFormat format;
            switch (extension.ToLower())
            {
                case "bmp":
                    {
                        format = ImageFormat.Bmp;
                    }

                    break;
                case "emf":
                    {
                        format = ImageFormat.Emf;
                    }

                    break;
                case "exif":
                    {
                        format = ImageFormat.Exif;
                    }

                    break;
                case "gif":
                    {
                        format = ImageFormat.Gif;
                    }

                    break;
                case "ico":
                case "icon":
                    {
                        format = ImageFormat.Icon;
                    }

                    break;
                case "jpg":
                case "jpeg":
                    {
                        format = ImageFormat.Jpeg;
                    }

                    break;

                case "png":
                    {
                        format = ImageFormat.Png;
                    }

                    break;
                case "tif":
                case "tiff":
                    {
                        format = ImageFormat.Tiff;
                    }

                    break;
                case "wmf":
                    {
                        format = ImageFormat.Wmf;
                    }

                    break;
                default:
                    {
                        format = ImageFormat.Jpeg;
                    }

                    break;
            }

            return format;
        }

        private static byte[] CreateThumb(string fullPath, string thumbPath, int width, int height, bool keepAspectRatio = true)
        {
            byte[] image = null;
            byte[] data = System.IO.File.ReadAllBytes(fullPath);

            try
            {
                image = Resize(data, new Size(width, height), thumbPath, false, keepAspectRatio);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("An error occurred while trying to resize image {0}.", fullPath), ex, LogLevel.Error);
                throw;
            }

            return image;
        }

        private byte[] GetImage(string fullPath, string thumbDirectory, int width, int height, bool keepAspectRatio = true)
        {
            var thumbPath = this.GetThumbPath(fullPath, thumbDirectory, width, height);
            bool thumbExists = System.IO.File.Exists(thumbPath);
            byte[] image = null;

            if (thumbExists)
            {
                image = System.IO.File.ReadAllBytes(thumbPath);
            }
            else
            {
                image = CreateThumb(fullPath, thumbPath, width, height, keepAspectRatio);
            }

            return image;
        }

        private string GetThumbPath(string fullPath, string thumbDirectory, int width, int height)
        {
            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }

            var thumbPhysicalPath = StrixPlatform.Environment.MapPath(thumbDirectory);

            if (!Directory.Exists(thumbPhysicalPath))
            {
                Directory.CreateDirectory(thumbPhysicalPath);
            }

            string pathWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            return string.Format("{0}\\{1}_{2}_{3}.{4}", thumbPhysicalPath, pathWithoutExtension, width, height, extension).Replace("..", ".");
        }

        private string GetThumbDirectory()
        {
            string thumbDirectory;

            if (HttpContext.Current != null)
            {
                thumbDirectory = StrixCms.Config.Files.ThumbDirectory;
            }
            else
            {
                thumbDirectory = Path.Combine(StrixPlatform.Environment.WorkingDirectory, StrixCms.Config.Files.ThumbDirectory).Replace("/", "\\");
            }

            return this.GetSecureDirectory(thumbDirectory);
        }

        private string GetSecureDirectory(string directory)
        {
            if (StrixCms.Config.Files.SecureFiles && !directory.ToLower().Contains(string.Format("\\{0}", CmsConstants.SECURE).ToLower()))
            {
                directory = directory + string.Format("\\{0}", CmsConstants.SECURE);
            }

            if (!StrixPlatform.User.IsInMainGroup)
            {
                directory = directory + "\\" + StrixPlatform.User.GroupName;
            }

            return directory;
        }
    }
}