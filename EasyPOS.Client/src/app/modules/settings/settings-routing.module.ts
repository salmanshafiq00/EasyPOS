import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CompanyInfoListComponent } from "./components/company-info-list/company-info-list.component";
import { CompanyInfoDetailComponent } from "./components/company-info-detail/company-info-detail.component";

const routes: Routes = [
  {path: 'company', component: CompanyInfoDetailComponent}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class SettingsRoutingModule{}

export const settingsRoutingComponents = [
  CompanyInfoListComponent,
  CompanyInfoDetailComponent,
]