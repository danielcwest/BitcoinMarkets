import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { ArbitrageComponent } from './arbitrage/arbitrage.component';
import { DetailComponent } from './arbitrage/detail/detail.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full'
    },
    {
        path: 'dashboard',
        component: DashboardComponent
    },
    {
        path: 'arbitrage',
        component: ArbitrageComponent
    },
    {
        path: 'arbitrage/:ticker',
        component: DetailComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }