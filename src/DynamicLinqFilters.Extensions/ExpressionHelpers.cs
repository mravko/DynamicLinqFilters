using DynamicLinqFilters.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicLinqFilters.Extensions
{
    public static class ExpressionHelpers
    {
        public static Expression BuildNestedPropertyExpression(ParameterExpression pe, string propertyName)
        {
            Expression body = pe;
            foreach (var member in propertyName.Split('.'))
            {
                body = FixReflectedType(Expression.PropertyOrField(body, member));
            }
            return body;
        }

        public static Expression ConcatExpressionsWithOperator(Expression left, Expression right, string expressionOperator)
        {
            switch (expressionOperator.ToLower())
            {
                case "=":
                    return Expression.Equal(left, right);
                case "!=":
                    return Expression.NotEqual(left, right);
                case "<":
                    return Expression.LessThan(left, right);
                case ">":
                    return Expression.GreaterThan(left, right);
                case "<=":
                    return Expression.LessThanOrEqual(left, right);
                case ">=":
                    return Expression.GreaterThanOrEqual(left, right);
                case "in":
                    {
                        MethodInfo method = right.Type.GetMethod("Contains", new[] { typeof(int) });
                        return Expression.Call(right, method, left);
                    }
                case "contains":
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        return Expression.Call(left, method, right);
                    }
                case "startswith":
                    {
                        MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                        return Expression.Call(left, method, right);
                    }
                case "endswith":
                    {
                        MethodInfo method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                        return Expression.Call(left, method, right);
                    }
                default:
                    return Expression.Equal(left, right);
            }
        }

        public static Expression ConcatExpressionsWithOperator(Expression left, Expression right, FilterJoinType filterJoinType)
        {
            switch (filterJoinType)
            {   
                case FilterJoinType.And:
                    return Expression.And(left, right);
                case FilterJoinType.Or:
                    return Expression.OrElse(left, right);
                default:
                    return Expression.And(left, right);
            }
        }

        private static MemberExpression FixReflectedType(MemberExpression expr)
        {
            var member = expr.Member;
            var declaringType = member.DeclaringType;

            if (member.ReflectedType != declaringType)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Property:
                        return Expression.Property(expr.Expression, declaringType, member.Name);
                    case MemberTypes.Field:
                        return Expression.Field(expr.Expression, declaringType, member.Name);
                }
            }

            return expr;
        }
    }
}
