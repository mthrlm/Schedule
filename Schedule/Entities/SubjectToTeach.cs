using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Entities
{
    public class SubjectToTeach : Subject
    {
        private Teacher teacher;
        public Teacher SubjectTeacher { get { return teacher; } }

        public SubjectToTeach(string name, Teacher teacher) : base(name) 
        {
            this.teacher = teacher;
        }
    }
}
