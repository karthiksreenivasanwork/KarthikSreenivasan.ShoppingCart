using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShoppingCart.API.BusinessLogic
{
    /// <summary>
    /// Summary:
    ///     It manages product image uploading and downloading services.
    /// </summary>
    public class ProductImageManager
    {
        /// <summary>
        /// Uploads a product image to a server location.
        /// </summary>
        /// <param name="productImageFile">A Microsoft.AspNetCore.Http.IFormFile</param>
        /// <returns>Return true if the product image was uploaded successfully and false otherwise</returns>
        public static bool uploadProductImage(IFormFile productImageFile)
        {
            if (productImageFile != null && productImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(productImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productImageFile.CopyTo(fileStream);
                }
                return true;
            }
            return false;
        }
    }
}
