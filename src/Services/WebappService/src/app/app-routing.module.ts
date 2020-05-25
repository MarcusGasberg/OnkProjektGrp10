import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BuySellStockComponent } from './buy-sell-stock/buy-sell-stock.component';
import { AuthorizationGuard } from './auth/auth-guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'buysell',
    component: BuySellStockComponent,
    canActivate: [AuthorizationGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
