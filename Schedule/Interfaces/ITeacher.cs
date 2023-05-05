using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Interfaces
{
    internal interface ITeacher
    {
        string? Name { get; set; }
        string? MiddleName { get; set; }
        string? Surname { get; set; }
        List<ISubject> Subjects { get; set; }
        void AddSubject(ISubject subject);
        bool DeleteSubject(ISubject subject);
        void EditProperties(ITeacher teacher);
        void EditProperties(string? name, string? surname, string? middleName);
    }
}
