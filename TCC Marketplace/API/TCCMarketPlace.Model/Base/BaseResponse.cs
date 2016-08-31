using System;
using System.Runtime.Serialization;

namespace TCCMarketPlace.Model
{
    /// <summary>
    /// API response base model.
    /// </summary>
    [DataContract]
    [Serializable]
    public abstract class BaseResponse
    {
        //True if response has error
        [DataMember]
        public bool HasError { get; set; }
        //Error message
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
