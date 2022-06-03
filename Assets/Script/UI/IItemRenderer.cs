namespace Script.UI
{
    public interface IItemRenderer<T>
    {
        void SetDataInWidget(T localInfo);
    }
}