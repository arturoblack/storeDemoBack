using System;
namespace otroProyecto5.Utils
{
	public class CopyTools
	{
        static readonly List<string> _mimeTypes = new List<string> {
                "image/jpg", "image/jpeg", "image/png" };
        static readonly long _maxImageSize = 2048000;

        public static async Task<string> CopyImageAsync(
            IFormFile image,
            string folder,
            string rootFolder,
            string imageName = null,
            long? maxImageSize = null,
            List<string> aceptedMimeTypes = null
            )
        {
            if (aceptedMimeTypes == null)
                aceptedMimeTypes = _mimeTypes;
            if (imageName == null)
            {
                imageName = Guid.NewGuid().ToString();
            }

            string mimetype = image.ContentType;
            if (aceptedMimeTypes.Where(m => m == mimetype).Count() <= 0)
            {
                throw new Exception("Tipo de archivo invalido");
            }
            long size = image.Length;
            long maxSize = maxImageSize ?? _maxImageSize;
            if (size > maxSize)
            {
                decimal max = (decimal)maxSize / 1024 / 1024;
                int maxSizeInMB = (int)Math.Floor(max);
                throw new Exception($"No archivos mayores a {maxSizeInMB} MB");
            }
            else if (size > 0)
            {
                string ext = image.FileName.Split('.').Last();
                string nameImage = $"{imageName}.{ext}";
                string file = Path.Combine(rootFolder, "uploads", folder, nameImage);
                using (var stream = System.IO.File.Create(file))
                {
                    await image.CopyToAsync(stream);
                }
                return $"/uploads/{folder}/{nameImage}";
            }
            throw new Exception("Algun error");

        }

    }
}

