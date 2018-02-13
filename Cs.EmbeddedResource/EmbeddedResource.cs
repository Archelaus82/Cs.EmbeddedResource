using System;
using System.IO;

namespace Cs.EmbeddedResource
{
    public class EmbeddedResourceException : Exception
    {
        public EmbeddedResourceException()
        {
        }

        public EmbeddedResourceException(string message)
            : base(message)
        {
        }

        public EmbeddedResourceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class EmbeddedResource
    {
        /// <summary>
        /// Method reads in an embeded text resource file for use.
        /// File must be part of the .csproj
        /// Properties.Build Action = Embedded Resource
        /// Ex. GetResourceFile(this, QC.AddOns.EVBcom.Codec.XML, "Definitions_WCD9335_1_Internal.xml")
        /// </summary>
        /// <param name="defaultNamespace">
        /// string - the namespace a newly created class would have in the same folder as the file
        /// </param>
        /// <param name="filename">strng - filename</param>
        /// <returns>stream - file as stream</returns>
        public static Stream GetFileAsStream(object invoker, string defaultNamespace, string filename)
        {
            try
            {
                string result = string.Empty;
                string resource = defaultNamespace + "." + filename;
                Stream stream = invoker.GetType().Assembly.GetManifestResourceStream(resource);

                return stream;
            }
            catch (Exception ex)
            {
                throw new EmbeddedResourceException("Failed to get embedded resource", ex);
            }
        }

        /// <summary>
        /// Method reads in an embeded text resource file for use.
        /// File must be part of the .csproj
        /// Properties.Build Action = Embedded Resource
        /// Ex. GetResourceFile(this, QC.AddOns.EVBcom.Codec.XML, "Definitions_WCD9335_1_Internal.xml")
        /// </summary>
        /// <param name="defaultNamespace">
        /// string - the namespace a newly created class would have in the same folder as the file
        /// </param>
        /// <param name="filename">strng - filename</param>
        /// <returns>string - file as string</returns>
        public static string GetFileAsString(object invoker, string defaultNamespace, string filename)
        {
            string resource = defaultNamespace + "." + filename;
            try
            {
                string result = string.Empty;
                using (Stream stream = invoker.GetType().Assembly.GetManifestResourceStream(resource))
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();

                    return result;
                }
            }
            catch (ArgumentNullException ex)
            {
                if (ex.Message.Contains("stream"))
                    throw new EmbeddedResourceException(String.Format("Verify resource file [{0}] BuildAction is set to Embedded Resource", resource));
                else
                    throw;
            }
            catch (Exception ex)
            {
                throw new EmbeddedResourceException("Failed to get embedded resource", ex);
            }
        }
    }
}
