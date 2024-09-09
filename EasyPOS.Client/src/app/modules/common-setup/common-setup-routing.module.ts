import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LookupListComponent } from "./components/lookup-list/lookup-list.component";
import { LookupDetailComponent } from "./components/lookup-detail/lookup-detail.component";
import { LookupDetailListComponent } from "./components/lookup-detail-list/lookup-detail-list.component";
import { LookupDetailDetailComponent } from "./components/lookup-detail-detail/lookup-detail-detail.component";

const routes: Routes = [
  {path: 'lookups', component: LookupListComponent, data: {breadcrumb: 'Lookup'}},
  {path: 'lookup-details', component: LookupDetailListComponent, data: {breadcrumb: 'Lookup Detail'}}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommonSetupRoutingModule{}

export const CommonSetupRoutingComponents = [
  LookupListComponent,
  LookupDetailComponent,
  LookupDetailListComponent,
  LookupDetailDetailComponent
]