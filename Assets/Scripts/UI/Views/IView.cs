using Services;

public interface IView<TModel> : IService
{
    void UpdateViewWithModel(TModel model);
}
