import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BuySellStockComponent } from './buy-sell-stock/buy-sell-stock.component';
import { AuthorizationGuard } from './auth/auth-guard';
import { AccountComponent } from './account/account.component';
import { SellstockComponent } from './sellstock/sellstock.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'buy',
    component: BuySellStockComponent,
    canActivate: [AuthorizationGuard],
  },
  {
    path: 'account',
    component: AccountComponent,
    canActivate: [AuthorizationGuard],
  },
  {
    path: 'sell',
    component: SellstockComponent,
    canActivate: [AuthorizationGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
