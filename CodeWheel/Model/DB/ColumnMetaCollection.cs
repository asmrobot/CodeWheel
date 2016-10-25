using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model.DB
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
                if (!this.m_list.ContainsKey(item.ColumnName))
                {
                    m_list.Add(item.ColumnName, item);
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



        private Dictionary<string, ColumnMeta> m_list = new Dictionary<string, ColumnMeta>();

        public int Count
        {
            get
            {
                return m_list.Count;
            }
        }

        public void Add(ColumnMeta meta)
        {
            if (meta == null)
            {
                return;
            }
            if (m_list.ContainsKey(meta.ColumnName))
            {
                return;
            }

            m_list.Add(meta.ColumnName, meta);
        }

        public void Remove(string fieldName)
        {
            if (!m_list.ContainsKey(fieldName))
            {
                return;
            }

            m_list.Remove(fieldName);
        }

        public ColumnMeta this[string fieldName]
        {
            get
            {
                if (m_list.ContainsKey(fieldName))
                {
                    return m_list[fieldName];
                }
                return null;
            }
        }

        public IEnumerator<ColumnMeta> GetEnumerator()
        {
            return this.m_list.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.m_list.Values.GetEnumerator();
        }
    }
}
