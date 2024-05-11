using System.Linq.Expressions;
using Specification.Pattern.WebApi.Specifications.Common;

namespace Specification.Pattern.WebApi.Specifications.Product;

public class ProductPriceWeightSpecification:Specification<Entities.Product>
{
    private readonly decimal? _weight;

    public ProductPriceWeightSpecification(decimal? weight)
    {
        _weight = weight;
    }

    public override Expression<Func<Entities.Product, bool>> ToExpression()
    {
        return product => product.Weight > _weight;
    }
}