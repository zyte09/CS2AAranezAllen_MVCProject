namespace CS2AAranezAllen_MVCProject.BusLogic.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);


    }
}
