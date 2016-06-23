using System;
using System.Runtime.Serialization;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Apis.Models
{
    [DataContract]
    public class ApiResponse<T> : BaseResponse
    {
        public ApiResponse()
        {

        }
        public ApiResponse(T data)
        {
            Data = data;
        }

        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public bool IsBusinessValidation { get; set; }

        private string _responseTime = DateTime.Now.ToString("ddd MMM dd HH:mm:ss IST yyyy");
        [DataMember]
        public string ResponseTime
        {
            get
            {
                return _responseTime;
            }
            set
            {
                _responseTime = value;
            }
        }

        [DataMember]
        public string Status { get; set; }
    }
}