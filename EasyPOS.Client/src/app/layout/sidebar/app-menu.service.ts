import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { SidebarMenuModel } from "src/app/modules/generated-clients/api-service";

@Injectable()
export class AppMenuService{
  private sidebarMenus = new BehaviorSubject<SidebarMenuModel[]>([]);
  private selectedMenu = new BehaviorSubject<any>(null);

  setSidebarMenus(menus: SidebarMenuModel[]) {
    this.sidebarMenus.next(menus);
  }

  getSidebarMenus(): Observable<SidebarMenuModel[]> {
    return this.sidebarMenus.asObservable();
  }

  setSelectedMenu(menu: any){
    this.selectedMenu.next(menu);
  }

  getSelectedMenu(): Observable<any>{
    return this.selectedMenu.asObservable();
  }
}