using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Dtos.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    { 
       readonly IOrderService _orderService;
        readonly IMailService _mailService;
        public CompleteOrderCommandHandler(IOrderService orderService,IMailService mailService) 
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            
          (bool succeded, CompletedOrderDto dto) = await _orderService.CompleteOrderAsync(request.Id);
            if (succeded)
               await _mailService.SenCompletedOrderMailAsync(dto.EMail,dto.OrderCode,dto.OrderDate,dto.Username,dto.UserSurname);

            return new();
        }
    }
}
