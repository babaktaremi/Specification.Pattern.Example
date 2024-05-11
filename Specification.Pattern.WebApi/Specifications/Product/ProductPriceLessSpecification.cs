using System.Linq.Expressions;
using Specification.Pattern.WebApi.Specifications.Common;

namespace Specification.Pattern.WebApi.Specifications.Product;

public class ProductPriceLessSpecification:Specification<Entities.Product>
{
    private readonly decimal _minPrice;

    public ProductPriceLessSpecification(decimal minPrice)
    {
        _minPrice = minPrice;
    }

    public override Expression<Func<Entities.Product, bool>> ToExpression()
    {
        return product => product.ListPrice >= _minPrice;
    }
}