using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading;

namespace EDCWebApp.Tests
{
    //based on http://www.asp.net/web-api/overview/testing-and-debugging/mocking-entity-framework-when-unit-testing-aspnet-web-api-2
    public class TestDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T>, IDbAsyncEnumerable<T>
        where T: class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public TestDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public override T Add(T entity)
        {
            _data.Add(entity);

            return entity;
        }

        public override T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }
        public override T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }
        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }
        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }
        public override ObservableCollection<T> Local
        {
            get
            {
                return new ObservableCollection<T>(_data);
            }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }
        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }
        //IQueryProvider IQueryable.Provider
        //{
        //    get { return _query.Provider; }
        //}

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(_query.Provider); }
        } 
        IDbAsyncEnumerator<T> IDbAsyncEnumerable<T>.GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(_data.GetEnumerator());
        } 
    }

    internal class TestDbAsyncQueryProvider<T> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }
        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }
        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }
        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }

    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            :base(enumerable)
        {

        }
        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        {

        }
        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }
}
