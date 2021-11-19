//@BaseCode
//MdStart
using System;
using System.Reflection;

namespace CSharpCodeGenerator.Logic.Models
{
    internal class Relation
    {
        public string Name => Reference.Name;
        public Type From { get; private set; }
        public Type To { get; private set; }
        public PropertyInfo Reference { get; private set; }

        public Relation(Type from, Type to, PropertyInfo reference)
        {
            from.CheckArgument(nameof(from));
            to.CheckArgument(nameof(to));
            reference.CheckArgument(nameof(reference));

            From = from;
            To = to;
            Reference = reference;
        }
    }
}
//MdEnd