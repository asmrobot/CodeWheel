using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Infrastructure.DB
{
    /// <summary>
    /// 行无数据集合
    /// </summary>
    public class ColumnMetaCollection : IEnumerable<ColumnMeta>
    {
        public ColumnMetaCollection()
        {

        }

        public ColumnMetaCollection(IEnumerable<ColumnMeta> columns)
        {
            foreach (var item in columns)
            {
                if (!this.list.ContainsKey(item.ColumnName))
                {
                    list.Add(item.ColumnName, item);
                }
            }
        }

        /// <summary>
        /// 得到没有主键的集合
        /// </summary>
        public ColumnMetaCollection GetNoKeyCollection()
        {
            ColumnMetaCollection collection = new ColumnMetaCollection();
            foreach (var item in this)
            {
                if (!item.IsKey)
                {
                    collection.Add(item);
                }
            }
            return collection;
        }


        /// <summary>
        /// 得到主键的集合
        /// </summary>
        public ColumnMetaCollection GetKeyCollection()
        {
            ColumnMetaCollection collection = new ColumnMetaCollection();
            foreach (var item in this)
            {
                if (item.IsKey)
                {
                    collection.Add(item);
                }
            }
            return collection;
        }




        /// <summary>
        /// 得到所有非主键列
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ColumnMetaCollection GetNoPrimaryKeyColumns()
        {
            return new ColumnMetaCollection(this.Where((col) => { return !col.IsKey; }));

        }

        /// <summary>
        /// 得到所有非主键列
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ColumnMetaCollection GetPrimaryKeyColumns()
        {
            return new ColumnMetaCollection(this.Where((col) => { return col.IsKey; }));
        }


        /// <summary>
        /// 得到所有非主键列
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ColumnMeta GetPrimaryKeyColumn()
        {
            return this.Where((col) => { return col.IsKey; }).FirstOrDefault();
        }






        private Dictionary<string, ColumnMeta> list = new Dictionary<string, ColumnMeta>();

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public void Add(ColumnMeta meta)
        {
            if (meta == null)
            {
                return;
            }
            if (list.ContainsKey(meta.ColumnName))
            {
                return;
            }

            list.Add(meta.ColumnName, meta);
        }

        public void Remove(string fieldName)
        {
            if (!list.ContainsKey(fieldName))
            {
                return;
            }

            list.Remove(fieldName);
        }

        public ColumnMeta this[string fieldName]
        {
            get
            {
                if (list.ContainsKey(fieldName))
                {
                    return list[fieldName];
                }
                return null;
            }
        }

        public IEnumerator<ColumnMeta> GetEnumerator()
        {
            return this.list.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.list.Values.GetEnumerator();
        }
    }
}
