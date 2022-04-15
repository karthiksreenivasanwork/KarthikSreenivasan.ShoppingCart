using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ShoppingCart.API.Models
{
    public class AboutModelLibrary
    {
        public static string XMLDocumentationFilePath
        {
            get
            {
                var dirPath = Assembly.GetExecutingAssembly().Location;
                dirPath = Path.GetDirectoryName(dirPath);
                return string.Concat(dirPath, @"\ShoppingCart.API.Models.xml");

            }
        }
    }
}
