using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EleksTask;
using EleksTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public OrderService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<int>> AddOrder(CreateOrderRequstDto dto, string userId)
        {
            var response = new Response<int>();
            var tour = await _context.Tours.Where(t => t.Id == dto.TourId).Include(t => t.FileModels).FirstOrDefaultAsync();
            var order = _mapper.Map<Order>(dto);
            order.Tour = tour;
            order.ApplicationUserId = userId;
            order.IsConfirmed = false;
            order.Path = tour.FileModels.FirstOrDefault().Path;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            response.Data = order.Id;
            return response;
        }

        public async Task<Response<List<GetOrdersResponseDto>>> GetOrder(string userId)
        {
            var response = new Response<List<GetOrdersResponseDto>>();
            var orders = await _context.Orders.Where(o => o.ApplicationUserId == userId).ToListAsync();
            var dto = _mapper.Map<List<GetOrdersResponseDto>>(orders);
            response.Data = dto;
            return response;
        }

        public async Task<Response<bool>> DeleteOrder(int id)
        {
            var response = new Response<bool>();
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                response.Error = new Error("Order not found");
                return response;
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            response.Data = true;
            return response;
        }

        public async Task<Response<bool>> ConfirmOrder(int id)
        {
            var response = new Response<bool>();
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                response.Error = new Error("Order not found");
                return response;
            }

            order.IsConfirmed = true;
            await _context.SaveChangesAsync();
            response.Data = true;
            return response;
        }

        public async Task<Response<List<GetOrdersResponseDto>>> GetAllOrders()
        {
            var response = new Response<List<GetOrdersResponseDto>>();
            var orders =await _context.Orders.ToListAsync();
            response.Data = _mapper.Map<List<GetOrdersResponseDto>>(orders);
            return response;
        }
    }
}
