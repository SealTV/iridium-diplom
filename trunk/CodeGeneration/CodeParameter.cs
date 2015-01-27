using System;

namespace Tech.CodeGeneration
{
    public class CodeParameter
    {
        public CodeParameter(string name, Type type)
            : this(name, type, null)
        {
        }


        public CodeParameter(string name, Type type, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("name");

            if (type == null)
                throw new ArgumentNullException("type");

            Name = name;
            ParameterType = type;
            Value = value;
        }


        public static CodeParameter Create<T>(string name, T value)
        {
            return new CodeParameter(name, typeof(T), value);        
        }
    

        public string Name { get; private set; }
        public Type   ParameterType  { get; private set; }
        public object Value { get; private set; }
    }
}