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
        public static bool uploadProductImage(IFormFile productImageFile, string renamedFileName)
        {
            try
            {
                if (productImageFile != null && productImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(renamedFileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        productImageFile.CopyTo(fileStream);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information.
                System.Diagnostics.Debug.WriteLine(String.Format("Error occured while uploading the image {0}", productImageFile.FileName));
                throw new Exception("Something went wrong. Unable to add a new product.");
            }

            return false;
        }
    }
}
