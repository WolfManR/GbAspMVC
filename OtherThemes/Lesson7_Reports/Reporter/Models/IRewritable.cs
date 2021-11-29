namespace Reporter.Models
{
    public interface IRewritable<in TImportData>
    {
        void Rewrite(TImportData importData);
    }
}