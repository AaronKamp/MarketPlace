using System;

namespace TCCMarketPlace.Model
{
    /// <summary>
    /// Service provider details business model.
    /// </summary>
    public class ServiceProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SignUpUrl { get; set; }
        public string StatusUrl { get; set; }
        public string UnEnrollUrl { get; set; }
        public bool GenerateBearerToken { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string TokenUrl { get; set; }
        public string AppId { get; set; }
        public string SecretKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }

    }
}
