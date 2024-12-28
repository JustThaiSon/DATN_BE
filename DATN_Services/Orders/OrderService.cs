using AutoMapper;
using DATN_Helpers.Constants;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Order.Req;
using DATN_Services.Orders.Interface;

namespace DATN_Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDAO _orderDao;
        private readonly IMapper _mapper;
        public OrderService(IOrderDAO orderDao, IMapper mapper)
        {
            _orderDao = orderDao;
            _mapper = mapper;
        }

        public void AddTicket(Guid UserID, CreateTicketDAL req, out int responseCode)
        {
            var order = new CreateOrderDAL
            {
                IsAnonymous = req.IsAnonymous,
                PaymentId = req.PaymentId,
                Status = req.Status,
                TotalPrice = req.TotalPrice,
                QuantityTicket = req.QuantityTicket,
                TotalPriceTicket = req.TotalPriceTicket
            };
            _orderDao.CreateOrder(UserID, order, out Guid orderDetailId, out Guid orderId, out int response);
            if (response != (int)ResponseCodeEnum.SUCCESS)
            {
                responseCode = response;
                return;
            }
            foreach (var ticket in req.Tickets)
            {
                _orderDao.CreateTicket(orderDetailId, ticket, out int res);
                if (res != (int)ResponseCodeEnum.SUCCESS)
                {
                    responseCode = res;
                    return;
                }
            }
            foreach (var service in req.Services)
            {
                _orderDao.CreateOrderService(orderId, service, out int res);
                if (res != (int)ResponseCodeEnum.SUCCESS)
                {
                    responseCode = res;
                    return;
                }
            }
            responseCode = (int)ResponseCodeEnum.SUCCESS;
        }
    }
}
