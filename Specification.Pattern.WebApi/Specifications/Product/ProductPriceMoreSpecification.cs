using System.Linq.Expressions;
using Specification.Pattern.WebApi.Specifications.Common;

namespace Specification.Pattern.WebApi.Specifications.Product;

public class ProductPriceMoreSpecification:Specification<Entities.Product>
{
    private readonly decimal _maxPrice;

    public ProductPriceMoreSpecification(decimal minPrice)
    {
        _maxPrice = minPrice;
    }

    public override Expression<Func<Entities.Product, bool>> ToExpression()
    {
        return product => product.ListPrice <= _maxPrice;
    }
}