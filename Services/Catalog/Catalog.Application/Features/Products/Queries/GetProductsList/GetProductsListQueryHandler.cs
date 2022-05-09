using AutoMapper;
using Catalog.Application.Contracts.Persistence;
using Catalog.Domain.Entities;
using Catalog.Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Products.Queries.GetProductsList
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, List<ProductResponse>>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;
        public GetProductsListQueryHandler(
            IProductsRepository productsRepository,
            IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductResponse>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Product, object>>>
            {
                p => p.ProductBrand,
                p => p.ProductType
            };

            var list = await _productsRepository.GetAsync(
                predicate: null,
                orderBy: null,
                includes: includes,
                disableTracking: true);
            return _mapper.Map<List<ProductResponse>>(list);
        }
    }
}
