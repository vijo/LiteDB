﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace LiteDB
{
    public partial class LiteCollection<T>
    {
        #region Count

        /// <summary>
        /// Get document count using property on collection.
        /// </summary>
        public int Count()
        {
            // do not use indexes - collections has DocumentCount property
            return this.Query().Count();
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public int Count(BsonExpression predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return this.Query().Where(predicate).Count();
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public int Count(string predicate, BsonDocument parameters) => this.Count(BsonExpression.Create(predicate, parameters));

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public int Count(string predicate, params BsonValue[] args) => this.Count(BsonExpression.Create(predicate, args));

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public int Count(Expression<Func<T, bool>> predicate) =>this.Count(_mapper.GetExpression(predicate));

        #endregion

        #region LongCount

        /// <summary>
        /// Get document count using property on collection.
        /// </summary>
        public long LongCount()
        {
            return this.Query().LongCount();
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public long LongCount(BsonExpression predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return this.Query().Where(predicate).LongCount();
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public long LongCount(string predicate, BsonDocument parameters) => this.LongCount(BsonExpression.Create(predicate, parameters));

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public long LongCount(string predicate, params BsonValue[] args) => this.LongCount(BsonExpression.Create(predicate, args));

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public long LongCount(Expression<Func<T, bool>> predicate) => this.LongCount(_mapper.GetExpression(predicate));

        #endregion

        #region Exists

        /// <summary>
        /// Returns true if query returns any document. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public bool Exists(BsonExpression predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return this.Query().Where(predicate).Exists();
        }

        /// <summary>
        /// Returns true if query returns any document. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public bool Exists(string predicate, BsonDocument parameters) => this.Exists(BsonExpression.Create(predicate, parameters));

        /// <summary>
        /// Returns true if query returns any document. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public bool Exists(string predicate, params BsonValue[] args) => this.Exists(BsonExpression.Create(predicate, args));

        /// <summary>
        /// Returns true if query returns any document. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public bool Exists(Expression<Func<T, bool>> predicate) => this.Exists(_mapper.GetExpression(predicate));

        #endregion

        #region Min/Max

        /// <summary>
        /// Returns the min value from specified key value in collection
        /// </summary>
        public BsonValue Min(BsonExpression keySelector)
        {
            if (string.IsNullOrEmpty(keySelector)) throw new ArgumentNullException(nameof(keySelector));

            return this.Query()
                .OrderBy(keySelector)
                .Select(keySelector)
                .ToDocuments()
                .First();
        }

        /// <summary>
        /// Returns the min value of _id index
        /// </summary>
        public BsonValue Min() => this.Min("_id");

        /// <summary>
        /// Returns the min value from specified key value in collection
        /// </summary>
        public K Min<K>(Expression<Func<T, K>> keySelector)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var expr = _mapper.GetExpression(keySelector);

            var value = this.Min(expr);

            return (K)_mapper.Deserialize(typeof(K), value);
        }

        /// <summary>
        /// Returns the max value from specified key value in collection
        /// </summary>
        public BsonValue Max(BsonExpression keySelector)
        {
            if (string.IsNullOrEmpty(keySelector)) throw new ArgumentNullException(nameof(keySelector));

            return this.Query()
                .OrderByDescending(keySelector)
                .Select(keySelector)
                .ToDocuments()
                .First();
        }

        /// <summary>
        /// Returns the max _id index key value
        /// </summary>
        public BsonValue Max() => this.Max("_id");

        /// <summary>
        /// Returns the last/max field using a linq expression
        /// </summary>
        public K Max<K>(Expression<Func<T, K>> keySelector)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var expr = _mapper.GetExpression(keySelector);

            var value = this.Max(expr);

            return (K)_mapper.Deserialize(typeof(K), value);
        }

        #endregion
    }
}