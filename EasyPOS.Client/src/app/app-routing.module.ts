import { NgModule } from '@angular/core';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { AppLayoutComponent } from "./layout/app.layout.component";
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard, childAuthGuard } from './core/auth/guards/auth.guard';

const routes: Routes = [
    {
        path: '', component: AppLayoutComponent,
        children: [
            { path: '', loadChildren: () => import('./modules/dashboard/dashboard.module').then(m => m.DashboardModule), canActivateChild: [AuthGuard] },
            { path: 'pages', loadChildren: () => import('./demo/components/pages/pages.module').then(m => m.PagesModule) },
            { path: 'setup', loadChildren: () => import('./modules/common-setup/common-setup.module').then(m => m.CommonSetupModule), canActivateChild: [AuthGuard] },
            { path: 'admin', loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule), canActivateChild: [AuthGuard] },
            { path: 'product', loadChildren: () => import('./modules/products/products.module').then(m => m.ProductsModule), canActivateChild: [AuthGuard] },
            { path: 'stake', loadChildren: () => import('./modules/stakeholders/stakeholders.module').then(m => m.StakeholdersModule), canActivateChild: [AuthGuard] }
        ]
    },
    { path: 'auth', loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule) },
    { path: 'landing', loadChildren: () => import('./demo/components/landing/landing.module').then(m => m.LandingModule) },
    { path: 'notfound', component: NotfoundComponent },
    { path: '**', redirectTo: '/notfound' },
];

@NgModule({
    imports: [
       RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
