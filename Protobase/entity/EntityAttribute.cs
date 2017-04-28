using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Protobase.entity
{
    public delegate void ValueChangedEventHandler(EntityAttribute sender, ValueChangedEvent changeEvent);

    public class ValueChangedEvent
    {
        public string ValueName { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }


        internal ValueChangedEvent(string valueName, object oldValue, object newValue)
        {
            this.ValueName = valueName;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    public abstract class EntityAttribute
    {
        public event ValueChangedEventHandler OnValueChanged;

        protected void FireValueChangedEvent(string propertyName,object oldValue, object newValue)
        {
            if (this.OnValueChanged != null)
            {
                this.OnValueChanged(this, new ValueChangedEvent(propertyName, oldValue, newValue));
            }
        }
    }
}
