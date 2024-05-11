using System.Linq.Expressions;

namespace Specification.Pattern.WebApi.Specifications.Common;

public abstract class Specification<TEntity>
{
    public static Specification<TEntity> Default => new DefaultSpecification<TEntity>();
    
    public abstract Expression<Func<TEntity, bool>> ToExpression();

    public bool IsSatisfiedBy(TEntity entity)
    {
        var func = this.ToExpression().Compile();

        return func(entity);
    }


    public Specification<TEntity> And(Specification<TEntity> specification)
    {
        if (specification == Default)
            return this;

        if (this == Default)
            return specification;

        return new AndSpecifcation<TEntity>(this, specification);
    }

    public Specification<TEntity> Or(Specification<TEntity> specification)
    {
        if (specification == Default ||this==Default)
            return Default;

        return new OrSpecification<TEntity>(this, specification);
    }


    #region Private Classes

    private class DefaultSpecification<TEntity> : Specification<TEntity>
    {
        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return entity => true;
        }
    }
    
    private class AndSpecifcation<TEntity>:Specification<TEntity>
    {
        private readonly Specification<TEntity> _right;
        private readonly Specification<TEntity> _left;


        public AndSpecifcation(Specification<TEntity> right, Specification<TEntity> left)
        {
            _right = right;
            _left = left;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            var rightExpression = _right.ToExpression();
            var leftExpression = _left.ToExpression();

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

            var lambdaExpression = Expression.Lambda(Expression.AndAlso(leftExpression.Body, invokedExpression),
                leftExpression.Parameters);

            return (Expression<Func<TEntity, bool>>)lambdaExpression;
        }
    }
    
    
    private class OrSpecification<TEntity> : Specification<TEntity>
    {
        private readonly Specification<TEntity> _right;
        private readonly Specification<TEntity> _left;

        public OrSpecification(Specification<TEntity> right, Specification<TEntity> left)
        {
            _right = right;
            _left = left;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            var rightExpression = _right.ToExpression();
            var leftExpression = _left.ToExpression();

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);
            
            var lambdaExpression = Expression.Lambda(Expression.OrElse(leftExpression.Body, invokedExpression),
                leftExpression.Parameters);

            return (Expression<Func<TEntity, bool>>)lambdaExpression;
        }
    }

    #endregion
}

