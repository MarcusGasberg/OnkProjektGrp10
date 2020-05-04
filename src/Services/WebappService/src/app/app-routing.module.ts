import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BuySellStockComponent } from './buy-sell-stock/buy-sell-stock.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'buysell', component: BuySellStockComponent },
];

@NgModule( {
  imports: [ RouterModule.forRoot( routes ) ],
  exports: [ RouterModule ]
} )
export class AppRoutingModule { }
