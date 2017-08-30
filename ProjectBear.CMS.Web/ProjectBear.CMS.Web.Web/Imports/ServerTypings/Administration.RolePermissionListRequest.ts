namespace ProjectBear.CMS.Web.Administration {
    export interface RolePermissionListRequest extends Serenity.ServiceRequest {
        RoleID?: number;
        Module?: string;
        Submodule?: string;
    }
}

