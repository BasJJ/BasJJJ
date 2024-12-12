using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.MVVM.Exceptions
{
    [Serializable]
    public class SomethingWentAllToShiiiiitException : System.Exception
    {
        public SomethingWentAllToShiiiiitException()
            : base("The cache entry is marked as permanent and cannot be overwritten.")
        {
        }

        public SomethingWentAllToShiiiiitException(string message)
            : base(message)
        {
        }

        public SomethingWentAllToShiiiiitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        protected SomethingWentAllToShiiiiitException(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context)
        {
        }
    }
}


