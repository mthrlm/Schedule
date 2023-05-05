using Schedule.Interfaces;

namespace Schedule
{
    public partial class Form1 : Form, IView
    {
        private readonly Presenter? presenter;

        public Form1()
        {
            InitializeComponent();
            //presenter = new Presenter(this);
        }
    }
}