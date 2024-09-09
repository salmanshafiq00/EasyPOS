namespace EasyPOS.Application.Common.Constants.CommonSqlConstants;

public static class SelectListSqls
{
    public const string GetLookupSelectListSql = """
        SELECT Id, Name 
        FROM dbo.Lookups l
        WHERE 1 = 1
        ORDER BY Name
        """;

    public const string GetLookupParentSelectListSql = """
        SELECT l.Id, l.Name 
        FROM dbo.Lookups l
        INNER JOIN dbo.Lookups l2 ON l2.ParentId = l.Id
        WHERE 1 = 1
        ORDER BY l.Name
        """;

    public const string GetLookupDetailSelectListSql = """
        SELECT Id, Name 
        FROM dbo.LookupDetails ld
        WHERE 1 = 1
        ORDER BY Name
        """;

    public const string GetLookupDetailParentSelectListSql = """
        SELECT Distinct l.Id, l.Name 
        FROM dbo.LookupDetails l
        INNER JOIN dbo.LookupDetails l2 ON l2.ParentId = l.Id
        WHERE 1 = 1
        ORDER BY l.Name
        """;

    public const string GetLookupDetailSelectListByDevCodeSql = """
        SELECT ld.Id, ld.Name 
        FROM dbo.Lookups l
        INNER JOIN dbo.LookupDetails ld ON ld.LookupId = l.Id
        WHERE 1 = 1
        AND l.DevCode = @DevCode
        ORDER BY ld.Name
        """;

    public const string GetRoleSelectListSql = """
        SELECT Id, Name 
        FROM [identity].Roles r
        WHERE 1 = 1
        ORDER BY Name
        """;

    public const string GetAppMenuSelectListSql = """
        SELECT Id, Label AS Name
        FROM [dbo].AppMenus 
        WHERE 1 = 1
        ORDER BY Label
        """;
}
