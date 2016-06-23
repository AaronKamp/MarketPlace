using System;
using System.Runtime.Serialization;

namespace TCCMarketPlace.Model
{
    [DataContract]
    [Serializable]
    public abstract class BaseResponse
    {
        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
