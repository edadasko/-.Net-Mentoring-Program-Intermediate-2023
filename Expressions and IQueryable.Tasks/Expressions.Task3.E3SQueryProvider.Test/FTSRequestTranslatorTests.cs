using System;
using System.Linq;
using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.Models.Entities;
using Expressions.Task3.E3SQueryProvider.Models.Request;
using Xunit;

namespace Expressions.Task3.E3SQueryProvider.Test
{
    public class FtsRequestTranslatorTests
    {
        #region SubTask 1 : operands order

        [Fact]
        public void TestBinaryBackOrder()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => "EPRUIZHW006" == employee.Workstation;

            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Single(translated.Statements);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated.Statements.Single().Query);
        }

        #endregion

        #region SubTask 2: inclusion operations

        [Fact]
        public void TestBinaryEqualsQueryable()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation == "EPRUIZHW006");

            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Single(translated.Statements);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated.Statements.Single().Query);
        }

        [Fact]
        public void TestBinaryEquals()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation == "EPRUIZHW006";

            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Single(translated.Statements);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated.Statements.Single().Query);
        }

        [Fact]
        public void TestMethodEquals()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.Equals("EPRUIZHW006");

            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Single(translated.Statements);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated.Statements.Single().Query);
        }

        [Fact]
        public void TestStartsWith()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.StartsWith("EPRUIZHW006");

            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Single(translated.Statements);
            Assert.Equal("Workstation:(EPRUIZHW006*)", translated.Statements.Single().Query);
        }

        [Fact]
        public void TestEndsWith()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.EndsWith("IZHW0060");

            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Single(translated.Statements);
            Assert.Equal("Workstation:(*IZHW0060)", translated.Statements.Single().Query);
        }

        [Fact]
        public void TestContains()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.Contains("IZHW006");

            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Single(translated.Statements);
            Assert.Equal("Workstation:(*IZHW006*)", translated.Statements.Single().Query);
        }

        #endregion


        [Fact]
        public void TestChaining()
        {
            var translator = new ExpressionToFtsRequestTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation == "EPRUIZHW006").Where(e => e.Manager.Equals("John"));


            FtsQueryRequest translated = translator.Translate(expression);

            Assert.Equal(2, translated.Statements.Count);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated.Statements[0].Query);
            Assert.Equal("Manager:(John)", translated.Statements[1].Query);
        }
    }
}
