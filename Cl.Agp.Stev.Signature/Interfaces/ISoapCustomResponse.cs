using System.Xml.Linq;

namespace Cl.Agp.Stev.Signature.Interfaces
{
    interface ISoapCustomResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Deserialize(XDocument response);
    }
}
