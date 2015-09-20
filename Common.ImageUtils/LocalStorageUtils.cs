using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Transplicity.Common.ImageUtils
{
    public static class LocalStorageUtils
    {

        public static string UploadImage(string imagesFolder, Image image, string imageName, ImageFormat format)
        {
            //return null;
            //TODO: Implement
            var path = Path.Combine(ConfigurationManager.AppSettings["RootFiles"], imagesFolder);
            var exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);

            image.Save(Path.Combine(path, imageName), format);

            return $"{ConfigurationManager.AppSettings["httpRootFiles"]}/{imagesFolder}/{imageName}";
        }

        public static string UploadMedia(FileInfo mediaName)
        {
            var mediaFolder = ConfigurationManager.AppSettings["MediaFolder"];
            var rootFolder = ConfigurationManager.AppSettings["RootFiles"];
            var path = Path.Combine(rootFolder, mediaFolder);
            var exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);
            mediaName.CopyTo(Path.Combine(path, mediaName.Name));

            //    image.Save(Path.Combine(path, imageName), format);

            return $"{ConfigurationManager.AppSettings["httpRootFiles"]}/{mediaFolder}/{mediaName.Name}";
        }
    }
}
