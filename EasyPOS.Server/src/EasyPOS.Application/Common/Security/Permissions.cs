namespace EasyPOS.Application.Common.Security;

public static class Permissions
{
    // These will generate all constant property value of particular module like Faculties
    public static List<string> GetPermissionsByfeature(string feature)
    {
        return new List<string>
            {
                 $"Permissions.{feature}.Create",
                 $"Permissions.{feature}.View",
                 $"Permissions.{feature}.Edit",
                 $"Permissions.{feature}.Delete"
            };
    }

    public static List<string> GetPermissionsByfeatures(List<string> features)
    {
        List<string> permissions = [];
        foreach (string feature in features)
        {
            permissions.AddRange(new List<string>
            {
                 $"Permissions.{feature}.Create",
                 $"Permissions.{feature}.View",
                 $"Permissions.{feature}.Edit",
                 $"Permissions.{feature}.Delete"
            });
        }
        return permissions;
    }

    /// <summary>
    /// These will generate list of modules name (string) like ApplicationUsers, IdentityRoles
    /// </summary>
    /// <returns>List<string></returns>
    public static List<string> GetAllNestedModule(Type type)
    {
        Type[] nestedTypes = type.GetNestedTypes();
        List<string> result = [];
        foreach (var nestedType in nestedTypes)
        {
            result.Add(nestedType.Name);
        }
        return result;
    }

    /// <summary>
    /// These will generate list of modules type (Type) like ApplicationUsers, IdentityRoles
    /// </summary>
    /// <returns>Type[]</returns>
    public static Type[] GetAllNestedModuleType(Type type)
    {
        Type[] nestedTypes = type.GetNestedTypes();
        return nestedTypes;
    }

    #region Admin

    public static class Admin
    {
        public static class ApplicationUsers
        {
            public const string View = "Permissions.ApplicationUsers.View";
            public const string Create = "Permissions.ApplicationUsers.Create";
            public const string Edit = "Permissions.ApplicationUsers.Edit";
            public const string Delete = "Permissions.ApplicationUsers.Delete";
        }
        public static class IdentityRoles
        {
            public const string View = "Permissions.IdentityRoles.View";
            public const string Create = "Permissions.IdentityRoles.Create";
            public const string Edit = "Permissions.IdentityRoles.Edit";
            public const string Delete = "Permissions.IdentityRoles.Delete";
        }

        public static class ManageUserRoles
        {
            public const string View = "Permissions.ManageUserRoles.View";
            public const string Create = "Permissions.ManageUserRoles.Create";
            public const string Edit = "Permissions.ManageUserRoles.Edit";
            public const string Delete = "Permissions.ManageUserRoles.Delete";
        }

        public static class ManageRoleClaims
        {
            public const string View = "Permissions.ManageRoleClaims.View";
            public const string Create = "Permissions.ManageRoleClaims.Create";
            public const string Edit = "Permissions.ManageRoleClaims.Edit";
            public const string Delete = "Permissions.ManageRoleClaims.Delete";
        }
        public static class AppMenus
        {
            public const string View = "Permissions.AppMenus.View";
            public const string Create = "Permissions.AppMenus.Create";
            public const string Edit = "Permissions.AppMenus.Edit";
            public const string Delete = "Permissions.AppMenus.Delete";
        }
        public static class AppPageSetting
        {
            public const string View = "Permissions.AppPageSetting.View";
            public const string Create = "Permissions.AppPageSetting.Create";
            public const string Edit = "Permissions.AppPageSetting.Edit";
            public const string Delete = "Permissions.AppPageSetting.Delete";
        }
    }

    #endregion

    #region Common

    public static class CommonSetup
    {
        public static class Lookups
        {
            public const string View = "Permissions.Lookups.View";
            public const string Create = "Permissions.Lookups.Create";
            public const string Edit = "Permissions.Lookups.Edit";
            public const string Delete = "Permissions.Lookups.Delete";
        }

        public static class LookupDetails
        {
            public const string View = "Permissions.LookupDetails.View";
            public const string Create = "Permissions.LookupDetails.Create";
            public const string Edit = "Permissions.LookupDetails.Edit";
            public const string Delete = "Permissions.LookupDetails.Delete";
        }

    }


    #endregion

    #region Products
    public static class Categories
    {
        public const string View = "Permissions.Categories.View";
        public const string Create = "Permissions.Categories.Create";
        public const string Edit = "Permissions.Categories.Edit";
        public const string Delete = "Permissions.Categories.Delete";
    }

    public static class Products
    {
        public const string View = "Permissions.Products.View";
        public const string Create = "Permissions.Products.Create";
        public const string Edit = "Permissions.Products.Edit";
        public const string Delete = "Permissions.Products.Delete";
    }

    public static class Warehouses
    {
        public const string View = "Permissions.Warehouses.View";
        public const string Create = "Permissions.Warehouses.Create";
        public const string Edit = "Permissions.Warehouses.Edit";
        public const string Delete = "Permissions.Warehouses.Delete";
    }

    public static class Brands
    {
        public const string View = "Permissions.Brands.View";
        public const string Create = "Permissions.Brands.Create";
        public const string Edit = "Permissions.Brands.Edit";
        public const string Delete = "Permissions.Brands.Delete";
    }

    public static class Units
    {
        public const string View = "Permissions.Units.View";
        public const string Create = "Permissions.Units.Create";
        public const string Edit = "Permissions.Units.Edit";
        public const string Delete = "Permissions.Units.Delete";
    }

    public static class Taxes
    {
        public const string View = "Permissions.Taxes.View";
        public const string Create = "Permissions.Taxes.Create";
        public const string Edit = "Permissions.Taxes.Edit";
        public const string Delete = "Permissions.Taxes.Delete";
    }
    #endregion

    #region Stakeholders
    public static class Customers
    {
        public const string View = "Permissions.Customers.View";
        public const string Create = "Permissions.Customers.Create";
        public const string Edit = "Permissions.Customers.Edit";
        public const string Delete = "Permissions.Customers.Delete";
    }

    public static class CustomerGroups
    {
        public const string View = "Permissions.CustomerGroups.View";
        public const string Create = "Permissions.CustomerGroups.Create";
        public const string Edit = "Permissions.CustomerGroups.Edit";
        public const string Delete = "Permissions.CustomerGroups.Delete";
    }

    public static class Suppliers
    {
        public const string View = "Permissions.Suppliers.View";
        public const string Create = "Permissions.Suppliers.Create";
        public const string Edit = "Permissions.Suppliers.Edit";
        public const string Delete = "Permissions.Suppliers.Delete";
    }

    #endregion

    #region Trades
    public static class Purchases
    {
        public const string View = "Permissions.Purchases.View";
        public const string Create = "Permissions.Purchases.Create";
        public const string Edit = "Permissions.Purchases.Edit";
        public const string Delete = "Permissions.Purchases.Delete";
    }

    public static class PurchasePayments
    {
        public const string View = "Permissions.PurchasePayments.View";
        public const string Create = "Permissions.PurchasePayments.Create";
        public const string Edit = "Permissions.PurchasePayments.Edit";
        public const string Delete = "Permissions.PurchasePayments.Delete";
    }

    public static class Sales
    {
        public const string View = "Permissions.Sales.View";
        public const string Create = "Permissions.Sales.Create";
        public const string Edit = "Permissions.Sales.Edit";
        public const string Delete = "Permissions.Sales.Delete";
    }
    #endregion

    #region Accounts
    public static class Accounts
    {
        public const string View = "Permissions.Accounts.View";
        public const string Create = "Permissions.Accounts.Create";
        public const string Edit = "Permissions.Accounts.Edit";
        public const string Delete = "Permissions.Accounts.Delete";
    }

    public static class MoneyTransfers
    {
        public const string View = "Permissions.MoneyTransfers.View";
        public const string Create = "Permissions.MoneyTransfers.Create";
        public const string Edit = "Permissions.MoneyTransfers.Edit";
        public const string Delete = "Permissions.MoneyTransfers.Delete";
    }

    #endregion

    #region Settings
    public static class CompanyInfos
    {
        public const string View = "Permissions.CompanyInfos.View";
        public const string Create = "Permissions.CompanyInfos.Create";
        public const string Edit = "Permissions.CompanyInfos.Edit";
        public const string Delete = "Permissions.CompanyInfos.Delete";
    }
    #endregion
}
