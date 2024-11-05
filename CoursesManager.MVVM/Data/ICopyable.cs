using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.MVVM.Data
{
    public interface ICopyable<out T>
    {
        T Copy();
    }
}
