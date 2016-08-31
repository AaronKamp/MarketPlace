using System;
using System.Runtime.Serialization;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Apis.Models
{
    /// <summary>
    /// API response model.
    /// </summary>
    /// <typeparam name="T"> Generic class </typeparam>
    [DataContract]
    public class ApiResponse<T> : BaseResponse
    {
        /// <summary>
        /// Default constructor for ApiResponse
        /// </summary>
        public ApiResponse()
        {

        }

        /// <summary>
        /// Constructor to initialize data
        /// </summary>
        /// <param name="data"></param>
        public ApiResponse(T data)
        {
            Data = data;
        }
        ///<summary>
        /// Data in the API response
        ///</summary>
        [DataMember]
        public T Data { get; set; }
        /// <summary>
        /// True if business validation error occurred.
        /// </summary>
        [DataMember]
        public bool IsBusinessValidation { get; set; }

        private string _responseTime = DateTime.UtcNow.ToString();

        /// <summary>
        /// Response time.
        /// </summary>
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
        /// <summary>
        /// Success or failure.
        /// </summary>
        [DataMember]
        public string Status { get; set; }
    }
}