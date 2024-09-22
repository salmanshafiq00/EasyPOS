namespace EasyPOS.Application.Common.Abstractions.Caching;

public static class CacheKeys
{
    public const string Lookup = nameof(Lookup);
    public const string Lookup_All_SelectList = nameof(Lookup_All_SelectList);

    public const string LookupDetail = nameof(LookupDetail);
    public const string LookupDetail_All_SelectList = nameof(LookupDetail_All_SelectList);

    #region Admin

    public const string AppUser = nameof(AppUser);
    public const string Role = nameof(Role);
    public const string Role_Permissions = nameof(Role_Permissions);
    public const string Role_All_SelectList = nameof(Role_All_SelectList);
    public const string AppMenu = nameof(AppMenu);
    public const string AppMenu_All_SelectList = nameof(AppMenu_All_SelectList);
    public const string AppMenu_Parent_SelectList = nameof(AppMenu_Parent_SelectList);
    public const string AppMenu_Tree_SelectList = nameof(AppMenu_Tree_SelectList);
    public const string AppMenu_Sidebar_Tree_List = nameof(AppMenu_Sidebar_Tree_List);
    public const string AppPage = nameof(AppPage);
    public const string AppPage_All_SelectList = nameof(AppPage_All_SelectList);
    public const string AppNotification = nameof(AppNotification);

    #endregion

    #region Products
    public const string Category = nameof(Category);
    public const string Category_All_SelectList = nameof(Category_All_SelectList);
    public const string Product = nameof(Product);
    public const string Product_All_SelectList = nameof(Product_All_SelectList);
    public const string Warehouse = nameof(Warehouse);
    public const string Warehouse_All_SelectList = nameof(Warehouse_All_SelectList);
    public const string Brand = nameof(Brand);
    public const string Brand_All_SelectList = nameof(Brand_All_SelectList);
    public const string Unit = nameof(Unit);
    public const string Unit_All_SelectList = nameof(Unit_All_SelectList);
    #endregion

    #region Stakeholders
    public const string Customer = nameof(Customer);
    public const string Customer_All_SelectList = nameof(Customer_All_SelectList);
    public const string CustomerGroup = nameof(CustomerGroup);
    public const string CustomerGroup_All_SelectList = nameof(CustomerGroup_All_SelectList);

    public const string Supplier = nameof(Supplier);
    public const string Supplier_All_SelectList = nameof(Supplier_All_SelectList);

    #endregion

    #region Trades

    public const string Purchase = nameof(Purchase);
    public const string Sale = nameof(Sale);

    #endregion
}
