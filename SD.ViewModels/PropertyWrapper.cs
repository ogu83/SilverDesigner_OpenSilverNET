using SilverDesigner.DOM;
using System;
using System.Reflection;
using System.Windows.Media;

namespace SilverDesigner
{
    public class PropertyWrapper
    {
        private readonly object target;
        private readonly PropertyInfo property;

        public PropertyWrapper(object target, PropertyInfo property)
        {
            this.target = target;
            this.property = property;
            this.PropertyName = property.Name;
        }

        public object Value
        {
            get
            {
                if (property.PropertyType == typeof(MarginVM))
                    return ((MarginVM)property.GetValue(target, null)).ToString();
                else if (property.PropertyType == typeof(PagePositionVM))
                    return ((PagePositionVM)property.GetValue(target, null)).ToString();
                else if (property.PropertyType == typeof(bool))
                    return ((bool)property.GetValue(target, null)).ToInt();
                else
                    return (object)property.GetValue(target, null);
            }
            set
            {
                object myValue = value;

                Type myType = this.property.PropertyType;
                if (myType == typeof(double))
                    myValue = (value as string).ToDouble();
                else if (myType == typeof(int))
                    myValue = (value as string).ToInt();
                else if (myType == typeof(MarginVM))
                    myValue = MarginVM.FromString(value as string);
                else if (myType == typeof(PagePositionVM))
                    myValue = PagePositionVM.FromString(value as string);
                else if (myType == typeof(bool))
                    myValue = (value as string).ToInt().ToBoolean();
                else if (myType == typeof(Color))
                    myValue = (value as string).ToColor();

                property.SetValue(target, myValue, null);
            }
        }

        public PropertyInfo Property { get { return this.property; } }

        public string PropertyName { get; private set; }
    }
}
