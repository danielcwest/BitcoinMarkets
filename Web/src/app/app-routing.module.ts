import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ArbitrageComponent } from './arbitrage/arbitrage.component';
import { DetailComponent } from './arbitrage/detail/detail.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/arbitrage',
        pathMatch: 'full'
    },
    {
        path: 'arbitrage',
        component: ArbitrageComponent
    },
    {
        path: 'arbitrage/:pairId',
        component: DetailComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
