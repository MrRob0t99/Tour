using System.Collections.Generic;
using System.Threading.Tasks;
using TourServer.Dto;

namespace TourServer.ServicesInterface
{
    public interface IOrderService
    {
        Task<Response<int>> AddOrder(CreateOrderRequstDto dto, string userId);

        Task<Response<List<GetOrdersResponseDto>>> GetOrder(string userId);

        Task<Response<bool>> DeleteOrder(int id);

        Task<Response<List<GetOrdersResponseDto>>> GetAllOrders();
    }
}
