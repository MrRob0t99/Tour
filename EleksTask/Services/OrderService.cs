using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EleksTask;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(ApplicationContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> AddOrder(CreateOrderRequstDto dto, string userId)
        {
            var response = new Response<int>();
            var tour = await _unitOfWork.TourRepository.Find(t => t.Id == dto.TourId, to => to.FileModels);
            var order = _mapper.Map<Order>(dto);
            order.Tour = tour;
            order.ApplicationUserId = userId;
            order.IsConfirmed = false;
            order.Path = tour.FileModels.FirstOrDefault()?.Path;
            await _unitOfWork.OrderRepository.Create(order);
            await _unitOfWork.Commit();
            response.Data = order.Id;
            return response;
        }

        public async Task<Response<List<GetOrdersResponseDto>>> GetOrder(string userId)
        {
            var response = new Response<List<GetOrdersResponseDto>>();
            var orders = await _unitOfWork.OrderRepository.Get(o => o.ApplicationUserId == userId);
            var dto = _mapper.Map<List<GetOrdersResponseDto>>(orders);
            response.Data = dto;
            return response;
        }

        public async Task<Response<bool>> DeleteOrder(int id)
        {
            var response = new Response<bool>();
            var order = await _unitOfWork.OrderRepository.Find(o => o.Id==id);
            if (order == null)
            {
                response.Error = new Error("Order not found");
                return response;
            }
            _unitOfWork.OrderRepository.Remove(order);
            await _unitOfWork.Commit();
            response.Data = true;
            return response;
        }

        public async Task<Response<List<GetOrdersResponseDto>>> GetAllOrders()
        {
            var response = new Response<List<GetOrdersResponseDto>>();
            var orders = await _unitOfWork.OrderRepository.Get();
            response.Data = _mapper.Map<List<GetOrdersResponseDto>>(orders);
            return response;
        }
    }
}
