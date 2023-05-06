using Schedule.Entities;

namespace Schedule.Interfaces
{
    public interface IView
    {
        event EventHandler<object>? OnClose;
        event EventHandler<object>? OnAdd;
        event EventHandler<object>? OnDelete;
        void ShowForm(object sender);
        void HideForm(object sender);
        void Update(object view, object args, string type);
    }
}
