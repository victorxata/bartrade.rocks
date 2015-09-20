using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Transplicity.Domain.Data.Models.Enum;

namespace Transplicity.Common.ImageUtils
{
    public static class ImageHelpers
    {
        public static Image Resize(Image img, int srcX, int srcY, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            var bmp = new Bitmap(dstWidth, dstHeight);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    var destRect = new Rectangle(0, 0, dstWidth, dstHeight);
                    graphics.DrawImage(img, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return bmp;
        }

        public static Image ResizeProportional(Image img, int width, int height, bool enlarge = false)
        {
            var ratio = Math.Max(img.Width / (double)width, img.Height / (double)height);
            if (ratio < 1 && !enlarge) return img;
            var result = Resize(img, 0, 0, img.Width, img.Height, (int)Math.Round(img.Width / ratio), (int)Math.Round(img.Height / ratio));
            return result;
        }

        public static Image ResizeCropExcess(Image img, int dstWidth, int dstHeight)
        {
            var srcRatio = img.Width / (double)img.Height;
            var dstRatio = dstWidth / (double)dstHeight;
            int srcX, srcY, cropWidth, cropHeight;

            if (srcRatio < dstRatio) // trim top and bottom
            {
                cropHeight = dstHeight * img.Width / dstWidth;
                srcY = (img.Height - cropHeight) / 2;
                cropWidth = img.Width;
                srcX = 0;
            }
            else // trim left and right
            {
                cropWidth = dstWidth * img.Height / dstHeight;
                srcX = (img.Width - cropWidth) / 2;
                cropHeight = img.Height;
                srcY = 0;
            }

            return Resize(img, srcX, srcY, cropWidth, cropHeight, dstWidth, dstHeight);
        }

        public static ImageType GetImageType(Image image)
        {
            if (ImageFormat.Png.Equals(image.RawFormat))
            {
                return ImageType.Png;
            }
            if (ImageFormat.Jpeg.Equals(image.RawFormat))
            {
                return ImageType.Jpg;
            }
            return ImageFormat.Gif.Equals(image.RawFormat) ? ImageType.Gif : ImageType.Bmp;
        }

        public static ImageFormat GetImageFormat(ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.Png: return ImageFormat.Png;
                case ImageType.Bmp: return ImageFormat.Bmp;
                case ImageType.Gif: return ImageFormat.Gif;
                case ImageType.Jpg: return ImageFormat.Jpeg;
                default:
                    return ImageFormat.Png;
            }
        }
    }
}
