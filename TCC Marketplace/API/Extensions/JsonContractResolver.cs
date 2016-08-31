using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Handles the contract creation for object type IDictionary
    /// </summary>
    public class JsonContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// If objectType is of type IDictionary, JsonArrayContract is returned, else returns JsonContract.
        /// </summary>
        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType.GetInterfaces().Any(i => i == typeof(IDictionary) ||
                (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>))))
            {
                return base.CreateArrayContract(objectType);
            }
            return base.CreateContract(objectType);
        }
    }
}
