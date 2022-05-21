using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ShoppingCart.API.Models
{
    /// <summary>
    /// Provdes informtion about this class library - ShoppingCart.API.Models
    /// </summary>
    public class AboutModelLibrary
    {
        /// <summary>
        /// Returns the XML document that contains a detailed XML comment for each model and it's properties.
        /// </summary>
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
