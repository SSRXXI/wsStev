using System.Xml.Linq;

namespace Cl.Agp.Stev.Signature.Interfaces
{
    interface ISoapCustomRequest<T>
    {
        /// <summary>
        /// Genera un request a partir de una plantilla
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string</returns>
        XDocument GenerateQuery(T obj);
    }
}
