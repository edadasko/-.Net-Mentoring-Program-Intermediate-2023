using Expressions.Task3.E3SQueryProvider.Models.Request;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly FtsQueryRequest _resultRequest;
        readonly StringBuilder _nextStatementStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultRequest = new FtsQueryRequest();
            _nextStatementStringBuilder = new StringBuilder();
        }

        public FtsQueryRequest Translate(Expression exp)
        {
            Visit(exp);

            return _resultRequest;
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                Visit(node.Arguments[0]);
                Visit(node.Arguments[1]);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                Visit(node.Object);
                _nextStatementStringBuilder.Append(GetOpenSymbols(node.Method.Name));
                Visit(node.Arguments[0]);
                _nextStatementStringBuilder.Append(GetCloseSymbols(node.Method.Name));
                AddNextStatement();

                return node;

                string GetOpenSymbols(string methodName)
                {
                    switch (methodName)
                    {
                        case "Equals":
                        case "StartsWith":
                            return "(";
                        case "EndsWith":
                        case "Contains":
                            return "(*";
                        default:
                            throw new NotSupportedException($"Not supported string method: {methodName}");
                    }
                }

                string GetCloseSymbols(string methodName)
                {
                    switch (methodName)
                    {
                        case "Equals":
                        case "EndsWith":
                            return ")";
                        case "StartsWith":
                        case "Contains":
                            return "*)";
                        default:
                            throw new NotSupportedException($"Not supported string method: {methodName}");
                    }
                }
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:

                    bool directOrder =
                        node.Left.NodeType == ExpressionType.MemberAccess
                        && node.Right.NodeType == ExpressionType.Constant;

                    if (!directOrder)
                    {
                        bool reverseOrder =
                            node.Left.NodeType == ExpressionType.Constant
                            && node.Right.NodeType == ExpressionType.MemberAccess;

                        if (!reverseOrder)
                        {
                            throw new NotSupportedException($"Incorect operands: {node.NodeType}");
                        }
                    }

                    Visit(directOrder ? node.Left : node.Right);
                    _nextStatementStringBuilder.Append("(");
                    Visit(directOrder ? node.Right : node.Left);
                    _nextStatementStringBuilder.Append(")");
                    AddNextStatement();
                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _nextStatementStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _nextStatementStringBuilder.Append(node.Value);

            return node;
        }

        #endregion

        private void AddNextStatement()
        {
            _resultRequest.Statements.Add(new Statement() { Query = _nextStatementStringBuilder.ToString() });
            _nextStatementStringBuilder.Clear();
        }
    }
}
