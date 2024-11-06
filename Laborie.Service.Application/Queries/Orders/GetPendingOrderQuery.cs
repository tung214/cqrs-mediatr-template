using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Queries.Orders;

/// <summary>
/// Lấy đơn hàng chưa hoàn tất trong vòng 30 ngày
/// </summary>
/// <param name="UserId"></param>
/// <param name="PageSize"></param>
/// <param name="PageIndex"></param>
/// <returns></returns>
public sealed record GetPendingOrderQuery(string UserId, int PageSize, int PageIndex) : IQuery<Response>;
/// <summary>
/// Lấy chi tiết đơn hàng
/// </summary>
/// <param name="UserId"></param>
/// <param name="OrderId"></param>
/// <returns></returns>
public sealed record GetOrderQuery(string UserId, string OrderId) : IQuery<Response>;
/// <summary>
/// Lấy lịch sử đặt hàng
/// </summary>
/// <param name="UserId"></param>
/// <param name="PageSize"></param>
/// <param name="PageIndex"></param>
/// <returns></returns>
public sealed record GetOrderHistoryQuery(string UserId, int PageSize, int PageIndex) : IQuery<Response>;