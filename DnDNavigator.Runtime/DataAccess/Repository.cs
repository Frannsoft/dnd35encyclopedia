using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.DataAccess
{
    [DataContract(Name = "Repository")]
    public class Repository<T> : IList<T>
        where T : class
    {
        [DataMember]
        private List<T> repo;

        public Repository()
        {
            repo = new List<T>();
        }


        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(T item)
        {
            repo.Add(item);
            IsolatedStorage.IsoDataChanged();
        }

        public void AddRange(IList<T> items)
        {
            foreach(T item in items)
            {
                if(item != null)
                {
                    repo.Add(item);
                }
            }
        }

        public void Clear()
        {
            repo.Clear();
        }

        public bool Contains(T item)
        {
            bool result = false;
            if(repo.Contains<T>(item))
            {
                result = true;
            }
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return repo.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            bool success = false;
            if(repo.Contains(item))
            {
                repo.Remove(item);
                IsolatedStorage.IsoDataChanged();
                success = true;
            }
            return success;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return repo.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
