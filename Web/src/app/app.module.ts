import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { ServicesModule } from './services/services.module';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { ArbitrageComponent } from './arbitrage/arbitrage.component';
import { DetailComponent } from './arbitrage/detail/detail.component';

@NgModule({
    declarations: [
        AppComponent,
        ArbitrageComponent,
        DetailComponent
    ],
    imports: [
        BrowserModule,
        HttpModule,
        ServicesModule,
        AppRoutingModule,
        SharedModule,
        FormsModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
