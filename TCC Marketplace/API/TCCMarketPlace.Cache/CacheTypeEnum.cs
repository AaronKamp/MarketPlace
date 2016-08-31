namespace TCCMarketPlace.Cache
{
    /// <summary>
    /// To chose between Redis cache and InMemory cache.
    /// </summary>
    internal enum CacheTypeEnum
    {
        InProcess,
        Distributed
    }
}
