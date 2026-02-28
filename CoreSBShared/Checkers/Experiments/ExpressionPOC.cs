using System;
using System.Linq.Expressions;

namespace InfrastructureCheckers
{

    /// <summary>
    /// Custom linq proof of concept. traversing expression treee with custom strings.
    /// </summary>
    #region POC
    //testing different expression types
    public class ExpressionsPOC
    {

        public static void GO()
        {
            var item = new ExpressionsPOC();
            item.BuildExpressionManualy();
        }
        ParameterExpression leftParamExpr;
        Type leftType_ = null;

        ConstantExpression constExpr;

        public ExpressionsPOC()
        {

        }
        public string VisitLeftRightFromExpressionTypes<T>(Expression<Func<T, bool>> expr_)
            where T : TestEntity
        {

            var b = expr_;
            var c = b.Body;
            var gtp = c.GetType();
            var nt = c.NodeType;
            var pm = expr_.Parameters;
            var nm = expr_.Name;
            var tp = expr_.Type;
            var typeName = typeof(T);

            //straight convertsion
            string straight = expr_.ToString();
            BinaryExpression binaryE = (BinaryExpression)expr_.Body;

            //conversion from nested class
            string straightNested = binaryE.ToString();

            if (binaryE.Left != null) { VisitConditional(binaryE.Left); }
            if (binaryE.Right != null) { VisitConditional(binaryE.Right); }

            Expression leftParameter = Expression.Parameter(leftType_, leftType_.Name);
            Type tp0 = leftParameter.GetType();
            Expression leftExpr = Expression.Property(leftParameter, leftParamExpr.Name);
            Type tp1 = leftExpr.GetType();
            ExpressionType nodeType = c.NodeType;
            Expression rightParameter = Expression.Constant(constExpr.Value, constExpr.Type);
            Type tp2 = rightParameter.GetType();
            Expression e0 = BuildExpressionType(nodeType, leftExpr, rightParameter);
            Type tp3 = e0.GetType();

            string ets = e0.ToString();

            string lb = this.VisitBinary((MemberExpression)binaryE.Left, "oper", binaryE);
            string lb2 = this.VisitBinary(binaryE, "converted", leftExpr, nodeType, rightParameter);
            string lb3 = this.BuildExpressionStr(nodeType, leftExpr, rightParameter);

            //variable not invoked
            //var a = Expression.Lambda(e0).Compile().DynamicInvoke();

            return ets;
        }
        public Expression VisitConditional(Expression expr)
        {
            Type type_ = expr.GetType().BaseType;
            if (type_ == typeof(MemberExpression))
            {
                MemberExpression memberExpr = (MemberExpression)expr;
                leftType_ = memberExpr.Expression.Type;
                /*
                MemberExpression mn=(MemberExpression)memberExpr.Expression;
                memberName=mn.Member.Name;
                */
                leftParamExpr = Expression.Parameter(memberExpr.Type, memberExpr.Member.Name);
            }
            if (type_ == typeof(ConstantExpression) || type_ == typeof(Expression))
            {
                constExpr = (ConstantExpression)expr;
                Expression rightExpr = Expression.Constant(constExpr.Value, constExpr.Type);
            }
            return expr;
        }
        public Expression BuildExpressionType(ExpressionType nodeType_, Expression lt, Expression rt)
        {
            Expression res = null;
            if (nodeType_ == ExpressionType.GreaterThan)
            {
                res = Expression.GreaterThan(lt, rt);
            }
            if (nodeType_ == ExpressionType.GreaterThanOrEqual)
            {
                res = Expression.GreaterThanOrEqual(lt, rt);
            }
            if (nodeType_ == ExpressionType.LessThan)
            {
                res = Expression.LessThan(lt, rt);
            }
            if (nodeType_ == ExpressionType.LessThanOrEqual)
            {
                res = Expression.LessThanOrEqual(lt, rt);
            }
            if (nodeType_ == ExpressionType.Assign)
            {
                res = Expression.Assign(lt, rt);
            }
            if (nodeType_ == ExpressionType.Equal)
            {
                res = Expression.Equal(lt, rt);
            }
            if (nodeType_ == ExpressionType.NotEqual)
            {
                res = Expression.NotEqual(lt, rt);
            }
            return res;
        }
        public string BuildExpressionStr(ExpressionType nodeType_, Expression lt, Expression rt)
        {
            string res = null;
            res = $"{lt}{nodeType_}{rt}";
            return res;
        }

        public string VisitBinary(MemberExpression binary, string @operator, BinaryExpression expression) =>
            $"{@operator}({binary},{expression})";
        public string VisitBinary(BinaryExpression expression, string op, Expression left, ExpressionType nt, Expression right) =>
            $"{expression}({op}),{left}|{nt}|{right}";
        public string VisitReturnString<T>(Expression<Func<T, bool>> expr_)
        {
            BinaryExpression be = (BinaryExpression)expr_.Body;
            string res = $"{be}";
            return res;
        }

        public string TraverseExpression<T>(Expression<Func<T, bool>> expr_)
        {
            //traversing expression
            Pv pv = new Pv();
            string traversed = pv.VisitBody(expr_);
            return traversed;
        }

        public string BuildExpressionManualy()
        {
            string res = string.Empty;
            ParameterExpression pe = Expression.Parameter(typeof(TestEntity), "te");
            ConstantExpression ce = Expression.Constant(new TestEntity() { Id = 0 }, typeof(TestEntity));
            BinaryExpression be = Expression.Equal(pe, ce);

            Expression<Func<TestEntity, bool>> lmb1 =
              Expression.Lambda<Func<TestEntity, bool>>(
              be, new ParameterExpression[] { pe }
            );

            res = lmb1.ToString();

            return res;
        }

        //build in function compile
        internal static void BuiltInCompile()
        {
            Expression<Func<double, double, double, double, double, double>> infix =
                (a, b, c, d, e) => a + b - c * d / 2 + e * 3;
            Func<double, double, double, double, double, double> function = infix.Compile();
            double result = function(1, 2, 3, 4, 5); // 12
        }
        public void ExpressionBuild()
        {

            BinaryExpression be = Expression.Power(Expression.Constant(2D), Expression.Constant(3D));
            Expression<Func<double>> fd = Expression.Lambda<Func<double>>(be);
            Func<double> ce = fd.Compile();
            double res = ce();
        }

    }

    //test POCO
    public class TestEntity
    {
        public string name { get; set; }
        static int id { get; set; } = 0;
        public ToggleProp tp { get; set; }
        public int Id
        {
            get
            {
                if (id == 0) { id += 1; }
                return id;
            }
            set { id = value; }
        }
        public bool intrinsicIsTrue { get; set; }
    }
    //toggle property for test POCO
    public class ToggleProp
    {
        public bool isTrue { get; set; }
    }

    //https://weblogs.asp.net/dixin/functional-csharp-function-as-data-and-expression-tree
    internal abstract class Ba<TResult>
    {

        internal virtual TResult VisitBody(LambdaExpression expression) => this.VisitNode(expression.Body, expression);

        protected TResult VisitNode(Expression node, LambdaExpression expression)
        {

            switch (node.NodeType)
            {

                case ExpressionType.Equal:
                    return this.VisitEqual((BinaryExpression)node, expression);

                case ExpressionType.GreaterThanOrEqual:
                    return this.VisitGreaterOrEqual((BinaryExpression)node, expression);

                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)node, expression);

                case ExpressionType.Constant:
                    return this.VisitConstatnt((ConstantExpression)node, expression);

                default:
                    throw new ArgumentOutOfRangeException(nameof(node));
            }

        }

        protected abstract TResult VisitEqual(BinaryExpression equal, LambdaExpression expression);
        protected abstract TResult VisitGreaterOrEqual(BinaryExpression equal, LambdaExpression expression);
        protected abstract TResult VisitMemberAccess(MemberExpression equal, LambdaExpression expression);
        protected abstract TResult VisitConstatnt(ConstantExpression equal, LambdaExpression expression);
    }
    internal class Pv : Ba<string>
    {
        protected override string VisitEqual
          (BinaryExpression add, LambdaExpression expression) => this.VisitBinary(add, "Equal", expression);

        protected override string VisitGreaterOrEqual
          (BinaryExpression add, LambdaExpression expression) => this.VisitBinary(add, "Greater", expression);

        protected override string VisitMemberAccess
          (MemberExpression add, LambdaExpression expression) => this.VisitBinary(add, "Member", expression);

        protected override string VisitConstatnt
          (ConstantExpression add, LambdaExpression expression) => this.VisitBinary(add, "Constant", expression);

        private string VisitBinary( // Recursion: operator(left, right)
          BinaryExpression binary, string @operator, LambdaExpression expression) =>
          $"{@operator}({this.VisitNode(binary.Left, expression)},{this.VisitNode(binary.Right, expression)})";

        private string VisitBinary( // Recursion: operator(left, right)
          MemberExpression binary, string @operator, LambdaExpression expression) =>
          $"{binary}";

        private string VisitBinary( // Recursion: operator(left, right)
          ConstantExpression binary, string @operator, LambdaExpression expression) =>
          $"{binary}";
    }
    #endregion
}
