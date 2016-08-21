using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VocaloidRadioUWP.Model
{
    [DataContract]
    public class ListWithMaximumSize<T>:IEnumerable<T>
    {
        [DataMember]
        private const int Maximum = 10;
        [DataMember]

        private List<T> _list = new List<T>();

        public bool IsEmpty => _list.Count == 0;

        public ListWithMaximumSize()
        {
            
        }

        public void AddRangeWithDistinct(IEnumerable<T> enumerable)
        {
            _list.AddRange(enumerable);
            _list.Sort();
            _list = _list.Distinct().ToList();
            if (_list.Count>Maximum)
            {
                _list = _list.Take(Maximum).ToList();
            }

        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
