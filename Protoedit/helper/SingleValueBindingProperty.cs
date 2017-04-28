using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protoedit.helper
{
    public class SingleValueBindingProperty<T> : NotifyPropertyChanged
    {
        private T _value = default(T);

        public SingleValueBindingProperty()
        {
        }

        public SingleValueBindingProperty(T val)
        {
            this._value = val;
        }

        public T Value
        {
            get { return _value; }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                    return;

                _value = value;
                OnPropertyChanged("Value");
            }
        }

      /*  public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }*/
    }
}
