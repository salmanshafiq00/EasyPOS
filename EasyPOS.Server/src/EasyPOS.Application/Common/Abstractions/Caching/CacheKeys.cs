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
    public const string AppMenu_Tree_SelectList = nameof(AppMenu_Tree_SelectList);
    public const string AppMenu_Sidebar_Tree_List = nameof(AppMenu_Sidebar_Tree_List);
    public const string AppPage = nameof(AppPage);
    public const string AppPage_All_SelectList = nameof(AppPage_All_SelectList);
    public const string AppNotification = nameof(AppNotification);

    #endregion
}
