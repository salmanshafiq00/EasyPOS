import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { UserListComponent } from "./components/user-list/user-list.component";
import { RoleListComponent } from "./components/role-list/role-list.component";
import { UserDetailComponent } from "./components/user-detail/user-detail.component";
import { RoleDetailComponent } from "./components/role-detail/role-detail.component";
import { AppMenuListComponent } from "./components/app-menu-list/app-menu-list.component";
import { AppMenuDetailComponent } from "./components/app-menu-detail/app-menu-detail.component";
import { AppPageDetailComponent } from "./components/app-page-detail/app-page-detail.component";
import { AppPageListComponent } from "./components/app-page-list/app-page-list.component";
import { UserProfileComponent } from "./components/user-profile/user-profile.component";
import { UserBasicComponent } from "./components/user-profile/components/user-basic/user-basic.component";
import { ChangePasswordComponent } from "./components/user-profile/components/change-password/change-password.component";
import { ChangeProfilePhotoComponent } from "./components/user-profile/components/change-profile-photo/change-profile-photo.component";

const routes: Routes = [
  {path: 'users', component: UserListComponent},
  {path: 'roles', component: RoleListComponent},
  {path: 'app-menus', component: AppMenuListComponent},
  {path: 'app-pages', component: AppPageListComponent},
  {path: 'user-profile', component: UserProfileComponent},
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class AdminRoutingModule{

}

export const adminRoutingComponents = [
  UserListComponent,
  UserDetailComponent,
  RoleListComponent,
  RoleDetailComponent,
  AppMenuListComponent,
  AppMenuDetailComponent,
  AppPageListComponent,
  AppPageDetailComponent,
  UserProfileComponent,
  UserBasicComponent,
  ChangePasswordComponent,
  ChangeProfilePhotoComponent

]