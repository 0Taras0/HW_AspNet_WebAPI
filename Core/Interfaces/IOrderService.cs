﻿using Core.Model.Delivery;
using Core.Model.General;
using Core.Model.Order;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetOrdersAsync();
        Task<List<SimpleModel>> GetPymentTypesAsync();
        Task<List<SimpleModel>> GetCitiesAsync(string city);
        Task<List<SimpleModel>> GetPostDepartmentsAsync(PostDepartmentSearchModel model);
        Task CreateOrderAsync(DeliveryInfoCreateModel model);
    }
}
