namespace Lesson_1_2.Handlers
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(object request);
    }

    public abstract class AbstractHandler : IHandler
    {
        private IHandler NextHandler;

        public IHandler SetNext(IHandler handler)
        {
            NextHandler = handler;

            return handler;
        }

        public virtual object Handle(object request)
        {
            if (NextHandler != null)
            {
                return NextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }

}
