namespace Laborie.Service.Application.Services;
public interface ICacheService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns> <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<string?> GetString(string key);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expire">time expired (minute)</param>
    /// <returns></returns>
    Task SetString(string key, string value, int expire);
}
